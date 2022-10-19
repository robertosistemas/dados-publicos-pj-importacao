using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;

namespace Importacao.Repositorios
{
    public class AtividadeEconomicaSecundariaRepositorio : RepositorioBase
    {
        public AtividadeEconomicaSecundariaRepositorio(FbConnection connection) : base(connection)
        {
        }

        private const string queryInsert = @"INSERT INTO ""AtividadeEconomicaSecundaria"" (""Cnpj"", ""AtividadeEconomicaId"") VALUES (@Cnpj, @AtividadeEconomicaId) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Cnpj", FbDbType.VarChar, 14);
            InsCmd.Parameters.Add("@AtividadeEconomicaId", FbDbType.Integer);
            InsCmd.Prepare();
        }

        public int Insere(AtividadeEconomicaSecundaria atividadeEconomicaSecundaria)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Cnpj"].Value = atividadeEconomicaSecundaria.Cnpj;
            InsCmd.Parameters["@AtividadeEconomicaId"].Value = atividadeEconomicaSecundaria.AtividadeEconomicaId;

            atividadeEconomicaSecundaria.Id = (int)InsCmd.ExecuteScalar();
            return atividadeEconomicaSecundaria.Id;
        }

        public int JaExite(AtividadeEconomicaSecundaria atividadeEconomicaSecundaria)
        {
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"SELECT ""Id"" FROM  ""AtividadeEconomicaSecundaria"" WHERE ""Cnpj"" = @Cnpj AND ""AtividadeEconomicaId"" = @AtividadeEconomicaId";
            cmd.Transaction = this.Transaction; ;

            cmd.Parameters.Add("@Cnpj", FbDbType.VarChar, 14);
            cmd.Parameters.Add("@AtividadeEconomicaId", FbDbType.Integer);

            cmd.Parameters["@Cnpj"].Value = atividadeEconomicaSecundaria.Cnpj;
            cmd.Parameters["@AtividadeEconomicaId"].Value = atividadeEconomicaSecundaria.AtividadeEconomicaId;

            var id = cmd.ExecuteScalar();
            if (id == null)
                return -1;
            else
                return (int)id;
        }
    }
}