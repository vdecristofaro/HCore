﻿using AspNetCore.DataProtection.SqlServer;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using HCore.Identity;
using HCore.Identity.Controllers.API.Impl;
using HCore.Identity.Database.SqlServer;
using HCore.Identity.Database.SqlServer.Models.Impl;
using HCore.Identity.Generated.Controllers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServiceCollectionExtensions
    {        
        public static IServiceCollection AddCoreIdentity<TStartup>(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(TStartup).GetTypeInfo().Assembly.GetName().Name;

            string connectionString = configuration[$"SqlServer:Identity:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("SQL Server connection string is empty");

            ConfigureSqlServer<TStartup>(services, configuration);
            ConfigureDataProtection(services, connectionString, configuration);
            ConfigureAspNetIdentity(services, configuration);
            ConfigureIdentityServer(services, connectionString, migrationsAssembly, configuration);
            ConfigureJwtAuthentication(services, configuration);

            // see https://github.com/IdentityServer/IdentityServer4.Samples/blob/release/Quickstarts/Combined_AspNetIdentity_and_EntityFrameworkStorage/src/IdentityServerWithAspIdAndEF/Startup.cs#L84

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddSingleton<IEmailSender, HCore.Identity.EmailSender.Impl.EmailSenderImpl>();

            services.AddScoped<ISecureApiController, SecureApiImpl>();

            return services;
        }

        private static void ConfigureSqlServer<TStartup>(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlServer<TStartup, SqlServerIdentityDbContext>("Identity", configuration);

            // lightweight one for default UI implementations
            services.AddSqlServer<TStartup, IdentityDbContext>("Identity", configuration);
        }

        private static void ConfigureDataProtection(IServiceCollection services, string connectionString, IConfiguration configuration)
        {
            string applicationName = configuration[$"Identity:Application:Name"];
            if (string.IsNullOrEmpty(applicationName))
                throw new Exception("Identity application name is empty");

            services.AddDataProtection()
                .PersistKeysToSqlServer(connectionString, "dbo", "DataProtectionKeys")
                .SetApplicationName(applicationName);
        }

        private static void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            string oidcAuthority = configuration[$"Identity:Oidc:Authority"];
            if (string.IsNullOrEmpty(oidcAuthority))
                throw new Exception("Identity OIDC authority string is empty");

            string oidcAudience = configuration[$"Identity:Oidc:Audience"];
            if (string.IsNullOrEmpty(oidcAudience))
                throw new Exception("Identity audience string is empty");

            var authenticationBuilder = services.AddAuthentication();

            bool useJwt = configuration.GetValue<bool>("WebServer:UseJwt");

            if (useJwt)
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

                authenticationBuilder.AddJwtBearer(IdentityCoreConstants.JwtScheme, jwt =>
                {
                    jwt.Authority = oidcAuthority;
                    jwt.RequireHttpsMetadata = true;
                    jwt.Audience = oidcAudience;
                });
            }            
        }

        private static void ConfigureAspNetIdentity(IServiceCollection services, IConfiguration configuration)
        {
            // now, on second priority, comes the identity which we tweaked

            string authCookieDomain = configuration[$"Identity:AuthCookie:Domain"];
            if (string.IsNullOrEmpty(authCookieDomain))
                throw new Exception("Identity auth cookie domain is empty");

            var identityBuilder = services.AddIdentity<UserModel, IdentityRole>();
            
            identityBuilder.AddEntityFrameworkStores<SqlServerIdentityDbContext>();
            identityBuilder.AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Domain = authCookieDomain;
                options.Cookie.Name = "RH.Core.Identity.session";
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
        }

        private static void ConfigureIdentityServer(IServiceCollection services, string connectionString, string migrationsAssembly, IConfiguration configuration)
        {
            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction.ErrorUrl = "/Account/Error";
                options.UserInteraction.ConsentUrl = "/Account/Consent";
            });

            // see http://amilspage.com/signing-certificates-idsv4/

            identityServerBuilder.AddSigningCredential(GetSigningKeyCertificate(configuration));

            identityServerBuilder.AddAspNetIdentity<UserModel>();

            // this adds the config data from DB (clients, resources)
            identityServerBuilder.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = dbContextBuilder =>
                    dbContextBuilder.UseSqlServer(connectionString,
                        sqlServerOptions => sqlServerOptions.MigrationsAssembly(migrationsAssembly));
            });

            // this adds the operational data from DB (codes, tokens, consents)
            identityServerBuilder.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = dbContextBuilder =>
                    dbContextBuilder.UseSqlServer(connectionString,
                        sqlServerOptions => sqlServerOptions.MigrationsAssembly(migrationsAssembly));

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                // options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
            });            
        }

        private static X509Certificate2 GetSigningKeyCertificate(IConfiguration configuration)
        {
            string signingKeyAssembly = configuration["Identity:SigningKey:Assembly"];
            if (string.IsNullOrEmpty(signingKeyAssembly))
                throw new Exception("Identity signing assembly not found");

            string signingKeyName = configuration["Identity:SigningKey:Name"];
            if (string.IsNullOrEmpty(signingKeyName))
                throw new Exception("Identity signing key name not found");

            string signingKeyPassword = configuration["Identity:SigningKey:Password"];
            if (string.IsNullOrEmpty(signingKeyPassword))
                throw new Exception("Identity signing password not found");

            Assembly _signingKeyAssembly = AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == signingKeyAssembly);

            if (signingKeyAssembly == null)
                throw new Exception("Identity signing key assembly is not present in the list of assemblies");

            var resourceStream = _signingKeyAssembly.GetManifestResourceStream(signingKeyName);

            if (resourceStream == null)
                throw new Exception("Identity core signing key resource not found");

            using (var memory = new MemoryStream((int)resourceStream.Length))
            {
                resourceStream.CopyTo(memory);

                return new X509Certificate2(memory.ToArray(), signingKeyPassword);
            }
        }
    }    
}