﻿using Microsoft.Extensions.DependencyInjection;
using HCore.Emailing.Sender;

namespace Microsoft.AspNetCore.Builder
{
    public static class EmailingApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEmailing(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<IEmailSender>();

            return app;
        }        
    }
}