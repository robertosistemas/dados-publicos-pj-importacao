SET SQL DIALECT 3;

SET NAMES WIN1252;

CONNECT 'C:\Receita\RECEITA.FDB' USER 'SYSDBA' PASSWORD 'masterkey';

/******************************************************************************/
/*                             Unique constraints                             */
/******************************************************************************/

-- ALTER TABLE "AtividadeEconomica" ADD CONSTRAINT "UK_AtividadeEconomica" UNIQUE ("Codigo");
ALTER TABLE "AtividadeEconomicaSecundaria" ADD CONSTRAINT "UK_AtividadeEconomicaSecundaria" UNIQUE ("Cnpj", "AtividadeEconomicaId");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "UK_DadoCadastral" UNIQUE ("Cnpj");
CREATE INDEX "IX_DadoCadastral_RazaSociNome" ON "DadoCadastral" ("RazaoSocialNomeEmpresarial");
-- ALTER TABLE "Logico" ADD CONSTRAINT "UK_Logico" UNIQUE ("Codigo");
-- ALTER TABLE "MatrizFilial" ADD CONSTRAINT "UK_MatrizFilial" UNIQUE ("Codigo");
-- ALTER TABLE "MotivoSituacaoCadastral" ADD CONSTRAINT "UK_MotivoSituacaoCadastral" UNIQUE ("Codigo");
-- ALTER TABLE "Municipio" ADD CONSTRAINT "UK_Municipio" UNIQUE ("Codigo");
-- ALTER TABLE "NaturezaJuridica" ADD CONSTRAINT "UK_NaturezaJuridica" UNIQUE ("Codigo");
-- ALTER TABLE "OpcaoSimples" ADD CONSTRAINT "UK_OpcaoSimples" UNIQUE ("Codigo");
-- ALTER TABLE "Pais" ADD CONSTRAINT "UK_Pais" UNIQUE ("Codigo");
-- ALTER TABLE "Porte" ADD CONSTRAINT "UK_Porte" UNIQUE ("Codigo");
-- ALTER TABLE "Qualificacao" ADD CONSTRAINT "UK_Qualificacao" UNIQUE ("Codigo");
-- ALTER TABLE "SituacaoCadastral" ADD CONSTRAINT "UK_SituacaoCadastral" UNIQUE ("Codigo");
-- ALTER TABLE "TipoSocio" ADD CONSTRAINT "UK_TipoSocio" UNIQUE ("Codigo");
-- ALTER TABLE "UnidadeFederacao" ADD CONSTRAINT "UK_UnidadeFederacao" UNIQUE ("Codigo");


/******************************************************************************/
/*                                Primary keys                                */
/******************************************************************************/

--ALTER TABLE "Logico" ADD CONSTRAINT "PK_Logico" PRIMARY KEY ("Id");
--ALTER TABLE "MatrizFilial" ADD CONSTRAINT "PK_MatrizFilial" PRIMARY KEY ("Id");
--ALTER TABLE "MotivoSituacaoCadastral" ADD CONSTRAINT "PK_MotivoSituacaoCadastral" PRIMARY KEY ("Id");
--ALTER TABLE "Municipio" ADD CONSTRAINT "PK_Municipio" PRIMARY KEY ("Id");
--ALTER TABLE "NaturezaJuridica" ADD CONSTRAINT "PK_NaturezaJuridica" PRIMARY KEY ("Id");
--ALTER TABLE "OpcaoSimples" ADD CONSTRAINT "PK_OpcaoSimples" PRIMARY KEY ("Id");
--ALTER TABLE "Pais" ADD CONSTRAINT "PK_Pais" PRIMARY KEY ("Id");
--ALTER TABLE "Porte" ADD CONSTRAINT "PK_Porte" PRIMARY KEY ("Id");
--ALTER TABLE "Qualificacao" ADD CONSTRAINT "PK_Qualificacao" PRIMARY KEY ("Id");
--ALTER TABLE "SituacaoCadastral" ADD CONSTRAINT "PK_SituacaoCadastral" PRIMARY KEY ("Id");
--ALTER TABLE "Socio" ADD CONSTRAINT "PK_Socio" PRIMARY KEY ("Id");
--ALTER TABLE "TipoSocio" ADD CONSTRAINT "PK_TipoSocio" PRIMARY KEY ("Id");
--ALTER TABLE "UnidadeFederacao" ADD CONSTRAINT "PK_UnidadeFederacao" PRIMARY KEY ("Id");

