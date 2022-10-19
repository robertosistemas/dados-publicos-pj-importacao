using FirebirdSql.Data.FirebirdClient;
using Importacao.Modelos;

namespace Importacao.Repositorios
{
    public class DadoCadastralRepositorio : RepositorioBase
    {
        public DadoCadastralRepositorio(FbConnection connection) : base(connection)
        {
        }

        private const string queryInsert = @"
        INSERT INTO ""DadoCadastral"" (
            ""Cnpj"",
            ""MatrizFilialId"",
            ""RazaoSocialNomeEmpresarial"",
            ""NomeFantasia"",
            ""SituacaoCadastralId"",
            ""DataSituacaoCadastral"",
            ""MotivoSituacaoCadastralId"",
            ""NomeCidadeExterior"",
            ""PaisId"",
            ""NaturezaJuridicaId"",
            ""DataInicioAtividade"",
            ""AtividadeEconomicaId"",
            ""TipoLogradouro"",
            ""Logradouro"",
            ""Numero"",
            ""Complemento"",
            ""Bairro"",
            ""Cep"",
            ""UnidadeFederacaoId"",
            ""MunicipioId"",
            ""Ddd1"",
            ""Telefone1"",
            ""Ddd2"",
            ""Telefone2"",
            ""DddFax"",
            ""Fax"",
            ""CorreioEletronico"",
            ""QualificacaoResponsavelId"",
            ""CapitalSocial"",
            ""PorteId"",
            ""OpcaoSimplesId"",
            ""DataOpcaoSimples"",
            ""DataExclusaoSimples"",
            ""OpcaoMei"",
            ""SituacaoEspecial"",
            ""DataSituacaoEspecial""
        ) VALUES (
            @Cnpj,
            @MatrizFilialId,
            @RazaoSocialNomeEmpresarial,
            @NomeFantasia,
            @SituacaoCadastralId,
            @DataSituacaoCadastral,
            @MotivoSituacaoCadastralId,
            @NomeCidadeExterior,
            @PaisId,
            @NaturezaJuridicaId,
            @DataInicioAtividade,
            @AtividadeEconomicaId,
            @TipoLogradouro,
            @Logradouro,
            @Numero,
            @Complemento,
            @Bairro,
            @Cep,
            @UnidadeFederacaoId,
            @MunicipioId,
            @Ddd1,
            @Telefone1,
            @Ddd2,
            @Telefone2,
            @DddFax,
            @Fax,
            @CorreioEletronico,
            @QualificacaoResponsavelId,
            @CapitalSocial,
            @PorteId,
            @OpcaoSimplesId,
            @DataOpcaoSimples,
            @DataExclusaoSimples,
            @OpcaoMei,
            @SituacaoEspecial,
            @DataSituacaoEspecial
        ) returning ""Id"" ";

