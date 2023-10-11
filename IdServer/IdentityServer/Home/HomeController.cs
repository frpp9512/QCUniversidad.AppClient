// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using Client = IdentityServer4.EntityFramework.Entities.Client;
using IdentityResource = IdentityServer4.EntityFramework.Entities.IdentityResource;

namespace IdServer.IdentityServer.Home;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ConfigurationDbContext _configurationDbContext;

    public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ConfigurationDbContext configurationDbContext)
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _configurationDbContext = configurationDbContext;
        //CreateClient();
        //CreateUserAndRoles();
        //AddConfigurationResources();
        //AddApiResources();
    }

    private void CreateClient()
    {
        var client = new Client
        {
            ClientId = "QCUniversidad.WebClient",
            ClientName = "QCUniversidad.WebClient",
            AllowedGrantTypes = new List<ClientGrantType>
            {
                new ClientGrantType { GrantType = "client_credentials" }
            },
            AllowedScopes = new List<ClientScope>
            {
                new ClientScope
                {
                    Scope = "qcuniversidad.api"
                }
            },
            ClientSecrets = new List<ClientSecret>
            {
                new ClientSecret
                {
                    Value = "qcuforlife2022".ToSha256()
                }
            }
        };
        _ = _configurationDbContext.Clients.Add(client);
        _ = _configurationDbContext.SaveChanges();
    }

    private void CreateUserAndRoles()
    {
        var user = new IdentityUser("frank")
        {
            Email = "frpp9512@outlook.com",
            EmailConfirmed = true,
            PhoneNumber = "+53 58591754"
        };
        _userManager.CreateAsync(user, "mr.hyde").Wait();
        var role = new IdentityRole("QCU.Admin");
        _roleManager.CreateAsync(role).Wait();
        _userManager.AddToRoleAsync(user, "QCU.Admin").Wait();
    }

    private void AddApiResources()
    {
        var apiResource = new ApiResource()
        {
            DisplayName = "QCUniversidad.Api",
            Name = "qcuniversidad.api",
            Description = "Acceso de cliente a QCUniversidad.Api",
            ShowInDiscoveryDocument = true,
            Enabled = true,
            Scopes = new List<ApiResourceScope>
            {
                new ApiResourceScope
                {
                    Scope = "qcuniversidad.api"
                }
            }
        };
        _ = _configurationDbContext.Add(apiResource);
        _ = _configurationDbContext.SaveChanges();
    }

    private void AddConfigurationResources()
    {
        var idResource = new IdentityResource
        {
            Name = "roles",
            UserClaims = new List<IdentityResourceClaim>
            {
                new IdentityResourceClaim { Type = "role" }
            }
        };
        _ = _configurationDbContext.Add(idResource);

        var client = _configurationDbContext.Clients.Where(c => c.ClientId == "QCUniversidad.AppClient").FirstOrDefault();

        if (client is not null)
        {
            var scope = new ClientScope
            {
                Client = client,
                Scope = idResource.Name
            };
        }

        _ = _configurationDbContext.SaveChanges();
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