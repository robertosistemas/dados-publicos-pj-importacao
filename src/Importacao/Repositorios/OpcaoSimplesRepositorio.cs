using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class OpcaoSimplesRepositorio : RepositorioBase
    {
        public OpcaoSimplesRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""OpcaoSimples"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.Integer);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 30);
            InsCmd.Prepare();
        }

        public int Insere(OpcaoSimples opcaoSimples)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = opcaoSimples.Codigo;
            InsCmd.Parameters["@Descricao"].Value = opcaoSimples.Descricao;

            opcaoSimples.Id = (int)InsCmd.ExecuteScalar();

            OpcoesSimples.Add(opcaoSimples.Codigo, opcaoSimples);

            return opcaoSimples.Id;
        }

        private Dictionary<int, OpcaoSimples> OpcoesSimples;

        private void CarregaDados()
        {
            OpcoesSimples = new Dictionary<int, OpcaoSimples>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""OpcaoSimples"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var opcaoSimples = new OpcaoSimples
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetInt32(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    OpcoesSimples.Add(opcaoSimples.Codigo, opcaoSimples);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public OpcaoSimples ObtemPorCodigo(int Codigo)
        {
            if (OpcoesSimples.ContainsKey(Codigo))
                return OpcoesSimples[Codigo];
            return null;
        }

    }
}