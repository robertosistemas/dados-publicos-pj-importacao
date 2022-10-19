using Importacao.Auxiliares;
using Importacao.Modelos;
using Importacao.Repositorios;

namespace Importacao.Servicos
{
    public class DadoCadastralServico
    {
        private readonly DadoCadastralRepositorio DadoCadastralRepositorio;
        private readonly MatrizFilialRepositorio MatrizFilialRepositorio;
        private readonly SituacaoCadastralRepositorio SituacaoCadastralRepositorio;
        private readonly MotivoSituacaoCadastralRepositorio MotivoSituacaoCadastralRepositorio;
        private readonly PaisRepositorio PaisRepositorio;
        private readonly NaturezaJuridicaRepositorio NaturezaJuridicaRepositorio;
        private readonly AtividadeEconomicaRepositorio AtividadeEconomicaRepositorio;
        private readonly UnidadeFederacaoRepositorio UnidadeFederacaoRepositorio;
        private readonly MunicipioRepositorio MunicipioRepositorio;
        private readonly QualificacaoRepositorio QualificacaoRepositorio;
        private readonly PorteRepositorio PorteRepositorio;
        private readonly OpcaoSimplesRepositorio OpcaoSimplesRepositorio;
        public bool VerificarSeJaExiste { get; set; }

        public DadoCadastralServico(DadoCadastralRepositorio dadoCadastralRepositorio,
            MatrizFilialRepositorio matrizFilialRepositorio,
            SituacaoCadastralRepositorio situacaoCadastralRepositorio,
            MotivoSituacaoCadastralRepositorio motivoSituacaoCadastralRepositorio,
            PaisRepositorio paisRepositorio,
            NaturezaJuridicaRepositorio maturezaJuridicaRepositorio,
            AtividadeEconomicaRepositorio atividadeEconomicaRepositorio,
            UnidadeFederacaoRepositorio unidadeFederacaoRepositorio,
            MunicipioRepositorio municipioRepositorio,
            QualificacaoRepositorio qualificacaoRepositorio,
            PorteRepositorio porteRepositorio,
            OpcaoSimplesRepositorio opcaoSimplesRepositorio)
        {
            DadoCadastralRepositorio = dadoCadastralRepositorio;
            MatrizFilialRepositorio = matrizFilialRepositorio;
            SituacaoCadastralRepositorio = situacaoCadastralRepositorio;
            MotivoSituacaoCadastralRepositorio = motivoSituacaoCadastralRepositorio;
            PaisRepositorio = paisRepositorio;
            NaturezaJuridicaRepositorio = maturezaJuridicaRepositorio;
            AtividadeEconomicaRepositorio = atividadeEconomicaRepositorio;
            UnidadeFederacaoRepositorio = unidadeFederacaoRepositorio;
            MunicipioRepositorio = municipioRepositorio;
            QualificacaoRepositorio = qualificacaoRepositorio;
            PorteRepositorio = porteRepositorio;
            OpcaoSimplesRepositorio = opcaoSimplesRepositorio;
        }

        public void ProcessaDadoCadastral(string registro)
        {
            var dadoCadastral = ObtemDadoCadastral(registro);
            if (VerificarSeJaExiste)
            {
                if (DadoCadastralRepositorio.JaExite(dadoCadastral) == -1)
                    DadoCadastralRepositorio.Insere(dadoCadastral);
            }
            else
            {
                DadoCadastralRepositorio.Insere(dadoCadastral);
            }
        }

