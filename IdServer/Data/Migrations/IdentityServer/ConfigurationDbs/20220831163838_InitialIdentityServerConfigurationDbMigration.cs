using Microsoft.EntityFrameworkCore.Migrations;
using System;


namespace IdServer.Data.Migrations.IdentityServer.ConfigurationDbs;

public partial class InitialIdentityServerConfigurationDbMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "ApiResources",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                ShowInDiscoveryDocument = table.Column<bool>(type: "INTEGER", nullable: false),
                Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                Updated = table.Column<DateTime>(type: "TEXT", nullable: true),
                LastAccessed = table.Column<DateTime>(type: "TEXT", nullable: true),
                NonEditable = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_ApiResources", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "ApiScopes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                Required = table.Column<bool>(type: "INTEGER", nullable: false),
                Emphasize = table.Column<bool>(type: "INTEGER", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_ApiScopes", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                ClientId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                ProtocolType = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                RequireClientSecret = table.Column<bool>(type: "INTEGER", nullable: false),
                ClientName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                ClientUri = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                LogoUri = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                RequireConsent = table.Column<bool>(type: "INTEGER", nullable: false),
                AllowRememberConsent = table.Column<bool>(type: "INTEGER", nullable: false),
                AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "INTEGER", nullable: false),
                RequirePkce = table.Column<bool>(type: "INTEGER", nullable: false),
                AllowPlainTextPkce = table.Column<bool>(type: "INTEGER", nullable: false),
                RequireRequestObject = table.Column<bool>(type: "INTEGER", nullable: false),
                AllowAccessTokensViaBrowser = table.Column<bool>(type: "INTEGER", nullable: false),
                FrontChannelLogoutUri = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                FrontChannelLogoutSessionRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                BackChannelLogoutUri = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                BackChannelLogoutSessionRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                AllowOfflineAccess = table.Column<bool>(type: "INTEGER", nullable: false),
                IdentityTokenLifetime = table.Column<int>(type: "INTEGER", nullable: false),
                AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                AccessTokenLifetime = table.Column<int>(type: "INTEGER", nullable: false),
                AuthorizationCodeLifetime = table.Column<int>(type: "INTEGER", nullable: false),
                ConsentLifetime = table.Column<int>(type: "INTEGER", nullable: true),
                AbsoluteRefreshTokenLifetime = table.Column<int>(type: "INTEGER", nullable: false),
                SlidingRefreshTokenLifetime = table.Column<int>(type: "INTEGER", nullable: false),
                RefreshTokenUsage = table.Column<int>(type: "INTEGER", nullable: false),
                UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "INTEGER", nullable: false),
                RefreshTokenExpiration = table.Column<int>(type: "INTEGER", nullable: false),
                AccessTokenType = table.Column<int>(type: "INTEGER", nullable: false),
                EnableLocalLogin = table.Column<bool>(type: "INTEGER", nullable: false),
                IncludeJwtId = table.Column<bool>(type: "INTEGER", nullable: false),
                AlwaysSendClientClaims = table.Column<bool>(type: "INTEGER", nullable: false),
                ClientClaimsPrefix = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                PairWiseSubjectSalt = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                Updated = table.Column<DateTime>(type: "TEXT", nullable: true),
                LastAccessed = table.Column<DateTime>(type: "TEXT", nullable: true),
                UserSsoLifetime = table.Column<int>(type: "INTEGER", nullable: true),
                UserCodeType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                DeviceCodeLifetime = table.Column<int>(type: "INTEGER", nullable: false),
                NonEditable = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Clients", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "IdentityResources",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                Required = table.Column<bool>(type: "INTEGER", nullable: false),
                Emphasize = table.Column<bool>(type: "INTEGER", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>(type: "INTEGER", nullable: false),
                Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                Updated = table.Column<DateTime>(type: "TEXT", nullable: true),
                NonEditable = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_IdentityResources", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "ApiResourceClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ApiResourceId = table.Column<int>(type: "INTEGER", nullable: false),
                Type = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ApiResourceClaims_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ApiResourceProperties",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ApiResourceId = table.Column<int>(type: "INTEGER", nullable: false),
                Key = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Value = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ApiResourceProperties_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ApiResourceScopes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Scope = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                ApiResourceId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ApiResourceScopes_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ApiResourceSecrets",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ApiResourceId = table.Column<int>(type: "INTEGER", nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                Value = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>(type: "TEXT", nullable: true),
                Type = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ApiScopeClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ScopeId = table.Column<int>(type: "INTEGER", nullable: false),
                Type = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ApiScopeClaims_ApiScopes_ScopeId",
                    column: x => x.ScopeId,
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ApiScopeProperties",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ScopeId = table.Column<int>(type: "INTEGER", nullable: false),
                Key = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Value = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ApiScopeProperties_ApiScopes_ScopeId",
                    column: x => x.ScopeId,
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Type = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Value = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientClaims", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientClaims_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientCorsOrigins",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Origin = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientCorsOrigins_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientGrantTypes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                GrantType = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientGrantTypes_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientIdPRestrictions",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Provider = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientIdPRestrictions_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientPostLogoutRedirectUris",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                PostLogoutRedirectUri = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientProperties",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                Key = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Value = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientProperties", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientProperties_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientRedirectUris",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RedirectUri = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientRedirectUris_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientScopes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Scope = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientScopes", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientScopes_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ClientSecrets",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                Value = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>(type: "TEXT", nullable: true),
                Type = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientSecrets_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "IdentityResourceClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                IdentityResourceId = table.Column<int>(type: "INTEGER", nullable: false),
                Type = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
                    column: x => x.IdentityResourceId,
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "IdentityResourceProperties",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                IdentityResourceId = table.Column<int>(type: "INTEGER", nullable: false),
                Key = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Value = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_IdentityResourceProperties_IdentityResources_IdentityResourceId",
                    column: x => x.IdentityResourceId,
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiResourceClaims_ApiResourceId",
            table: "ApiResourceClaims",
            column: "ApiResourceId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiResourceProperties_ApiResourceId",
            table: "ApiResourceProperties",
            column: "ApiResourceId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiResources_Name",
            table: "ApiResources",
            column: "Name",
            unique: true);

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiResourceScopes_ApiResourceId",
            table: "ApiResourceScopes",
            column: "ApiResourceId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiResourceSecrets_ApiResourceId",
            table: "ApiResourceSecrets",
            column: "ApiResourceId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiScopeClaims_ScopeId",
            table: "ApiScopeClaims",
            column: "ScopeId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiScopeProperties_ScopeId",
            table: "ApiScopeProperties",
            column: "ScopeId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiScopes_Name",
            table: "ApiScopes",
            column: "Name",
            unique: true);

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientClaims_ClientId",
            table: "ClientClaims",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientCorsOrigins_ClientId",
            table: "ClientCorsOrigins",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientGrantTypes_ClientId",
            table: "ClientGrantTypes",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientIdPRestrictions_ClientId",
            table: "ClientIdPRestrictions",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientPostLogoutRedirectUris_ClientId",
            table: "ClientPostLogoutRedirectUris",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientProperties_ClientId",
            table: "ClientProperties",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientRedirectUris_ClientId",
            table: "ClientRedirectUris",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Clients_ClientId",
            table: "Clients",
            column: "ClientId",
            unique: true);

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientScopes_ClientId",
            table: "ClientScopes",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientSecrets_ClientId",
            table: "ClientSecrets",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_IdentityResourceClaims_IdentityResourceId",
            table: "IdentityResourceClaims",
            column: "IdentityResourceId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_IdentityResourceProperties_IdentityResourceId",
            table: "IdentityResourceProperties",
            column: "IdentityResourceId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_IdentityResources_Name",
            table: "IdentityResources",
            column: "Name",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "ApiResourceClaims");

        _ = migrationBuilder.DropTable(
            name: "ApiResourceProperties");

        _ = migrationBuilder.DropTable(
            name: "ApiResourceScopes");

        _ = migrationBuilder.DropTable(
            name: "ApiResourceSecrets");

        _ = migrationBuilder.DropTable(
            name: "ApiScopeClaims");

        _ = migrationBuilder.DropTable(
            name: "ApiScopeProperties");

        _ = migrationBuilder.DropTable(
            name: "ClientClaims");

        _ = migrationBuilder.DropTable(
            name: "ClientCorsOrigins");

        _ = migrationBuilder.DropTable(
            name: "ClientGrantTypes");

        _ = migrationBuilder.DropTable(
            name: "ClientIdPRestrictions");

        _ = migrationBuilder.DropTable(
            name: "ClientPostLogoutRedirectUris");

        _ = migrationBuilder.DropTable(
            name: "ClientProperties");

        _ = migrationBuilder.DropTable(
            name: "ClientRedirectUris");

        _ = migrationBuilder.DropTable(
            name: "ClientScopes");

        _ = migrationBuilder.DropTable(
            name: "ClientSecrets");

        _ = migrationBuilder.DropTable(
            name: "IdentityResourceClaims");

        _ = migrationBuilder.DropTable(
            name: "IdentityResourceProperties");

        _ = migrationBuilder.DropTable(
            name: "ApiResources");

        _ = migrationBuilder.DropTable(
            name: "ApiScopes");

        _ = migrationBuilder.DropTable(
            name: "Clients");

        _ = migrationBuilder.DropTable(
            name: "IdentityResources");
    }
}
