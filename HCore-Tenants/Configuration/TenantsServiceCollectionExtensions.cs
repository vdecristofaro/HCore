﻿using HCore.Tenants;
using HCore.Tenants.Database.SqlServer;
using HCore.Tenants.Impl;
using HCore.Web.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantsServiceCollectionExtensions
    {
        public static TenantsBuilder AddTenants<TStartup>(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlServer<TStartup, SqlServerTenantDbContext>("Tenant", configuration);

            services.AddSingleton<ITenantDataProvider, TenantDataProviderImpl>();
           
            services.TryAddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.GetTenantInfo());
            services.TryAddSingleton<ITenantInfoAccessor, TenantInfoAccessorImpl>();

            services.AddScoped<HCore.Tenants.IUrlProvider, UrlProviderImpl>();
            var descriptor = new ServiceDescriptor(typeof(HCore.Web.Providers.IUrlProvider), typeof(UrlProviderImpl), ServiceLifetime.Scoped);

            services.Replace(descriptor);
            
            return new TenantsBuilder(services);
        }
    }    
}