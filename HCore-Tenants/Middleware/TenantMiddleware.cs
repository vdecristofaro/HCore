﻿using HCore.Tenants.Models;
using HCore.Tenants.Providers;
using HCore.Web.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HCore.Tenants.Middleware
{
    internal class TenantsMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ITenantDataProvider _tenantDataProvider;

        private readonly ILogger<TenantsMiddleware> _logger;

        public TenantsMiddleware(RequestDelegate next, ITenantDataProvider tenantDataProvider, ILogger<TenantsMiddleware> logger)
        {
            _next = next;

            _tenantDataProvider = tenantDataProvider;

            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Items.ContainsKey(TenantConstants.TenantInfoContextKey))
            {
                ITenantInfo tenantInfo = null;

                var hostString = context.Request.Host;

                string host = null;

                if (hostString.HasValue)
                {
                    host = hostString.Host;
                    
                    tenantInfo = _tenantDataProvider.LookupTenantByHost(host);
                }

                if (tenantInfo == null)
                {
                    // we could not find any tenant

                    _logger.LogError($"No tenant found for host {hostString}");

                    throw new NotFoundApiException(NotFoundApiException.TenantNotFound, $"The tenant for host {host} was not found", host);                    
                }

                context.Items.Add(TenantConstants.TenantInfoContextKey, tenantInfo);
            }

            await _next.Invoke(context).ConfigureAwait(false);
        }
    }
}
