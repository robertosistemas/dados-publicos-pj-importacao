using Importacao.Auxiliares;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Importacao
{
    public static class Program
    {
        static int Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();
            var configuracaoBaseDados = new ConfiguracaoBaseDados();
            configuration.GetSection("ConfiguracaoBaseDados").Bind(configuracaoBaseDados);

            string arquivoOrigem;
            string arquivoDestino;

#if DEBUG
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                arquivoOrigem = "/opt/databases/K3241.K03200DV.D00904.L00001";
                arquivoDestino = "/opt/databases/RECEITA.FDB";
            }
            else
            {
                arquivoOrigem = "C:\\Receita\\K3241.K03200DV.D00904.L00001";
                arquivoDestino = "C:\\Receita\\RECEITA.FDB";
            }
#endif

            if (args.Length > 0)
            {
                arquivoOrigem = args[0];
            }

            if (args.Length > 1)
            {
                arquivoDestino = args[1];
            }

            if (!File.Exists(arquivoOrigem))
            {
                Console.WriteLine($"O arquivo de origem informado: {arquivoOrigem}, não existe!");
                Console.WriteLine($"Pressione qualquer tecla para finalizar o programa...");
                Console.ReadKey();
                return 1;
            }

            if (configuracaoBaseDados.TipoConexao == TipoConexao.Embarcada)
            {
                if (!File.Exists(arquivoDestino))
                {
                    Console.WriteLine($"O arquivo de destino informado: {arquivoDestino}, não existe!");
                    Console.WriteLine($"Pressione qualquer tecla para finalizar o programa...");
                    Console.ReadKey();
                    return 1;
                }
            }

            Console.WriteLine($"Importando dados do arquivo: {arquivoOrigem}...");

            var databaseUtil = new DatabaseUtil(arquivoOrigem, arquivoDestino, configuracaoBaseDados);
            databaseUtil.ExtrairFirebird();
            databaseUtil.ImportaDados();
            Console.Beep();
            Console.WriteLine($"Quantidade de registros processados: {databaseUtil.QuantidadeImportada}, Quantidade de segundos: {databaseUtil.TempoUtilizado}");
            Console.WriteLine($"Pressione qualquer tecla para finalizar o programa...");
            Console.ReadKey();
            return 0;
        }
    }
}