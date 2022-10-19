namespace Importacao.Auxiliares
{
    public class ConfiguracaoBaseDados
    {
        public TipoConexao TipoConexao { get; set; }
        public string TemplateConexaoRemota { get; set; }
        public string TemplateConexaoEmbarcada { get; set; }
        public bool VerificarSeJaExiste { get; set; }
    }
}
