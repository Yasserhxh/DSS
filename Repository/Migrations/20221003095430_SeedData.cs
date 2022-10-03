using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Article_Id", "Designation", "Tarif" },
                values: new object[,]
                {
                    { 1, "Beton 1", 50.0 },
                    { 2, "Beton 2", 60.0 },
                    { 3, "Beton 3", 70.0 },
                    { 4, "Beton Spécial", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "126a635b-d763-48bf-ab0e-f5599cffb4fe", "0748555d-5884-467c-a3ba-88130f9ee2e7", "Prescripteur technique", "PRESCRIPTEUR TECHNIQUE" },
                    { "1588b3b9-37bc-4f58-acf9-e42fd47f1c28", "2bf243d6-5aa0-40c1-bc3c-470ba24c7dfb", "Chef de ventes", "CHEF DE VENTES" },
                    { "3e130fc1-981e-4183-a12d-fa5f73d13bb2", "8223a248-89fb-4aec-95bb-f83ead8ccdd1", "Administration des ventes", "ADMINISTRATION DES VENTES" },
                    { "48e33a01-bd1f-4739-a27f-126e8e8b2d1c", "8bb2741f-9704-4d37-a501-6741d14d5f93", "Admin", "ADMIN" },
                    { "6eafdfbe-ed07-4687-9d2c-0b767b15a305", "42d08be2-5105-4aca-94ce-ad8c9129fbff", "Responsable commercial", "RESPONSABLE COMMERCIAL" },
                    { "7b8ab704-463e-4074-8c19-a62905f62e11", "b46a563a-b39b-491a-b71e-aef526e640c4", "Controle crédit", "CONTROLE CREDIT" },
                    { "9be024db-d122-4cd2-8329-2afd0d259e77", "0a545942-a790-4345-8674-88f653b3d100", "DA BPE", "DA BPE" },
                    { "a7846740-cb77-4d25-8da6-d5c68dfb590d", "30aac02c-8443-4e5e-b07e-46a508938b04", "Responsable logistique", "RESPONSABLE LOGISTIQUE" },
                    { "ab68ea17-b8d8-490a-aa11-75ff2973c01e", "da46d04a-d1de-42ee-aa4b-38f8669528c2", "Commercial", "COMMERCIAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "Nom", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Prenom", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2e142fe7-a372-4b98-ab9d-dcc4e4966b88", 0, "7d8ee1a9-ab94-4afc-bb77-7dd014156a25", null, false, false, false, null, "RCNom", null, "RC", "AQAAAAEAACcQAAAAEDrXcu1fcDfKrt6aNuproHNrJ3xx4roGVcwHiQe6KGO7RSb/QGfzAOhLsIyGRasMfA==", null, false, "RCPrenom", "925b8781-e141-4f60-9dfc-293d3e24b78f", false, "RC" },
                    { "3375dc1e-b359-403e-9f13-5e2b395ffafc", 0, "93013dd1-eb1c-458b-a397-f07a4d01e5f6", "t.abdelmajid@Alexsys.solutions", true, false, false, null, null, "T.ABDELMAJID@ALEXSYS.SOLUTIONS", "ADMIN", "AQAAAAEAACcQAAAAELfYBUTvEAubMIkjap6ZKZU7Fr9SVYbMLgokgubRXPLBzLMpWndYUBJE90zhtZ/hcg==", null, false, null, "78806f6d-7f1f-4d2f-9a13-ce06383ed217", false, "admin" },
                    { "3e45b10c-0d15-49f2-903b-7de4bbc62f98", 0, "4ff6ca60-b596-4c08-9177-78379769437b", null, false, false, false, null, "PTNom", null, "PT", "AQAAAAEAACcQAAAAEFV0UoW+VSGxwM2SoLOc4eYoApxr0xoIIZF9LGqTHrR1FIe2iqkbJzSTBghzbaeToA==", null, false, "PTPrenom", "58053aac-a438-4e44-acce-4f967d8df300", false, "PT" },
                    { "48e50810-f7fc-45c8-8d13-0a03a6a8b1de", 0, "a6a669fd-f838-4fbc-967c-2fe45b02bda7", null, false, false, false, null, "CCNom", null, "CC", "AQAAAAEAACcQAAAAEJRyaeytwUC7u0TPLi2ObUhuIrmnc2Fx1KhfzC3CcyhkGwDgzzB9yxOWwlog8s1HQA==", null, false, "CCPrenom", "8e031d55-7c1e-4a55-896b-288a6f628ecd", false, "CC" },
                    { "7167d11d-9358-4262-b7a4-77372e1c121d", 0, "a44029d7-21cd-4504-8e7b-9d06cfa06e22", null, false, false, false, null, "CVNom", null, "CV", "AQAAAAEAACcQAAAAEPhqIY39UAWwZCPxG1+zjh3zLwtj/dqto4nkCQcRCiLRBiWp8rdl0IKipWtvVQrpUA==", null, false, "CVPrenom", "45a41cea-397f-4837-9706-945073586524", false, "CV" },
                    { "b8888a0f-ebcf-4b0a-815a-83ccc0a4c349", 0, "1d46eb0e-e1a3-43f5-b0c5-5c297547601b", null, false, false, false, null, "DAPBENom", null, "DABPE", "AQAAAAEAACcQAAAAEKILS6AhXb9SC6VxeKWvTQrGn453uWRRbQUBIdPzkJXISS4K4JO1YYvXvljgxSF8AQ==", null, false, "DAPBEPrenom", "54227a7c-67ee-4ebb-b9d0-272ccb30e483", false, "DAPBE" },
                    { "d19f9bc1-13b4-42b3-881a-a847f4c0684e", 0, "bb77036a-bf60-4249-82c1-3ddfc75311e0", null, false, false, false, null, "RLNom", null, "RL", "AQAAAAEAACcQAAAAEB1o/Igh/C5MIqzrZhvM5bOGlnfYkNzyVLG2PmfTSj9m1JHebEE8XvkbsvKAvtVNXA==", null, false, "RLPrenom", "adf0467d-bf4f-4bf5-a576-078dac83af6b", false, "RL" },
                    { "f32d5249-cfe5-4b76-909e-4ef6d73cd504", 0, "c628c842-66a9-4496-912e-ba81ecbcd02e", null, false, false, false, null, "CNom", null, "COMMERCIAL", "AQAAAAEAACcQAAAAEPMPKXvQjbCSYTmmF30iyI18EqJUMwilOByfsYEeEK8WjakVjJ/SWNz4s9ophsAJZA==", null, false, "CPrenom", "8e031d55-7c1e-4a55-896b-288a6f628ecd", false, "Commercial" },
                    { "fe1300d0-ecf8-4bb5-afaf-5030b27959bd", 0, "ec48f552-56aa-4998-ab5c-e2b57df36f7c", null, false, false, false, null, "ADVNome", null, "ADV", "AQAAAAEAACcQAAAAEM01AKiu2t9LlsZUAyoZnIPfMZxnIqc/nnAOZ+vLCYEZLTaaw00O7AWEmPmgwWC/+A==", null, false, "ADVPrenom", "02d7a2a5-9486-4980-b031-b1728f9125f1", false, "ADV" }
                });

            migrationBuilder.InsertData(
                table: "Forme_Juridique",
                columns: new[] { "FormeJuridique_Id", "FormeJuridique_Libelle" },
                values: new object[,]
                {
                    { 1, "SA" },
                    { 2, "SARL" }
                });

            migrationBuilder.InsertData(
                table: "Pays",
                columns: new[] { "IdPays", "CodePaysSap", "NomPays" },
                values: new object[] { 1, "MA", "Maroc" });

            migrationBuilder.InsertData(
                table: "Statut",
                columns: new[] { "IdStatut", "CodeStatutSap", "Libelle" },
                values: new object[,]
                {
                    { 1, "1", "Etude et proposition de prix" },
                    { 2, "2", "Parametrage des prix PBE" },
                    { 3, "3", "Validation de l'offre de prix" },
                    { 4, "4", "Fixation de prix du transport" },
                    { 5, "5", "Parametrage des prix de services" },
                    { 6, "6", "Validé" },
                    { 7, "7", "En cours de validation" }
                });

            migrationBuilder.InsertData(
                table: "Tarif_Pompe",
                columns: new[] { "Tpr_Id", "LongFleche_Libelle", "LongFleche_Prix" },
                values: new object[,]
                {
                    { 1, "48", 50m },
                    { 2, "50", 60m },
                    { 3, "52", 70m }
                });

            migrationBuilder.InsertData(
                table: "Type_Chantier",
                columns: new[] { "Tc_Id", "Tc_Libelle" },
                values: new object[,]
                {
                    { 1, "Type 1" },
                    { 2, "Type 2" },
                    { 3, "Type 3" }
                });

            migrationBuilder.InsertData(
                table: "Unite",
                columns: new[] { "IdUnite", "Libelle" },
                values: new object[] { 1, "m3" });

            migrationBuilder.InsertData(
                table: "Zone",
                columns: new[] { "Zone_Id", "Zone_Libelle", "Zone_Prix" },
                values: new object[,]
                {
                    { 1, "Zone 1", 50m },
                    { 2, "Zone 2", 60m },
                    { 3, "Zone 3", 70m }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "6eafdfbe-ed07-4687-9d2c-0b767b15a305", "2e142fe7-a372-4b98-ab9d-dcc4e4966b88" },
                    { "48e33a01-bd1f-4739-a27f-126e8e8b2d1c", "3375dc1e-b359-403e-9f13-5e2b395ffafc" },
                    { "126a635b-d763-48bf-ab0e-f5599cffb4fe", "3e45b10c-0d15-49f2-903b-7de4bbc62f98" },
                    { "7b8ab704-463e-4074-8c19-a62905f62e11", "48e50810-f7fc-45c8-8d13-0a03a6a8b1de" },
                    { "1588b3b9-37bc-4f58-acf9-e42fd47f1c28", "7167d11d-9358-4262-b7a4-77372e1c121d" },
                    { "9be024db-d122-4cd2-8329-2afd0d259e77", "b8888a0f-ebcf-4b0a-815a-83ccc0a4c349" },
                    { "a7846740-cb77-4d25-8da6-d5c68dfb590d", "d19f9bc1-13b4-42b3-881a-a847f4c0684e" },
                    { "ab68ea17-b8d8-490a-aa11-75ff2973c01e", "f32d5249-cfe5-4b76-909e-4ef6d73cd504" },
                    { "3e130fc1-981e-4183-a12d-fa5f73d13bb2", "fe1300d0-ecf8-4bb5-afaf-5030b27959bd" }
                });

            migrationBuilder.InsertData(
                table: "Ville",
                columns: new[] { "IdVille", "CodePaysSap", "CodeVilleSap", "IdPays", "NomVille" },
                values: new object[,]
                {
                    { 1, "MA", "R", 1, "Rabat" },
                    { 2, "MA", "C", 1, "Casablanca" }
                });

            migrationBuilder.InsertData(
                table: "Centrale_Beton",
                columns: new[] { "Ctr_Id", "Ctr_Adresse", "Ctr_CodePostal", "Ctr_Email", "Ctr_Gsm", "Ctr_Latiture", "Ctr_Longitude", "Ctr_Nom", "Ctr_Responsable", "Ctr_Responsable_Gsm", "Ctr_Ville_Id", "Rayon" },
                values: new object[] { 1, "Adresse 1", 20250, "Centrale1@gmail.com", "0620055784", null, "-7.618710247586268", "Centrale 1", "Test", "0620055784", 2, null });

            migrationBuilder.InsertData(
                table: "Centrale_Beton",
                columns: new[] { "Ctr_Id", "Ctr_Adresse", "Ctr_CodePostal", "Ctr_Email", "Ctr_Gsm", "Ctr_Latiture", "Ctr_Longitude", "Ctr_Nom", "Ctr_Responsable", "Ctr_Responsable_Gsm", "Ctr_Ville_Id", "Rayon" },
                values: new object[] { 2, "Adresse 2", 20250, "Centrale2@gmail.com", "0520055784", null, "-7.533456", "Centrale 2", "Test 2", "0625255784", 2, null });

            migrationBuilder.InsertData(
                table: "Centrale_Beton",
                columns: new[] { "Ctr_Id", "Ctr_Adresse", "Ctr_CodePostal", "Ctr_Email", "Ctr_Gsm", "Ctr_Latiture", "Ctr_Longitude", "Ctr_Nom", "Ctr_Responsable", "Ctr_Responsable_Gsm", "Ctr_Ville_Id", "Rayon" },
                values: new object[] { 3, "Adresse 3", 20256, "Centrale3@gmail.com", "0525055784", null, "-7,6738", "Centrale 3", "Test 3", "0625285784", 2, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Article_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Article_Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Article_Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Article_Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6eafdfbe-ed07-4687-9d2c-0b767b15a305", "2e142fe7-a372-4b98-ab9d-dcc4e4966b88" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "48e33a01-bd1f-4739-a27f-126e8e8b2d1c", "3375dc1e-b359-403e-9f13-5e2b395ffafc" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "126a635b-d763-48bf-ab0e-f5599cffb4fe", "3e45b10c-0d15-49f2-903b-7de4bbc62f98" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7b8ab704-463e-4074-8c19-a62905f62e11", "48e50810-f7fc-45c8-8d13-0a03a6a8b1de" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1588b3b9-37bc-4f58-acf9-e42fd47f1c28", "7167d11d-9358-4262-b7a4-77372e1c121d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9be024db-d122-4cd2-8329-2afd0d259e77", "b8888a0f-ebcf-4b0a-815a-83ccc0a4c349" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a7846740-cb77-4d25-8da6-d5c68dfb590d", "d19f9bc1-13b4-42b3-881a-a847f4c0684e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ab68ea17-b8d8-490a-aa11-75ff2973c01e", "f32d5249-cfe5-4b76-909e-4ef6d73cd504" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3e130fc1-981e-4183-a12d-fa5f73d13bb2", "fe1300d0-ecf8-4bb5-afaf-5030b27959bd" });

            migrationBuilder.DeleteData(
                table: "Centrale_Beton",
                keyColumn: "Ctr_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Centrale_Beton",
                keyColumn: "Ctr_Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Centrale_Beton",
                keyColumn: "Ctr_Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Forme_Juridique",
                keyColumn: "FormeJuridique_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Forme_Juridique",
                keyColumn: "FormeJuridique_Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statut",
                keyColumn: "IdStatut",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Statut",
                keyColumn: "IdStatut",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statut",
                keyColumn: "IdStatut",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Statut",
                keyColumn: "IdStatut",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Statut",
                keyColumn: "IdStatut",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Statut",
                keyColumn: "IdStatut",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Statut",
                keyColumn: "IdStatut",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tarif_Pompe",
                keyColumn: "Tpr_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tarif_Pompe",
                keyColumn: "Tpr_Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tarif_Pompe",
                keyColumn: "Tpr_Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Type_Chantier",
                keyColumn: "Tc_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Type_Chantier",
                keyColumn: "Tc_Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Type_Chantier",
                keyColumn: "Tc_Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Unite",
                keyColumn: "IdUnite",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ville",
                keyColumn: "IdVille",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Zone",
                keyColumn: "Zone_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Zone",
                keyColumn: "Zone_Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Zone",
                keyColumn: "Zone_Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "126a635b-d763-48bf-ab0e-f5599cffb4fe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1588b3b9-37bc-4f58-acf9-e42fd47f1c28");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e130fc1-981e-4183-a12d-fa5f73d13bb2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48e33a01-bd1f-4739-a27f-126e8e8b2d1c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6eafdfbe-ed07-4687-9d2c-0b767b15a305");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b8ab704-463e-4074-8c19-a62905f62e11");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9be024db-d122-4cd2-8329-2afd0d259e77");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7846740-cb77-4d25-8da6-d5c68dfb590d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab68ea17-b8d8-490a-aa11-75ff2973c01e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e142fe7-a372-4b98-ab9d-dcc4e4966b88");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3375dc1e-b359-403e-9f13-5e2b395ffafc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3e45b10c-0d15-49f2-903b-7de4bbc62f98");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "48e50810-f7fc-45c8-8d13-0a03a6a8b1de");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7167d11d-9358-4262-b7a4-77372e1c121d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b8888a0f-ebcf-4b0a-815a-83ccc0a4c349");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d19f9bc1-13b4-42b3-881a-a847f4c0684e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f32d5249-cfe5-4b76-909e-4ef6d73cd504");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe1300d0-ecf8-4bb5-afaf-5030b27959bd");

            migrationBuilder.DeleteData(
                table: "Ville",
                keyColumn: "IdVille",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pays",
                keyColumn: "IdPays",
                keyValue: 1);
        }
    }
}
