namespace Importacao.Modelos
{
    public class UnidadeFederacao : Entity<int>
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int? PaisId { get; set; }
    }
}
