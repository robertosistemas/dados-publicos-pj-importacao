using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class MatrizFilialRepositorio : RepositorioBase
    {
        public MatrizFilialRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""MatrizFilial"" (""Codigo"", ""Descricao"") VALUES (@Codigo, @Descricao) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.Integer);
            InsCmd.Parameters.Add("@Descricao", FbDbType.VarChar, 6);
            InsCmd.Prepare();
        }

        public int Insere(MatrizFilial matrizFilial)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = matrizFilial.Codigo;
            InsCmd.Parameters["@Descricao"].Value = matrizFilial.Descricao;

            matrizFilial.Id = (int)InsCmd.ExecuteScalar();

            MatrizesFiliais.Add(matrizFilial.Codigo, matrizFilial);

            return matrizFilial.Id;
        }

        private Dictionary<int, MatrizFilial> MatrizesFiliais;

        private void CarregaDados()
        {
            MatrizesFiliais = new Dictionary<int, MatrizFilial>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""MatrizFilial"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var matrizFilial = new MatrizFilial
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetInt32(dr.GetOrdinal("Codigo")),
                        Descricao = dr.GetString(dr.GetOrdinal("Descricao"))
                    };
                    MatrizesFiliais.Add(matrizFilial.Codigo, matrizFilial);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public MatrizFilial ObtemPorCodigo(int Codigo)
        {
            if (MatrizesFiliais.ContainsKey(Codigo))
                return MatrizesFiliais[Codigo];
            return null;
        }

    }
}