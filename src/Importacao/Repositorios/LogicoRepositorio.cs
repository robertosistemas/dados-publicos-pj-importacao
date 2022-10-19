using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class LogicoRepositorio : RepositorioBase
    {
        public LogicoRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsertNotExists = @"
            INSERT INTO ""Logico"" (""Codigo"", ""Descricao"")
            SELECT @Codigo, @Descricao
            FROM RDB$DATABASE
            WHERE NOT EXISTS(
            SELECT ""Codigo""
            FROM ""Logico"" r
            WHERE r.""Codigo"" = @CodigoKey)
            RETURNING ""Id"" ";

        private const string queryInsert = @"INSERT INTO ""Logico"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.VarChar, 1);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 5);
            InsCmd.Prepare();
        }

        public int Insere(Logico logico)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = logico.Codigo;
            InsCmd.Parameters["@Descricao"].Value = logico.Descricao;

            logico.Id = (int)InsCmd.ExecuteScalar();

            Logicos.Add(logico.Codigo, logico);

            return logico.Id;
        }

        private Dictionary<string, Logico> Logicos;

        private void CarregaDados()
        {
            Logicos = new Dictionary<string, Logico>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""Logico"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var logico = new Logico
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetString(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    Logicos.Add(logico.Codigo, logico);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Logico ObtemPorCodigo(string Codigo)
        {
            if (Logicos.ContainsKey(Codigo))
                return Logicos[Codigo];
            return null;
        }

    }
}