using System;

namespace Importacao.Modelos
{
    public class DadoCadastral : Entity<int>
    {
        public string Cnpj { get; set; }
        public int MatrizFilialId { get; set; }
        public string RazaoSocialNomeEmpresarial { get; set; }
        public string NomeFantasia { get; set; }
        public int? SituacaoCadastralId { get; set; }
        public DateTime? DataSituacaoCadastral { get; set; }
        public int? MotivoSituacaoCadastralId { get; set; }
        public string NomeCidadeExterior { get; set; }
        public int? PaisId { get; set; }
        public int? NaturezaJuridicaId { get; set; }
        public DateTime? DataInicioAtividade { get; set; }
        public int? AtividadeEconomicaId { get; set; }
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public int? UnidadeFederacaoId { get; set; }
        public int? MunicipioId { get; set; }
        public string Ddd1 { get; set; }
        public string Telefone1 { get; set; }
        public string Ddd2 { get; set; }
        public string Telefone2 { get; set; }
        public string DddFax { get; set; }
        public string Fax { get; set; }
        public string CorreioEletronico { get; set; }
        public int? QualificacaoResponsavelId { get; set; }
        public decimal? CapitalSocial { get; set; }
        public int? PorteId { get; set; }
        public int? OpcaoSimplesId { get; set; }
        public DateTime? DataOpcaoSimples { get; set; }
        public DateTime? DataExclusaoSimples { get; set; }
        public string OpcaoMei { get; set; }
        public string SituacaoEspecial { get; set; }
        public DateTime? DataSituacaoEspecial { get; set; }
    }
}
