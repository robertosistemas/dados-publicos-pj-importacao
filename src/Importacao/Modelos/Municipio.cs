namespace Importacao.Modelos
{
    public class Municipio : Entity<int>
    {
        public string Codigo { get; set; }
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public string Uf { get; set; }
        public int? UnidadeFederacaoId { get; set; }
    }
}
