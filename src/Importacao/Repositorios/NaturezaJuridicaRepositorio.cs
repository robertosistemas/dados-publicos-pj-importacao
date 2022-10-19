using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class NaturezaJuridicaRepositorio : RepositorioBase
    {
        public NaturezaJuridicaRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""NaturezaJuridica"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.Integer);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 50);
            InsCmd.Prepare();
        }

        public int Insere(NaturezaJuridica naturezaJuridica)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = naturezaJuridica.Codigo;
            InsCmd.Parameters["@Descricao"].Value = naturezaJuridica.Descricao;

            naturezaJuridica.Id = (int)InsCmd.ExecuteScalar();

            NaturezasJuridicas.Add(naturezaJuridica.Codigo, naturezaJuridica);

            return naturezaJuridica.Id;
        }

        private Dictionary<int, NaturezaJuridica> NaturezasJuridicas;

        private void CarregaDados()
        {
            NaturezasJuridicas = new Dictionary<int, NaturezaJuridica>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""NaturezaJuridica"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var naturezaJuridica = new NaturezaJuridica
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetInt32(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    NaturezasJuridicas.Add(naturezaJuridica.Codigo, naturezaJuridica);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public NaturezaJuridica ObtemPorCodigo(int Codigo)
        {
            if (NaturezasJuridicas.ContainsKey(Codigo))
                return NaturezasJuridicas[Codigo];
            return null;
        }

    }
}