using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class PorteRepositorio : RepositorioBase
    {
        public PorteRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""Porte"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.Integer);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 30);
            InsCmd.Prepare();
        }

        public int Insere(Porte porte)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = porte.Codigo;
            InsCmd.Parameters["@Descricao"].Value = porte.Descricao;

            porte.Id = (int)InsCmd.ExecuteScalar();

            Portes.Add(porte.Codigo, porte);

            return porte.Id;
        }

        private Dictionary<int, Porte> Portes;

        private void CarregaDados()
        {
            Portes = new Dictionary<int, Porte>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""Porte"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var porte = new Porte
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetInt32(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    Portes.Add(porte.Codigo, porte);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Porte ObtemPorCodigo(int Codigo)
        {
            if (Portes.ContainsKey(Codigo))
                return Portes[Codigo];
            return null;
        }

    }
}