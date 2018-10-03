﻿using Microsoft.Extensions.DependencyInjection;
using ReinhardHolzner.Core.Translations;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class TranslationsApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCoreTranslations(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var translationsProvider = scope.ServiceProvider.GetRequiredService<ITranslationsProvider>();

                string translation = translationsProvider.GetString("access_token_expired");

                if (string.IsNullOrEmpty(translation) || Equals(translation, "access_token_expired"))
                    throw new Exception("Translation can not be read");
            }

            return app;
        }        
    }
}