--ALTER TABLE "AtividadeEconomica" ADD CONSTRAINT "PK_AtividadeEconomica" PRIMARY KEY ("Id");
ALTER TABLE "AtividadeEconomicaSecundaria" ADD CONSTRAINT "PK_AtividadeEconomicaSecundaria" PRIMARY KEY ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "PK_DadoCadastral" PRIMARY KEY ("Id");

/******************************************************************************/
/*                                Foreign keys                                */
/******************************************************************************/

ALTER TABLE "AtividadeEconomicaSecundaria" ADD CONSTRAINT "FK_AtivEconSecu_AtivEcon" FOREIGN KEY ("AtividadeEconomicaId") REFERENCES "AtividadeEconomica" ("Id");
ALTER TABLE "AtividadeEconomicaSecundaria" ADD CONSTRAINT "FK_AtivEconSecu_DadoCadastral" FOREIGN KEY ("Cnpj") REFERENCES "DadoCadastral" ("Cnpj");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_AtivEcon" FOREIGN KEY ("AtividadeEconomicaId") REFERENCES "AtividadeEconomica" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_Logico" FOREIGN KEY ("OpcaoMei") REFERENCES "Logico" ("Codigo");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_MatrizFilial" FOREIGN KEY ("MatrizFilialId") REFERENCES "MatrizFilial" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_MotiSitu" FOREIGN KEY ("MotivoSituacaoCadastralId") REFERENCES "MotivoSituacaoCadastral" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_Municipio" FOREIGN KEY ("MunicipioId") REFERENCES "Municipio" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_NatuJuri" FOREIGN KEY ("NaturezaJuridicaId") REFERENCES "NaturezaJuridica" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_OpcaoSimples" FOREIGN KEY ("OpcaoSimplesId") REFERENCES "OpcaoSimples" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_Pais" FOREIGN KEY ("PaisId") REFERENCES "Pais" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_Porte" FOREIGN KEY ("PorteId") REFERENCES "Porte" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_QualResp" FOREIGN KEY ("QualificacaoResponsavelId") REFERENCES "Qualificacao" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_SituCada" FOREIGN KEY ("SituacaoCadastralId") REFERENCES "SituacaoCadastral" ("Id");
ALTER TABLE "DadoCadastral" ADD CONSTRAINT "FK_DadoCadastral_UnidFede" FOREIGN KEY ("UnidadeFederacaoId") REFERENCES "UnidadeFederacao" ("Id");
ALTER TABLE "Municipio" ADD CONSTRAINT "FK_Municipio_UnidadeFederacaoId" FOREIGN KEY ("UnidadeFederacaoId") REFERENCES "UnidadeFederacao" ("Id");
ALTER TABLE "Qualificacao" ADD CONSTRAINT "FK_Qualificacao_Logico" FOREIGN KEY ("ColetadoAtualmente") REFERENCES "Logico" ("Codigo");
ALTER TABLE "Socio" ADD CONSTRAINT "FK_Socio_DadoCadastral" FOREIGN KEY ("Cnpj") REFERENCES "DadoCadastral" ("Cnpj");
ALTER TABLE "Socio" ADD CONSTRAINT "FK_Socio_Pais" FOREIGN KEY ("PaisId") REFERENCES "Pais" ("Id");
ALTER TABLE "Socio" ADD CONSTRAINT "FK_Socio_QualificacaoRepr" FOREIGN KEY ("QualificacaoRepresentanteId") REFERENCES "Qualificacao" ("Id");
ALTER TABLE "Socio" ADD CONSTRAINT "FK_Socio_QualificacaoSocio" FOREIGN KEY ("QualificacaoSocioId") REFERENCES "Qualificacao" ("Id");
ALTER TABLE "Socio" ADD CONSTRAINT "FK_Socio_TipoSocio" FOREIGN KEY ("TipoSocioId") REFERENCES "TipoSocio" ("Id");
ALTER TABLE "UnidadeFederacao" ADD CONSTRAINT "FK_UnidadeFederacao_Pais" FOREIGN KEY ("PaisId") REFERENCES "Pais" ("Id");

