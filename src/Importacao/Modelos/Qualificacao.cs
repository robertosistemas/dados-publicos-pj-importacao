namespace Importacao.Modelos
{
    public class Qualificacao : Entity<int>
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string ColetadoAtualmente { get; set; }
    }
}
