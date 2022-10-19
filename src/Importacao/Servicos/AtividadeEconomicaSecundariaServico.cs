using Importacao.Modelos;
using Importacao.Repositorios;
using System.Text;

namespace Importacao.Servicos
{
    public class AtividadeEconomicaSecundariaServico
    {
        private readonly AtividadeEconomicaSecundariaRepositorio AtividadeEconomicaSecundariaRepositorio;
        private readonly AtividadeEconomicaRepositorio AtividadeEconomicaRepositorio;
        public bool VerificarSeJaExiste { get; set; }

        public AtividadeEconomicaSecundariaServico(AtividadeEconomicaSecundariaRepositorio atividadeEconomicaSecundariaRepositorio,
            AtividadeEconomicaRepositorio atividadeEconomicaRepositorio)
        {
            AtividadeEconomicaSecundariaRepositorio = atividadeEconomicaSecundariaRepositorio;
            AtividadeEconomicaRepositorio = atividadeEconomicaRepositorio;
        }

        public void ProcessaAtividadeEconomicaSecundaria(string registro)
        {
            var Cnpj = registro.Substring(3, 14).Trim();
            var CnaeSecundaria = registro.Substring(17, 693).Trim();
            string AtividadeEconomicaCd;
            int AtividadeEconomicaId;

            if (!string.IsNullOrWhiteSpace(CnaeSecundaria))
            {
                var tamanho = CnaeSecundaria.Length;
                var CnaeInserida = new StringBuilder();

                while (tamanho >= 7)
                {
                    if (tamanho > 7)
                    {
                        AtividadeEconomicaCd = CnaeSecundaria[..7];
                        CnaeSecundaria = CnaeSecundaria[7..tamanho];
                    }
                    else
                    {
                        AtividadeEconomicaCd = CnaeSecundaria.Trim();
                        CnaeSecundaria = "";
                    }
                    tamanho = CnaeSecundaria.Length;

                    if (AtividadeEconomicaCd.Length == 7 &&
                        AtividadeEconomicaCd != "0000000" &&
                        !CnaeInserida.ToString().Contains(AtividadeEconomicaCd, System.StringComparison.CurrentCulture))
                    {

                        var atividadeEconomica = AtividadeEconomicaRepositorio.ObtemPorCodigo(AtividadeEconomicaCd);

                        if (atividadeEconomica == null)
                        {
                            atividadeEconomica = new AtividadeEconomica
                            {
                                Codigo = AtividadeEconomicaCd,
                                Descricao = AtividadeEconomicaCd
                            };
                            AtividadeEconomicaId = AtividadeEconomicaRepositorio.Insere(atividadeEconomica);
                        }
                        else
                        {
                            AtividadeEconomicaId = atividadeEconomica.Id;
                        }

                        var atividadeEconomicaSecundaria = new AtividadeEconomicaSecundaria
                        {
                            Cnpj = Cnpj,
                            AtividadeEconomicaId = AtividadeEconomicaId
                        };

                        if (VerificarSeJaExiste)
                        {
                            if (AtividadeEconomicaSecundariaRepositorio.JaExite(atividadeEconomicaSecundaria) == -1)
                                AtividadeEconomicaSecundariaRepositorio.Insere(atividadeEconomicaSecundaria);
                        }
                        else
                        {
                            AtividadeEconomicaSecundariaRepositorio.Insere(atividadeEconomicaSecundaria);
                        }

                        CnaeInserida.Append(AtividadeEconomicaCd);
                    }

                }
            }
        }
    }
}
