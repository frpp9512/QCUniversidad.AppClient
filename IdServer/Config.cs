// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.Phone()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope 
                {
                    Name = "QCUniversidad.Api",
                    DisplayName = "Api de gestión de la aplicación QCUniversidad",
                    ShowInDiscoveryDocument = true,
                    Emphasize = true,
                    Required = true
                },
                new ApiScope
                {
                    Name = "QCUniversidad.Api.Admin",
                    DisplayName = "Api de gestión de la aplicación QCUniversidad - Acceso administrativo",
                    ShowInDiscoveryDocument = true,
                    Emphasize = true,
                    Required = true
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            {
                new Client 
                {
                    ClientId = "QCUniversidad.AppClient",
                    ClientName = "QCUniversidad.AppClient",
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientSecrets = new Secret[] { new Secret("nzZlohrelfycvrWjyh0Cs7zsQDeGq7x3HDhx4BLbmr0=") },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = new string[] { "openid", "profile", "roles", "qcuniversidad.api" },
                    RedirectUris = new string[] { "qcuniversidadappclient://" },
                    PostLogoutRedirectUris = new string[] { "qcuniversidadappclient://" },
                },
                new Client
                {
                    ClientId = "client1",
                    ClientName = "client1",
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientSecrets = new Secret[] { new Secret("cualquier secreto es malo".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new string[] { "api1" }
                },
                new Client
                {
                    ClientId = "client2",
                    ClientName = "client2",
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientSecrets = new Secret[] { new Secret("cualquier secreto es malo 2".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new string[] { "openid", "profile", "api1", "api2" },
                    RedirectUris = new string[] { "https://localhost:7050/signin-oidc" }
                },
            };
    }
}