        private DadoCadastral ObtemDadoCadastral(string texto)
        {
            var dadoCadastral = new DadoCadastral();

            int MatrizFilialCd;
            int SituacaoCadastralCd;
            int MotivoSituacaoCadastralCd;
            string PaisCd;
            string PaisNome;
            int NaturezaJuridicaCd;
            string AtividadeEconomicaCd;
            string UnidadeFederacaoCd;
            string MunicipioCd;
            string MunicipioNome;
            int QualificacaoResponsavelCd;
            int PorteCd;
            int OpcaoSimplesCd;

            string textoTemp;

            dadoCadastral.Cnpj = texto.Substring(3, 14).Trim();

            textoTemp = texto.Substring(17, 1).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && int.TryParse(textoTemp, out MatrizFilialCd))
            {
                var matrizFilial = MatrizFilialRepositorio.ObtemPorCodigo(MatrizFilialCd);
                if (matrizFilial == null)
                {
                    matrizFilial = new MatrizFilial
                    {
                        Codigo = MatrizFilialCd,
                        Descricao = MatrizFilialCd.ToString()
                    };
                    dadoCadastral.MatrizFilialId = MatrizFilialRepositorio.Insere(matrizFilial);
                }
                else
                {
                    dadoCadastral.MatrizFilialId = matrizFilial.Id;
                }
            }

            dadoCadastral.RazaoSocialNomeEmpresarial = texto.Substring(18, 150).Trim();

            textoTemp = texto.Substring(168, 55).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.NomeFantasia = textoTemp;

            textoTemp = texto.Substring(223, 2).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && int.TryParse(textoTemp, out SituacaoCadastralCd))
            {
                var situacaoCadastral = SituacaoCadastralRepositorio.ObtemPorCodigo(SituacaoCadastralCd);
                if (situacaoCadastral == null)
                {
                    situacaoCadastral = new SituacaoCadastral
                    {
                        Codigo = SituacaoCadastralCd,
                        Descricao = SituacaoCadastralCd.ToString()
                    };
                    dadoCadastral.SituacaoCadastralId = SituacaoCadastralRepositorio.Insere(situacaoCadastral);
                }
                else
                {
                    dadoCadastral.SituacaoCadastralId = situacaoCadastral.Id;
                }
            }

            textoTemp = texto.Substring(225, 8).Trim();
            dadoCadastral.DataSituacaoCadastral = DataUtil.ConverteData(textoTemp);

