using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class AtividadeEconomicaRepositorio : RepositorioBase
    {
        public AtividadeEconomicaRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""AtividadeEconomica"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.VarChar, 7);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 100);
            InsCmd.Prepare();
        }

        public int Insere(AtividadeEconomica atividadeEconomica)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = atividadeEconomica.Codigo;
            InsCmd.Parameters["@Descricao"].Value = atividadeEconomica.Descricao;

            atividadeEconomica.Id = (int)InsCmd.ExecuteScalar();

            AtividadesEconomicas.Add(atividadeEconomica.Codigo, atividadeEconomica);

            return atividadeEconomica.Id;
        }

        private Dictionary<string, AtividadeEconomica> AtividadesEconomicas;

        private void CarregaDados()
        {
            AtividadesEconomicas = new Dictionary<string, AtividadeEconomica>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""AtividadeEconomica"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var atividadeEconomica = new AtividadeEconomica
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetString(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    AtividadesEconomicas.Add(atividadeEconomica.Codigo, atividadeEconomica);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public AtividadeEconomica ObtemPorCodigo(string Codigo)
        {
            if (AtividadesEconomicas.ContainsKey(Codigo))
                return AtividadesEconomicas[Codigo];
            return null;
        }

    }
}