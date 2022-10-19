using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class SituacaoCadastralRepositorio : RepositorioBase
    {
        public SituacaoCadastralRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""SituacaoCadastral"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.Integer);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 6);
            InsCmd.Prepare();
        }

        public int Insere(SituacaoCadastral situacaoCadastral)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = situacaoCadastral.Codigo;
            InsCmd.Parameters["@Descricao"].Value = situacaoCadastral.Descricao;

            situacaoCadastral.Id = (int)InsCmd.ExecuteScalar();

            SituacoesCadastrais.Add(situacaoCadastral.Codigo, situacaoCadastral);

            return situacaoCadastral.Id;
        }

        private Dictionary<int, SituacaoCadastral> SituacoesCadastrais;

        private void CarregaDados()
        {
            SituacoesCadastrais = new Dictionary<int, SituacaoCadastral>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""SituacaoCadastral"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var situacaoCadastral = new SituacaoCadastral
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetInt32(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    SituacoesCadastrais.Add(situacaoCadastral.Codigo, situacaoCadastral);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public SituacaoCadastral ObtemPorCodigo(int Codigo)
        {
            if (SituacoesCadastrais.ContainsKey(Codigo))
                return SituacoesCadastrais[Codigo];
            return null;
        }

    }
}