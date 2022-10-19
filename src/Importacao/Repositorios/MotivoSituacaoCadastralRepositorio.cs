using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class MotivoSituacaoCadastralRepositorio : RepositorioBase
    {
        public MotivoSituacaoCadastralRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""MotivoSituacaoCadastral"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.Integer);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 6);
            InsCmd.Prepare();
        }

        public int Insere(MotivoSituacaoCadastral motivoSituacaoCadastral)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = motivoSituacaoCadastral.Codigo;
            InsCmd.Parameters["@Descricao"].Value = motivoSituacaoCadastral.Descricao;

            motivoSituacaoCadastral.Id = (int)InsCmd.ExecuteScalar();

            MotivosSituacoesCadastrais.Add(motivoSituacaoCadastral.Codigo, motivoSituacaoCadastral);

            return motivoSituacaoCadastral.Id;
        }

        private Dictionary<int, MotivoSituacaoCadastral> MotivosSituacoesCadastrais;

        private void CarregaDados()
        {
            MotivosSituacoesCadastrais = new Dictionary<int, MotivoSituacaoCadastral>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""MotivoSituacaoCadastral"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var motivoSituacaoCadastral = new MotivoSituacaoCadastral
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetInt32(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    MotivosSituacoesCadastrais.Add(motivoSituacaoCadastral.Codigo, motivoSituacaoCadastral);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public MotivoSituacaoCadastral ObtemPorCodigo(int Codigo)
        {
            if (MotivosSituacoesCadastrais.ContainsKey(Codigo))
                return MotivosSituacoesCadastrais[Codigo];
            return null;
        }

    }
}