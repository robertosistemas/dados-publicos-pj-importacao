using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class ControleImportacaoRepositorio : RepositorioBase
    {
        public ControleImportacaoRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""ControleImportacao"" (""NomeArquivo"", ""DataGravacao"", ""NumeroRemessa"", ""DataImportacao"") VALUES (@NomeArquivo, @DataGravacao, @NumeroRemessa, @DataImportacao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@NomeArquivo", FbDbType.VarChar, 11);
            InsCmd.Parameters.Add("@DataGravacao", FbDbType.Date);
            InsCmd.Parameters.Add("@NumeroRemessa", FbDbType.VarChar, 8);
            InsCmd.Parameters.Add("@DataImportacao", FbDbType.Date);
            InsCmd.Prepare();
        }

        public int Insere(ControleImportacao controleImportacao)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@NomeArquivo"].Value = controleImportacao.NomeArquivo;
            InsCmd.Parameters["@DataGravacao"].Value = controleImportacao.DataGravacao;
            InsCmd.Parameters["@NumeroRemessa"].Value = controleImportacao.NumeroRemessa;
            InsCmd.Parameters["@DataImportacao"].Value = ValorOuNulo(controleImportacao.DataImportacao);

            controleImportacao.Id = (int)InsCmd.ExecuteScalar();

            ControleImportacoes.Add(controleImportacao.NomeArquivo, controleImportacao);

            return controleImportacao.Id;
        }

        private const string queryUpdate = @"UPDATE ""ControleImportacao"" SET ""NomeArquivo"" = @NomeArquivo, ""DataGravacao"" = @DataGravacao, ""NumeroRemessa"" = @NumeroRemessa, ""DataImportacao"" = @DataImportacao WHERE ""Id"" = @Id";

        public void Atualiza(ControleImportacao controleImportacao)
        {
            using var UpdCmd = Connection.CreateCommand();

            UpdCmd.CommandText = queryUpdate;
            UpdCmd.Parameters.Add("@NomeArquivo", FbDbType.VarChar, 11);
            UpdCmd.Parameters.Add("@DataGravacao", FbDbType.Date);
            UpdCmd.Parameters.Add("@NumeroRemessa", FbDbType.VarChar, 8);
            UpdCmd.Parameters.Add("@DataImportacao", FbDbType.Date);
            UpdCmd.Parameters.Add("@Id", FbDbType.Integer);
            UpdCmd.Prepare();

            UpdCmd.Transaction = this.Transaction;

            UpdCmd.Parameters["@NomeArquivo"].Value = controleImportacao.NomeArquivo;
            UpdCmd.Parameters["@DataGravacao"].Value = controleImportacao.DataGravacao;
            UpdCmd.Parameters["@NumeroRemessa"].Value = controleImportacao.NumeroRemessa;
            UpdCmd.Parameters["@DataImportacao"].Value = ValorOuNulo(controleImportacao.DataImportacao);
            UpdCmd.Parameters["@Id"].Value = controleImportacao.Id;

            UpdCmd.ExecuteNonQuery();
        }

        private Dictionary<string, ControleImportacao> ControleImportacoes;

        private void CarregaDados()
        {
            ControleImportacoes = new Dictionary<string, ControleImportacao>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""ControleImportacao"" ORDER BY ""NomeArquivo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var controleImportacao = new ControleImportacao
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        NomeArquivo = dr.GetString(dr.GetOrdinal("NomeArquivo")),
                        DataGravacao = dr.GetDateTime(dr.GetOrdinal("DataGravacao")),
                        NumeroRemessa = dr.GetString(dr.GetOrdinal("NumeroRemessa"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("DataImportacao")))
                    {
                        controleImportacao.DataImportacao = dr.GetDateTime(dr.GetOrdinal("DataImportacao"));
                    }
                    ControleImportacoes.Add(controleImportacao.NomeArquivo, controleImportacao);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public ControleImportacao ObtemPorNomeArquivo(string NomeArquivo)
        {
            if (ControleImportacoes.ContainsKey(NomeArquivo))
                return ControleImportacoes[NomeArquivo];
            return null;
        }

    }
}