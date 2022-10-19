using System;

namespace Importacao.Auxiliares
{
    public static class DataUtil
    {
        public static DateTime? ConverteData(string data)
        {
            if (!string.IsNullOrWhiteSpace(data) && data.Length == 8)
            {
                var okAno = int.TryParse(data.Substring(0, 4).Trim(), out int ano);
                var okMes = int.TryParse(data.Substring(4, 2).Trim(), out int mes);
                var okDia = int.TryParse(data.Substring(6, 2).Trim(), out int dia);
                if (!(okAno && okDia && okMes))
                    return null;
                if ((ano >= 1 && ano <= 9999) && (mes >= 1 && mes <= 12) && (dia >= 1 && dia <= 31))
                {
                    return new DateTime(ano, mes, dia);
                }
            }
            return null;
        }
    }
}
