using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class UnidadeFederacaoRepositorio : RepositorioBase
    {
        public UnidadeFederacaoRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""UnidadeFederacao"" (""Codigo"", ""Nome"", ""PaisId"") VALUES (@Codigo, @Nome, @PaisId) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.VarChar, 2);
            InsCmd.Parameters.Add("@Nome", FbDbType.VarChar, 50);
            InsCmd.Parameters.Add("@PaisId", FbDbType.Integer);
            InsCmd.Prepare();
        }

        public int Insere(UnidadeFederacao unidadeFederacao)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = unidadeFederacao.Codigo;
            InsCmd.Parameters["@Nome"].Value = unidadeFederacao.Nome;
            InsCmd.Parameters["@PaisId"].Value = unidadeFederacao.PaisId;

            unidadeFederacao.Id = (int)InsCmd.ExecuteScalar();

            UnidadesFederacoes.Add(unidadeFederacao.Codigo, unidadeFederacao);

            return unidadeFederacao.Id;
        }

        private Dictionary<string, UnidadeFederacao> UnidadesFederacoes;

        private void CarregaDados()
        {
            UnidadesFederacoes = new Dictionary<string, UnidadeFederacao>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""UnidadeFederacao"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var unidadeFederacao = new UnidadeFederacao
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetString(dr.GetOrdinal("Codigo")),
                        Nome = dr.GetString(dr.GetOrdinal("Nome"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("PaisId")))
                    {
                        unidadeFederacao.PaisId = dr.GetInt32(dr.GetOrdinal("PaisId"));
                    }
                    UnidadesFederacoes.Add(unidadeFederacao.Codigo, unidadeFederacao);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public UnidadeFederacao ObtemPorCodigo(string Codigo)
        {
            if (UnidadesFederacoes.ContainsKey(Codigo))
                return UnidadesFederacoes[Codigo];
            return null;
        }

    }
}