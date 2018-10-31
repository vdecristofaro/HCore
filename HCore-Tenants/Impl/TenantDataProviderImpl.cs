﻿using HCore.Tenants.Database.SqlServer;
using HCore.Tenants.Database.SqlServer.Models.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCore.Tenants.Impl
{
    internal class TenantDataProviderImpl : ITenantDataProvider
    {
        private readonly Dictionary<string, DeveloperWrapper> _developerMappings;

        private readonly Dictionary<long, DeveloperWrapper> _developerMappingsByUuid;

        public List<ITenantInfo> Tenants { get; internal set; }

        private readonly ILogger<TenantDataProviderImpl> _logger;

        private class DeveloperWrapper
        {
            private Dictionary<string, ITenantInfo> _tenantInfoMappings;
            private Dictionary<long, ITenantInfo> _tenantInfoMappingsByUuid;

            public List<ITenantInfo> TenantInfos { get; set; }
            
            public DeveloperModel Developer { get; private set; }

            public DeveloperWrapper(DeveloperModel developer)
            {
                Developer = developer;

                _tenantInfoMappings = new Dictionary<string, ITenantInfo>();
                _tenantInfoMappingsByUuid = new Dictionary<long, ITenantInfo>();

                TenantInfos = new List<ITenantInfo>();

                developer.Tenants.ForEach(tenant =>
                {
                    string subdomainPattern = tenant.SubdomainPattern;

                    if (developer.Certificate != null && developer.Certificate.Length > 0 &&
                        string.IsNullOrEmpty(developer.CertificatePassword))
                    {
                        throw new Exception("Developer in tenant database has certificate set, but no certificate password is present");
                    }

                    var tenantInfo = new TenantInfoImpl()
                    {
                        DeveloperUuid = developer.Uuid,
                        DeveloperAuthority = developer.Authority,
                        DeveloperAudience = developer.Audience,
                        DeveloperCertificate = developer.Certificate,
                        CertificatePassword = developer.CertificatePassword,
                        DeveloperAuthCookieDomain = developer.AuthCookieDomain,
                        TenantUuid = tenant.Uuid,
                        Name = tenant.Name,
                        LogoUrl = tenant.LogoUrl,
                        ApiUrl = tenant.ApiUrl,
                        WebUrl = tenant.WebUrl
                    };

                    string[] subdomainPatternParts = subdomainPattern.Split(';');

                    subdomainPatternParts.ToList().ForEach(subdomainPatternPart =>
                    {
                        _tenantInfoMappings.Add(subdomainPatternPart, tenantInfo);                        
                    });

                    _tenantInfoMappingsByUuid.Add(tenantInfo.TenantUuid, tenantInfo);

                    TenantInfos.Add(tenantInfo);
                });
            }

            internal ITenantInfo LookupTenantBySubDomain(string subDomainLookup)
            {
                if (!_tenantInfoMappings.ContainsKey(subDomainLookup))
                    return null;

                var tenantInfo = _tenantInfoMappings[subDomainLookup];

                return tenantInfo;
            }

            internal ITenantInfo LookupTenantByUuid(long tenantUuid)
            {
                if (!_tenantInfoMappingsByUuid.ContainsKey(tenantUuid))
                    return null;

                return _tenantInfoMappingsByUuid[tenantUuid];
            }
        }

        public TenantDataProviderImpl(IServiceProvider serviceProvider, ILogger<TenantDataProviderImpl> logger)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                using (var dbContext = scope.ServiceProvider.GetRequiredService<SqlServerTenantDbContext>())
                {
                    IQueryable<DeveloperModel> query = dbContext.Developers;

                    query = query.Include(developerModel => developerModel.Tenants);

                    List<DeveloperModel> developers = query.ToList();

                    _developerMappings = new Dictionary<string, DeveloperWrapper>();
                    _developerMappingsByUuid = new Dictionary<long, DeveloperWrapper>();

                    Tenants = new List<ITenantInfo>();

                    developers.ForEach(developer =>
                    {
                        var developerWrapper = new DeveloperWrapper(developer);

                        _developerMappings.Add(developer.HostPattern, developerWrapper);
                        _developerMappingsByUuid.Add(developer.Uuid, developerWrapper);

                        Tenants.AddRange(developerWrapper.TenantInfos);                        
                    });
                }
            }

            _logger = logger;
        }

        public ITenantInfo LookupTenantByHost(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                _logger.LogError($"Host is empty for tenant parsing");

                return null;
            }
            
            string[] parts = host.Split('.');

            if (parts.Length == 2)
            {
                // prefix with www

                host = "www." + host;
            }

            parts = host.Split('.');

            if (parts.Length < 3)
            {
                _logger.LogError($"Host {host} does not have enough parts for tenant parsing");

                return null;
            }

            if (parts.Length > 3)
            {
                _logger.LogError($"Host {host} has too many parts for tenant parsing");

                return null;
            }

            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]) || string.IsNullOrEmpty(parts[2]))
            {
                _logger.LogError($"Host {host} has empty parts which is not allowed for tenant parsing");

                return null;
            }

            string subDomainLookup = parts[0];
            string hostLookup = parts[1] + "." + parts[2];

            if (!_developerMappings.ContainsKey(hostLookup))
            {
                _logger.LogError($"No developer found for host {host}");

                return null;
            }

            var developerWrapper = _developerMappings[hostLookup];
            if (developerWrapper == null)
            {
                _logger.LogError($"No developer found for host {host}");

                return null;
            }

            var tenantInfo = developerWrapper.LookupTenantBySubDomain(subDomainLookup);
            if (tenantInfo == null)
            {
                _logger.LogError($"No developer found for host {host}, developer {developerWrapper.Developer.Uuid} and sub domain lookup {subDomainLookup}");

                return null;
            }

            return tenantInfo;            
        }

        public ITenantInfo LookupTenantByUuid(long developerUuid, long tenantUuid)
        {
            if (!_developerMappingsByUuid.ContainsKey(developerUuid))
                return null;

            return _developerMappingsByUuid[developerUuid].LookupTenantByUuid(tenantUuid);
        }
    }
}