        protected override void CriaInsCmd()
        {
            InsCmd = Connection.CreateCommand();
            InsCmd.CommandText = queryInsert;
            InsCmd.Parameters.Add("@Cnpj", FbDbType.VarChar, 14);
            InsCmd.Parameters.Add("@MatrizFilialId", FbDbType.Integer);
            InsCmd.Parameters.Add("@RazaoSocialNomeEmpresarial", FbDbType.VarChar, 150);
            InsCmd.Parameters.Add("@NomeFantasia", FbDbType.VarChar, 55);
            InsCmd.Parameters.Add("@SituacaoCadastralId", FbDbType.Integer);
            InsCmd.Parameters.Add("@DataSituacaoCadastral", FbDbType.Date);
            InsCmd.Parameters.Add("@MotivoSituacaoCadastralId", FbDbType.Integer);
            InsCmd.Parameters.Add("@NomeCidadeExterior", FbDbType.VarChar, 55);
            InsCmd.Parameters.Add("@PaisId", FbDbType.Integer);
            InsCmd.Parameters.Add("@NaturezaJuridicaId", FbDbType.Integer);
            InsCmd.Parameters.Add("@DataInicioAtividade", FbDbType.Date);
            InsCmd.Parameters.Add("@AtividadeEconomicaId", FbDbType.Integer);
            InsCmd.Parameters.Add("@TipoLogradouro", FbDbType.VarChar, 20);
            InsCmd.Parameters.Add("@Logradouro", FbDbType.VarChar, 60);
            InsCmd.Parameters.Add("@Numero", FbDbType.VarChar, 6);
            InsCmd.Parameters.Add("@Complemento", FbDbType.VarChar, 156);
            InsCmd.Parameters.Add("@Bairro", FbDbType.VarChar, 50);
            InsCmd.Parameters.Add("@Cep", FbDbType.VarChar, 8);
            InsCmd.Parameters.Add("@UnidadeFederacaoId", FbDbType.Integer);
            InsCmd.Parameters.Add("@MunicipioId", FbDbType.Integer);
            InsCmd.Parameters.Add("@Ddd1", FbDbType.VarChar, 4);
            InsCmd.Parameters.Add("@Telefone1", FbDbType.VarChar, 9);
            InsCmd.Parameters.Add("@Ddd2", FbDbType.VarChar, 4);
            InsCmd.Parameters.Add("@Telefone2", FbDbType.VarChar, 9);
            InsCmd.Parameters.Add("@DddFax", FbDbType.VarChar, 4);
            InsCmd.Parameters.Add("@Fax", FbDbType.VarChar, 9);
            InsCmd.Parameters.Add("@CorreioEletronico", FbDbType.VarChar, 115);
            InsCmd.Parameters.Add("@QualificacaoResponsavelId", FbDbType.Integer);
            InsCmd.Parameters.Add("@CapitalSocial", FbDbType.Decimal);
            InsCmd.Parameters.Add("@PorteId", FbDbType.Integer);
            InsCmd.Parameters.Add("@OpcaoSimplesId", FbDbType.Integer);
            InsCmd.Parameters.Add("@DataOpcaoSimples", FbDbType.Date);
            InsCmd.Parameters.Add("@DataExclusaoSimples", FbDbType.Date);
            InsCmd.Parameters.Add("@OpcaoMei", FbDbType.VarChar, 1);
            InsCmd.Parameters.Add("@SituacaoEspecial", FbDbType.VarChar, 23);
            InsCmd.Parameters.Add("@DataSituacaoEspecial", FbDbType.Date);
            InsCmd.Prepare();
        }

        public int JaExite(DadoCadastral dadoCadastral)
        {
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"SELECT ""Id"" FROM  ""DadoCadastral"" WHERE ""Cnpj"" = @Cnpj";
            cmd.Transaction = this.Transaction; ;
            cmd.Parameters.Add("@Cnpj", FbDbType.VarChar, 14);
            cmd.Parameters["@Cnpj"].Value = dadoCadastral.Cnpj;
            var id = cmd.ExecuteScalar();
            if (id == null)
                return -1;
            else
                return (int)id;
        }

