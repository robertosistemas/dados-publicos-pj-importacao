using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;
using System.Collections.Generic;

namespace Importacao.Repositorios
{
    public class MunicipioRepositorio : RepositorioBase
    {
        public MunicipioRepositorio(FbConnection connection) : base(connection)
        {
            CarregaDados();
        }

        private const string queryInsert = @"INSERT INTO ""Municipio"" (""Codigo"", ""Nome"", ""UnidadeFederacaoId"") VALUES (@Codigo, @Nome, @UnidadeFederacaoId) returning ""Id""";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Codigo", FbDbType.VarChar, 4);
            InsCmd.Parameters.Add("@Nome", FbDbType.VarChar, 50);
            InsCmd.Parameters.Add("@UnidadeFederacaoId", FbDbType.Integer);
            InsCmd.Prepare();
        }

        public int Insere(Municipio municipio)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Codigo"].Value = municipio.Codigo;
            InsCmd.Parameters["@Nome"].Value = municipio.Nome;
            InsCmd.Parameters["@UnidadeFederacaoId"].Value = municipio.UnidadeFederacaoId;

            municipio.Id = (int)InsCmd.ExecuteScalar();

            Municipios.Add(municipio.Codigo, municipio);

            return municipio.Id;
        }

        private Dictionary<string, Municipio> Municipios;

        private void CarregaDados()
        {
            Municipios = new Dictionary<string, Municipio>();
            using var transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM  ""Municipio"" ORDER BY ""Codigo"" ";
                cmd.Transaction = transaction;
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var municipio = new Municipio
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Codigo = dr.GetString(dr.GetOrdinal("Codigo")),
                        Nome = dr.GetString(dr.GetOrdinal("Nome")),
                        UnidadeFederacaoId = dr.GetInt32(dr.GetOrdinal("UnidadeFederacaoId"))
                    };
                    Municipios.Add(municipio.Codigo, municipio);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Municipio ObtemPorCodigo(string Codigo)
        {
            if (Municipios.ContainsKey(Codigo))
                return Municipios[Codigo];
            return null;
        }

    }
}