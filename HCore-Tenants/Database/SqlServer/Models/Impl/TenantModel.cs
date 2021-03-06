﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCore.Tenants.Database.SqlServer.Models.Impl
{
    public class TenantModel
    {
        public const int MaxSubdomainPatternLength = 255;
        public const int MaxUrlLength = 255;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Uuid { get; set; }

        public DeveloperModel Developer { get; set; }

        [StringLength(MaxSubdomainPatternLength)]
        public string SubdomainPattern { get; set; }

        [StringLength(MaxUrlLength)]
        public string BackendApiUrl { get; set; }

        [StringLength(MaxUrlLength)]
        public string FrontendApiUrl { get; set; }

        [StringLength(MaxUrlLength)]
        public string WebUrl { get; set; }

        [StringLength(DeveloperModel.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(DeveloperModel.MaxLogoUrlLength)]
        public string LogoSvgUrl { get; set; }

        [StringLength(DeveloperModel.MaxLogoUrlLength)]
        public string LogoPngUrl { get; set; }

        [StringLength(DeveloperModel.MaxLogoUrlLength)]
        public string IconIcoUrl { get; set; }

        public string StorageImplementation { get; set; }
        public string StorageConnectionString { get; set; }

        public int? PrimaryColor { get; set; }
        public int? SecondaryColor { get; set; }
        public int? TextOnPrimaryColor { get; set; }
        public int? TextOnSecondaryColor { get; set; }

        public string SupportEmail { get; set; }
        public string NoreplyEmail { get; set; }

        public string ProductName { get; set; }

        public string DefaultCulture { get; set; }

        public int Version { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}
