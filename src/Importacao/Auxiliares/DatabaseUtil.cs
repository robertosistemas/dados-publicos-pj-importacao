using FirebirdSql.Data.FirebirdClient;
using Importacao.Repositorios;
using Importacao.Servicos;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Importacao.Auxiliares
{
    public class DatabaseUtil
    {
        private string Caminho;
        private string Arquitetura;
        private readonly string ArquivoOrigem;
        private readonly string ArquivoDestino;
        private readonly ConfiguracaoBaseDados ConfiguracaoBaseDados;

        public DatabaseUtil(string arquivoOrigem, string arquivoDestino, ConfiguracaoBaseDados configuracaoBaseDados)
        {
            ArquivoOrigem = arquivoOrigem;
            ArquivoDestino = arquivoDestino;
            ConfiguracaoBaseDados = configuracaoBaseDados;
            ObterInformacoes();
        }

        private void ObterInformacoes()
        {
            Arquitetura = RuntimeInformation.ProcessArchitecture == Architecture.X64 ? "x64" : "x86";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Caminho = "/opt/databases";
            }
            else
            {
                var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
                Caminho = directoryInfo.FullName;
#if DEBUG
                if (!File.Exists($"{Caminho}\\Importacao.csproj"))
                {
                    if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    {
                        Caminho = directoryInfo.Parent.Parent.Parent.FullName;
                    }
                    else
                    {
                        Caminho = directoryInfo.Parent.Parent.Parent.Parent.FullName;
                    }
                }
#endif
            }
        }

        public void ExtrairFirebird()
        {
            if (ConfiguracaoBaseDados.TipoConexao == TipoConexao.Embarcada && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Environment.SetEnvironmentVariable("FIREBIRD", $"{Caminho}\\{Arquitetura}");
                Environment.SetEnvironmentVariable("FIREBIRD_MSG", $"{Caminho}\\{Arquitetura}");

                var fileName = $"{Caminho}\\{Arquitetura}.zip";
                var folderName = $"{Caminho}";
                if (!Directory.Exists($"{folderName}\\{Arquitetura}"))
                {
                    ZipFile.ExtractToDirectory(fileName, folderName, true);
                }
                var fileInfo = new FileInfo(ArquivoDestino);
                if (!fileInfo.Directory.Exists)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }
                if (!File.Exists(fileInfo.FullName))
                {
                    ZipFile.ExtractToDirectory($"{Caminho}\\RECEITA.zip", fileInfo.Directory.FullName, true);
                }
            }
        }

        public int QuantidadeImportada { get; private set; }
        public long TempoUtilizado { get; private set; }

        private string ObtemStringConexao()
        {
            string stringConexaoTemplate;

            if (ConfiguracaoBaseDados.TipoConexao == TipoConexao.Embarcada)
            {
                stringConexaoTemplate = ConfiguracaoBaseDados.TemplateConexaoEmbarcada;
                stringConexaoTemplate = stringConexaoTemplate.Replace("{ArquivoDestino}", ArquivoDestino);
                stringConexaoTemplate = stringConexaoTemplate.Replace("{Caminho}", Caminho);
                stringConexaoTemplate = stringConexaoTemplate.Replace("{Arquitetura}", Arquitetura);
            }
            else
            {
                stringConexaoTemplate = ConfiguracaoBaseDados.TemplateConexaoRemota;
            }

            return stringConexaoTemplate;
        }

        public void ImportaDados()
        {
            var stopwatch = Stopwatch.StartNew();
            int conta = 0;

            using var fs = File.Open(ArquivoOrigem, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var bs = new BufferedStream(fs);
            using var sr = new StreamReader(bs);

            using var connection = new FbConnection(ObtemStringConexao());
            connection.Open();

            using var matrizFilialRepositorio = new MatrizFilialRepositorio(connection);
            using var situacaoCadastralRepositorio = new SituacaoCadastralRepositorio(connection);
            using var motivoSituacaoCadastralRepositorio = new MotivoSituacaoCadastralRepositorio(connection);
            using var paisRepositorio = new PaisRepositorio(connection);
            using var naturezaJuridicaRepositorio = new NaturezaJuridicaRepositorio(connection);
            using var atividadeEconomicaRepositorio = new AtividadeEconomicaRepositorio(connection);
            using var unidadeFederacaoRepositorio = new UnidadeFederacaoRepositorio(connection);
            using var municipioRepositorio = new MunicipioRepositorio(connection);
            using var qualificacaoRepositorio = new QualificacaoRepositorio(connection);
            using var porteRepositorio = new PorteRepositorio(connection);
            using var opcaoSimplesRepositorio = new OpcaoSimplesRepositorio(connection);

            using var dadoCadastralRepositorio = new DadoCadastralRepositorio(connection);

            var dadoCadastralServico = new DadoCadastralServico(dadoCadastralRepositorio,
                matrizFilialRepositorio,
                situacaoCadastralRepositorio,
                motivoSituacaoCadastralRepositorio,
                paisRepositorio,
                naturezaJuridicaRepositorio,
                atividadeEconomicaRepositorio,
                unidadeFederacaoRepositorio,
                municipioRepositorio,
                qualificacaoRepositorio,
                porteRepositorio,
                opcaoSimplesRepositorio)
            {
                VerificarSeJaExiste = ConfiguracaoBaseDados.VerificarSeJaExiste
            };

            using var atividadeEconomicaSecundariaRepositorio = new AtividadeEconomicaSecundariaRepositorio(connection);
            var atividadeEconomicaSecundariaServico = new AtividadeEconomicaSecundariaServico(atividadeEconomicaSecundariaRepositorio, atividadeEconomicaRepositorio)
            {
                VerificarSeJaExiste = ConfiguracaoBaseDados.VerificarSeJaExiste
            };

            bool emTransacao = false;
            FbTransaction transaction = null;
            string tipo;

            var linha = sr.ReadLine();

            while (linha != null)
            {
                if (!emTransacao)
                {
                    transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                    matrizFilialRepositorio.SetTransaction(transaction);
                    situacaoCadastralRepositorio.SetTransaction(transaction);
                    motivoSituacaoCadastralRepositorio.SetTransaction(transaction);
                    paisRepositorio.SetTransaction(transaction);
                    naturezaJuridicaRepositorio.SetTransaction(transaction);
                    atividadeEconomicaRepositorio.SetTransaction(transaction);
                    unidadeFederacaoRepositorio.SetTransaction(transaction);
                    municipioRepositorio.SetTransaction(transaction);
                    qualificacaoRepositorio.SetTransaction(transaction);
                    porteRepositorio.SetTransaction(transaction);
                    opcaoSimplesRepositorio.SetTransaction(transaction);

                    dadoCadastralRepositorio.SetTransaction(transaction);
                    atividadeEconomicaSecundariaRepositorio.SetTransaction(transaction);
                    emTransacao = true;
                }

                tipo = linha.Substring(0, 1);

                if (tipo == "0")
                {
                    //CONTEM O VALOR 0 PARA IDENTIFICAR O REGISTRO HEADER
                }

                if (tipo == "1")
                {
                    //CONTEM O VALOR 1 PARA IDENTIFICAR O REGISTRO DETALHE
                    dadoCadastralServico.ProcessaDadoCadastral(linha);
                    conta++;
                }

                if (tipo == "2")
                {
                    //CONTEM O VALOR 2 PARA IDENTIFICAR O REGISTRO DETALHE SOCIOS
                }

                if (tipo == "6")
                {
                    //CONTEM O VALOR 6 PARA IDENTIFICAR O REGISTRO CNAEs SECUNDÁRIOS
                    atividadeEconomicaSecundariaServico.ProcessaAtividadeEconomicaSecundaria(linha);
                    conta++;
                }

                if (tipo == "9")
                {
                    //CONTEM O VALOR 9 PARA IDENTIFICAR O REGISTRO TRAILLER
                }

                if (conta % 10000 == 0 && emTransacao)
                {
                    transaction.Commit();
                    emTransacao = false;
                }

                linha = sr.ReadLine();

            }

            if (emTransacao)
            {
                transaction.Commit();
            }

            QuantidadeImportada = conta;
            stopwatch.Stop();
            this.TempoUtilizado = (long)stopwatch.Elapsed.TotalSeconds;
        }
    }
}
