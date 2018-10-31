﻿using System.Collections.Generic;

namespace HCore.Tenants
{
    public interface ITenantDataProvider
    {
        List<ITenantInfo> Tenants { get; }
        ITenantInfo LookupTenant(string host);

        string GetTenantName(long developerUuid, long tenantUuid);
    }
}
