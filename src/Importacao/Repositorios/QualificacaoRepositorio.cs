using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class QualificacaoRepositorio : RepositorioBase
    {
        public QualificacaoRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""Qualificacao"" (""Codigo"", ""Descricao"", ""ColetadoAtualmente"") VALUES (@Codigo, @Descricao, @ColetadoAtualmente) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.Integer);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 70);
            InsCmd.Parameters.Add("@ColetadoAtualmente", FbDbType.VarChar, 1);
            InsCmd.Prepare();
        }

        public int Insere(Qualificacao qualificacao)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = qualificacao.Codigo;
            InsCmd.Parameters["@Descricao"].Value = qualificacao.Descricao;
            InsCmd.Parameters["@ColetadoAtualmente"].Value = qualificacao.ColetadoAtualmente;

            qualificacao.Id = (int)InsCmd.ExecuteScalar();

            Qualificacoes.Add(qualificacao.Codigo, qualificacao);

            return qualificacao.Id;
        }

        private Dictionary<int, Qualificacao> Qualificacoes;

        private void CarregaDados()
        {
            Qualificacoes = new Dictionary<int, Qualificacao>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""Qualificacao"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var qualificacao = new Qualificacao
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetInt32(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao")),
                        ColetadoAtualmente = dr.GetString(dr.GetOrdinal("ColetadoAtualmente"))
                    };
                    Qualificacoes.Add(qualificacao.Codigo, qualificacao);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Qualificacao ObtemPorCodigo(int Codigo)
        {
            if (Qualificacoes.ContainsKey(Codigo))
                return Qualificacoes[Codigo];
            return null;
        }

    }
}