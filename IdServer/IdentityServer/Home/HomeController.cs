// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdServer.Data.Context;
using IdServer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerHost.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;

        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger, IdServerDataContext context)
        {
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
            //CreateData(context);
            //CreateAppData(context);
        }

        private void CreateAppData(IdServerDataContext context)
        {
            var appClient = new StoredClient 
            {
                ClientId = "QCUniversidad.AppClient",
                AlwaysIncludeUserClaimsInIdToken = true,
                RedirectUris = new List<StoredClientRedirectUris> 
                { 
                    new StoredClientRedirectUris { Url = "QCUniversidad.AppClient://" } 
                },
                PostLogoutRedirectUris = new List<StoredClientPostLogoutRedirectUris>
                { 
                    new StoredClientPostLogoutRedirectUris { Url = "QCUniversidad.AppClient://" }
                },
                GrantTypes = GrantTypes.Code.Select(x => new StoredClientGrantType { GrantType = x }).ToList(),
                Secrets = new List<StoredClientSecret> 
                {
                    new StoredClientSecret { Value = "qcforlife2022".ToSha256() }
                },
                AllowedScopes = new List<StoredClientAllowedScope>
                {
                    new StoredClientAllowedScope { AllowedScope = "qcuniversidad.api" }
                }
            };
            var api = new StoredApiResource
            {
                DisplayName = "Api de gestión de la aplicación QCUniversidad",
                Name = "qcuniversidad.api",
                Scopes = new List<StoredApiResourceScope> 
                {
                    new StoredApiResourceScope { Scope = "qcuniversidad.api" }
                }
            };
            var apiscope = new StoredApiScope 
            {
                DisplayName = api.DisplayName,
                Name = api.Name,
                Emphasize = true,
                Required = true,
            };
            context.Add(appClient);
            context.Add(api);
            context.Add(apiscope);

            context.SaveChanges();
        }

        private void CreateData(IdServerDataContext context)
        {
            GenerateUsers(context);
            GenerateApiResources(context);
            GenerateClients(context);
            GenerateIdentityResources(context);
            context.SaveChangesAsync();
        }

        private static void GenerateIdentityResources(IdServerDataContext context)
        {
            var idresources = new List<StoredIdentityResource>
            {
                new StoredIdentityResource
                {
                    Name = IdentityServerConstants.StandardScopes.OpenId,
                    DisplayName = "Your user identifier",
                    Required = true,
                    Emphasize = false,
                    ShowInDiscoveryDocument = true,
                    UserClaims = new List<StoredIdentityResourceUserClaim> { new StoredIdentityResourceUserClaim { UserClaim = JwtClaimTypes.Subject } }
                },
                new StoredIdentityResource
                {
                    Name = IdentityServerConstants.StandardScopes.Profile,
                    DisplayName = "User profile",
                    Description = "Your user profile information (first name, last name, etc.)",
                    Required = false,
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    UserClaims = new List<StoredIdentityResourceUserClaim>
                    {
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "name",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "family_name",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "given_name",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "middle_name",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "nickname",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "preferred_username",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "profile",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "picture",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "website",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "gender",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "birthdate",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "zoneinfo",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "locale",
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "updated_at",
                        }
                    }
                },
                new StoredIdentityResource
                {
                    DisplayName = "Your postal address",
                    Name = "address",
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    UserClaims = new List<StoredIdentityResourceUserClaim>
                    {
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "address"
                        }
                    }
                },
                new StoredIdentityResource
                {
                    DisplayName = "Your phone number",
                    Name = "phone",
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    UserClaims = new List<StoredIdentityResourceUserClaim>
                    {
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "phone_number"
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "phone_number_verified"
                        }
                    }
                },
                new StoredIdentityResource
                {
                    DisplayName = "Your email address",
                    Name = "email",
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    UserClaims = new List<StoredIdentityResourceUserClaim>
                    {
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "email"
                        },
                        new StoredIdentityResourceUserClaim
                        {
                            UserClaim = "email_verified"
                        }
                    }
                }
            };
            foreach (var ir in idresources)
            {
                context.IdentityResources.Add(ir);
            }
        }

        private static void GenerateClients(IdServerDataContext context)
        {
            var client1 = new StoredClient
            {
                ClientId = "client1",
                Secrets = new List<StoredClientSecret> 
                { 
                    new StoredClientSecret { Value = "cualquier secreto es malo".ToSha256() } 
                },
                GrantTypes = GrantTypes.ClientCredentials.Select(x => new StoredClientGrantType { GrantType = x }).ToList(),
                AllowedScopes = new List<StoredClientAllowedScope> 
                { 
                    new StoredClientAllowedScope { AllowedScope = "api1" } 
                }
            };
            var client2 = new StoredClient
            {
                ClientId = "client2",
                Secrets = new List<StoredClientSecret> 
                { 
                    new StoredClientSecret { Value = "cualquier secreto es malo 2".ToSha256() } 
                },
                GrantTypes = GrantTypes.Code.Select(x => new StoredClientGrantType { GrantType = x }).ToList(),
                AllowedScopes = new List<StoredClientAllowedScope> 
                { 
                    new StoredClientAllowedScope { AllowedScope = "api1" }, 
                    new StoredClientAllowedScope { AllowedScope = "api2" } 
                }
            };
            context.Clients.Add(client1);
            context.Clients.Add(client2);
        }

        private static void GenerateApiResources(IdServerDataContext context)
        {
            var api1 = new StoredApiResource
            {
                DisplayName = "Api de prueba no. 1",
                Name = "api1",
                Scopes = new List<StoredApiResourceScope>
                {
                    new StoredApiResourceScope { Scope = "api1" }
                }
            };
            var scopeApi1 = new StoredApiScope
            {
                Name = api1.Name,
                DisplayName = api1.DisplayName
            };
            var api2 = new StoredApiResource
            {
                DisplayName = "Api de prueba no. 2",
                Name = "api2",
                Scopes = new List<StoredApiResourceScope>
                {
                    new StoredApiResourceScope { Scope = "api2" }
                }
            };
            var scopeApi2 = new StoredApiScope
            {
                Name = api2.Name,
                DisplayName = api2.DisplayName
            };
            context.ApiResources.Add(api1);
            context.ApiScopes.Add(scopeApi1);
            context.ApiResources.Add(api2);
            context.ApiScopes.Add(scopeApi2);
        }

        private static void GenerateUsers(IdServerDataContext context)
        {
            var user = new StoredUser
            {
                IsActive = true,
                Username = "alice",
                Claims = new List<StoredUserClaim>
                {
                    new StoredUserClaim
                    {
                        Type = JwtClaimTypes.GivenName,
                        Value = "Alice"
                    },
                    new StoredUserClaim
                    {
                        Type = JwtClaimTypes.FamilyName,
                        Value = "Smith"
                    },
                    new StoredUserClaim
                    {
                        Type = JwtClaimTypes.Email,
                        Value = "alicesmith@email.com"
                    },
                    new StoredUserClaim
                    {
                        Type = JwtClaimTypes.EmailVerified,
                        Value = "true",
                        ValueType = ClaimValueTypes.Boolean,
                    },
                    new StoredUserClaim
                    {
                        Type = JwtClaimTypes.WebSite,
                        Value = "http://alice.com"
                    },
                    new StoredUserClaim
                    {
                        Type = JwtClaimTypes.Address,
                        Value = JsonConvert.SerializeObject(new
                        {
                            street_address = "One Hacker Way",
                            locality = "Heidelberg",
                            postal_code = 69118,
                            country = "Germany"
                        }),
                        ValueType = "json"
                    }
                },
                Secrets = new StoredUserSecrets { Password = "alice" }
            };
            context.StoredUsers.Add(user);
        }

        public IActionResult Index()
        {
            if (_environment.IsDevelopment())
            {
                // only show in development
                return View();
            }

            _logger.LogInformation("Homepage is disabled in production. Returning 404.");
            return NotFound();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }
    }
}