using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class PaisRepositorio : RepositorioBase
    {
        public PaisRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""Pais"" (""Codigo"", ""Nome"") VALUES (@Codigo, @Nome) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.VarChar, 3);
            InsCmd.Parameters.Add("@Nome", FbDbType.VarChar, 70);
            InsCmd.Prepare();
        }

        public int Insere(Pais pais)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = pais.Codigo;
            InsCmd.Parameters["@Nome"].Value = pais.Nome;

            pais.Id = (int)InsCmd.ExecuteScalar();

            Paises.Add(pais.Codigo, pais);

            return pais.Id;
        }

        private Dictionary<string, Pais> Paises;

        private void CarregaDados()
        {
            Paises = new Dictionary<string, Pais>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""Pais"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var pais = new Pais
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetString(dr.GetOrdinal("Codigo")),
                        Nome = dr.GetString(dr.GetOrdinal("Nome"))
                    };
                    Paises.Add(pais.Codigo, pais);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Pais ObtemPorCodigo(string Codigo)
        {
            if (Paises.ContainsKey(Codigo))
                return Paises[Codigo];
            return null;
        }

    }
}