            textoTemp = texto.Substring(233, 2).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && int.TryParse(textoTemp, out MotivoSituacaoCadastralCd) && MotivoSituacaoCadastralCd != 0)
            {
                var motivoSituacaoCadastral = MotivoSituacaoCadastralRepositorio.ObtemPorCodigo(MotivoSituacaoCadastralCd);
                if (motivoSituacaoCadastral == null)
                {
                    motivoSituacaoCadastral = new MotivoSituacaoCadastral
                    {
                        Codigo = MotivoSituacaoCadastralCd,
                        Descricao = MotivoSituacaoCadastralCd.ToString()
                    };
                    dadoCadastral.MotivoSituacaoCadastralId = MotivoSituacaoCadastralRepositorio.Insere(motivoSituacaoCadastral);
                }
                else
                {
                    dadoCadastral.MotivoSituacaoCadastralId = motivoSituacaoCadastral.Id;
                }
            }

            textoTemp = texto.Substring(235, 55).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.NomeCidadeExterior = textoTemp;

            UnidadeFederacaoCd = texto.Substring(682, 2).Trim();
            PaisCd = texto.Substring(290, 3).Trim();
            PaisNome = texto.Substring(293, 70).Trim();

            if (string.IsNullOrWhiteSpace(PaisCd) && string.Compare(UnidadeFederacaoCd, "EX") != 0)
            {
                PaisCd = "105";
                PaisNome = "BRASIL";
            }

            if (!string.IsNullOrWhiteSpace(PaisCd))
            {
                var pais = PaisRepositorio.ObtemPorCodigo(PaisCd);
                if (pais == null)
                {
                    pais = new Pais
                    {
                        Codigo = PaisCd,
                        Nome = string.IsNullOrWhiteSpace(PaisNome) ? PaisCd : PaisNome
                    };
                    dadoCadastral.PaisId = PaisRepositorio.Insere(pais);
                }
                else
                {
                    dadoCadastral.PaisId = pais.Id;
                }
            }

            textoTemp = texto.Substring(363, 4).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && int.TryParse(textoTemp, out NaturezaJuridicaCd))
            {
                var naturezaJuridica = NaturezaJuridicaRepositorio.ObtemPorCodigo(NaturezaJuridicaCd);
                if (naturezaJuridica == null)
                {
                    naturezaJuridica = new NaturezaJuridica
                    {
                        Codigo = NaturezaJuridicaCd,
                        Descricao = NaturezaJuridicaCd.ToString()
                    };
                    dadoCadastral.NaturezaJuridicaId = NaturezaJuridicaRepositorio.Insere(naturezaJuridica);
                }
                else
                {
                    dadoCadastral.NaturezaJuridicaId = naturezaJuridica.Id;
                }
            }

            textoTemp = texto.Substring(367, 8).Trim();
            dadoCadastral.DataInicioAtividade = DataUtil.ConverteData(textoTemp);

            textoTemp = texto.Substring(375, 7).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
            {
                AtividadeEconomicaCd = textoTemp;
                var atividadeEconomica = AtividadeEconomicaRepositorio.ObtemPorCodigo(AtividadeEconomicaCd);
                if (atividadeEconomica == null)
                {
                    atividadeEconomica = new AtividadeEconomica
                    {
                        Codigo = AtividadeEconomicaCd,
                        Descricao = AtividadeEconomicaCd
                    };
                    dadoCadastral.AtividadeEconomicaId = AtividadeEconomicaRepositorio.Insere(atividadeEconomica);
                }
                else
                {
                    dadoCadastral.AtividadeEconomicaId = atividadeEconomica.Id;
                }
            }

            textoTemp = texto.Substring(382, 20).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.TipoLogradouro = textoTemp;

            textoTemp = texto.Substring(402, 60).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.Logradouro = textoTemp;

            textoTemp = texto.Substring(462, 6).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.Numero = textoTemp;

            textoTemp = texto.Substring(468, 156).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.Complemento = textoTemp;

            textoTemp = texto.Substring(624, 50).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.Bairro = textoTemp;

            textoTemp = texto.Substring(674, 8).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.Cep = textoTemp;

            textoTemp = texto.Substring(682, 2).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
            {
                UnidadeFederacaoCd = textoTemp;
                var unidadeFederacao = UnidadeFederacaoRepositorio.ObtemPorCodigo(UnidadeFederacaoCd);
                if (unidadeFederacao == null)
                {
                    if (string.Compare(UnidadeFederacaoCd, "EX") == 0)
                    {
                        unidadeFederacao = new UnidadeFederacao
                        {
                            Codigo = UnidadeFederacaoCd,
                            Nome = "EXTERIOR"
                        };
                    }
                    else
                    {
                        unidadeFederacao = new UnidadeFederacao
                        {
                            Codigo = UnidadeFederacaoCd,
                            Nome = UnidadeFederacaoCd.ToString(),
                            PaisId = dadoCadastral.PaisId
                        };
                    }
                    dadoCadastral.UnidadeFederacaoId = UnidadeFederacaoRepositorio.Insere(unidadeFederacao);
                }
                else
                {
                    dadoCadastral.UnidadeFederacaoId = unidadeFederacao.Id;
                }
            }

            textoTemp = texto.Substring(684, 4).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
            {
                MunicipioCd = textoTemp;
                MunicipioNome = texto.Substring(688, 50).Trim();
                var municipio = MunicipioRepositorio.ObtemPorCodigo(MunicipioCd);
                if (municipio == null)
                {
                    municipio = new Municipio
                    {
                        Codigo = MunicipioCd,
                        Nome = string.IsNullOrWhiteSpace(MunicipioNome) ? MunicipioCd : MunicipioNome,
                        UnidadeFederacaoId = dadoCadastral.UnidadeFederacaoId
                    };
                    dadoCadastral.MunicipioId = MunicipioRepositorio.Insere(municipio);
                }
                else
                {
                    dadoCadastral.MunicipioId = municipio.Id;
                }
            }

            textoTemp = texto.Substring(738, 12);

            dadoCadastral.Ddd1 = textoTemp.Substring(0, 4).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.Ddd1))
                dadoCadastral.Ddd1 = null;

            dadoCadastral.Telefone1 = textoTemp.Substring(4, 8).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.Telefone1))
                dadoCadastral.Telefone1 = null;

            textoTemp = texto.Substring(750, 12);

            dadoCadastral.Ddd2 = textoTemp.Substring(0, 4).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.Ddd2))
                dadoCadastral.Ddd2 = null;

            dadoCadastral.Telefone2 = textoTemp.Substring(4, 8).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.Telefone2))
                dadoCadastral.Telefone2 = null;

            textoTemp = texto.Substring(762, 12);

            dadoCadastral.DddFax = textoTemp.Substring(0, 4).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.DddFax))
                dadoCadastral.DddFax = null;

            dadoCadastral.Fax = textoTemp.Substring(4, 8).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.Fax))
                dadoCadastral.Fax = null;

            textoTemp = texto.Substring(774, 115).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp))
                dadoCadastral.CorreioEletronico = textoTemp;

            textoTemp = texto.Substring(889, 2).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && int.TryParse(textoTemp, out QualificacaoResponsavelCd) && QualificacaoResponsavelCd != 0)
            {
                var qualificacao = QualificacaoRepositorio.ObtemPorCodigo(QualificacaoResponsavelCd);
                if (qualificacao == null)
                {
                    qualificacao = new Qualificacao
                    {
                        Codigo = QualificacaoResponsavelCd,
                        Descricao = QualificacaoResponsavelCd.ToString(),
                        ColetadoAtualmente = "S"
                    };
                    dadoCadastral.QualificacaoResponsavelId = QualificacaoRepositorio.Insere(qualificacao);
                }
                else
                {
                    dadoCadastral.QualificacaoResponsavelId = qualificacao.Id;
                }
            }

            textoTemp = texto.Substring(891, 14).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && decimal.TryParse(textoTemp, out decimal tmpDec))
                dadoCadastral.CapitalSocial = tmpDec;

            textoTemp = texto.Substring(905, 2).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && int.TryParse(textoTemp, out PorteCd))
            {
                var porte = PorteRepositorio.ObtemPorCodigo(PorteCd);
                if (porte == null)
                {
                    porte = new Porte
                    {
                        Codigo = PorteCd,
                        Descricao = PorteCd.ToString()
                    };
                    dadoCadastral.PorteId = PorteRepositorio.Insere(porte);
                }
                else
                {
                    dadoCadastral.PorteId = porte.Id;
                }
            }

            textoTemp = texto.Substring(907, 1).Trim();
            if (!string.IsNullOrWhiteSpace(textoTemp) && int.TryParse(textoTemp, out OpcaoSimplesCd))
            {
                var opcaoSimples = OpcaoSimplesRepositorio.ObtemPorCodigo(OpcaoSimplesCd);
                if (opcaoSimples == null)
                {
                    opcaoSimples = new OpcaoSimples
                    {
                        Codigo = OpcaoSimplesCd,
                        Descricao = OpcaoSimplesCd.ToString()
                    };
                    dadoCadastral.OpcaoSimplesId = OpcaoSimplesRepositorio.Insere(opcaoSimples);
                }
                else
                {
                    dadoCadastral.OpcaoSimplesId = opcaoSimples.Id;
                }
            }

            textoTemp = texto.Substring(908, 8).Trim();
            dadoCadastral.DataOpcaoSimples = DataUtil.ConverteData(textoTemp);

            textoTemp = texto.Substring(916, 8).Trim();
            dadoCadastral.DataExclusaoSimples = DataUtil.ConverteData(textoTemp);

            dadoCadastral.OpcaoMei = texto.Substring(924, 1).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.OpcaoMei))
                dadoCadastral.OpcaoMei = null;

            dadoCadastral.SituacaoEspecial = texto.Substring(925, 23).Trim();
            if (string.IsNullOrWhiteSpace(dadoCadastral.SituacaoEspecial))
                dadoCadastral.SituacaoEspecial = null;

            textoTemp = texto.Substring(948, 8).Trim();
            dadoCadastral.DataSituacaoEspecial = DataUtil.ConverteData(textoTemp);

            return dadoCadastral;

        }

    }
}