        public int Insere(DadoCadastral dadoCadastral)
        {
            if (InsCmd.Transaction == null)
                InsCmd.Transaction = this.Transaction;

            InsCmd.Parameters["@Cnpj"].Value = dadoCadastral.Cnpj;
            InsCmd.Parameters["@MatrizFilialId"].Value = dadoCadastral.MatrizFilialId;
            InsCmd.Parameters["@RazaoSocialNomeEmpresarial"].Value = dadoCadastral.RazaoSocialNomeEmpresarial;
            InsCmd.Parameters["@NomeFantasia"].Value = ValorOuNulo(dadoCadastral.NomeFantasia);
            InsCmd.Parameters["@SituacaoCadastralId"].Value = ValorOuNulo(dadoCadastral.SituacaoCadastralId);
            InsCmd.Parameters["@DataSituacaoCadastral"].Value = ValorOuNulo(dadoCadastral.DataSituacaoCadastral);
            InsCmd.Parameters["@MotivoSituacaoCadastralId"].Value = ValorOuNulo(dadoCadastral.MotivoSituacaoCadastralId);
            InsCmd.Parameters["@NomeCidadeExterior"].Value = ValorOuNulo(dadoCadastral.NomeCidadeExterior);
            InsCmd.Parameters["@PaisId"].Value = ValorOuNulo(dadoCadastral.PaisId);
            InsCmd.Parameters["@NaturezaJuridicaId"].Value = ValorOuNulo(dadoCadastral.NaturezaJuridicaId);
            InsCmd.Parameters["@DataInicioAtividade"].Value = ValorOuNulo(dadoCadastral.DataInicioAtividade);
            InsCmd.Parameters["@AtividadeEconomicaId"].Value = ValorOuNulo(dadoCadastral.AtividadeEconomicaId);
            InsCmd.Parameters["@TipoLogradouro"].Value = ValorOuNulo(dadoCadastral.TipoLogradouro);
            InsCmd.Parameters["@Logradouro"].Value = ValorOuNulo(dadoCadastral.Logradouro);
            InsCmd.Parameters["@Numero"].Value = ValorOuNulo(dadoCadastral.Numero);
            InsCmd.Parameters["@Complemento"].Value = ValorOuNulo(dadoCadastral.Complemento);
            InsCmd.Parameters["@Bairro"].Value = ValorOuNulo(dadoCadastral.Bairro);
            InsCmd.Parameters["@Cep"].Value = ValorOuNulo(dadoCadastral.Cep);
            InsCmd.Parameters["@UnidadeFederacaoId"].Value = ValorOuNulo(dadoCadastral.UnidadeFederacaoId);
            InsCmd.Parameters["@MunicipioId"].Value = ValorOuNulo(dadoCadastral.MunicipioId);
            InsCmd.Parameters["@Ddd1"].Value = ValorOuNulo(dadoCadastral.Ddd1);
            InsCmd.Parameters["@Telefone1"].Value = ValorOuNulo(dadoCadastral.Telefone1);
            InsCmd.Parameters["@Ddd2"].Value = ValorOuNulo(dadoCadastral.Ddd2);
            InsCmd.Parameters["@Telefone2"].Value = ValorOuNulo(dadoCadastral.Telefone2);
            InsCmd.Parameters["@DddFax"].Value = ValorOuNulo(dadoCadastral.DddFax);
            InsCmd.Parameters["@Fax"].Value = ValorOuNulo(dadoCadastral.Fax);
            InsCmd.Parameters["@CorreioEletronico"].Value = ValorOuNulo(dadoCadastral.CorreioEletronico);
            InsCmd.Parameters["@QualificacaoResponsavelId"].Value = ValorOuNulo(dadoCadastral.QualificacaoResponsavelId);
            InsCmd.Parameters["@CapitalSocial"].Value = ValorOuNulo(dadoCadastral.CapitalSocial);
            InsCmd.Parameters["@PorteId"].Value = ValorOuNulo(dadoCadastral.PorteId);
            InsCmd.Parameters["@OpcaoSimplesId"].Value = ValorOuNulo(dadoCadastral.OpcaoSimplesId);
            InsCmd.Parameters["@DataOpcaoSimples"].Value = ValorOuNulo(dadoCadastral.DataOpcaoSimples);
            InsCmd.Parameters["@DataExclusaoSimples"].Value = ValorOuNulo(dadoCadastral.DataExclusaoSimples);
            InsCmd.Parameters["@OpcaoMei"].Value = ValorOuNulo(dadoCadastral.OpcaoMei);
            InsCmd.Parameters["@SituacaoEspecial"].Value = ValorOuNulo(dadoCadastral.SituacaoEspecial);
            InsCmd.Parameters["@DataSituacaoEspecial"].Value = ValorOuNulo(dadoCadastral.DataSituacaoEspecial);

            dadoCadastral.Id = (int)InsCmd.ExecuteScalar();
            return dadoCadastral.Id;
        }
    }
}
