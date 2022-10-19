using System;

namespace Importacao.Modelos
{
    public class ControleImportacao : Entity<int>
    {
        public string NomeArquivo { get; set; }
        public DateTime DataGravacao { get; set; }
        public string NumeroRemessa { get; set; }
        public DateTime? DataImportacao { get; set; }
    }
}
