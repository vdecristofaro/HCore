﻿using Microsoft.Extensions.Configuration;
using ReinhardHolzner.Core.Emailing;
using ReinhardHolzner.Core.Emailing.Impl;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailingServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailing(this IServiceCollection services, IConfiguration configuration)
        {
            string addresses = configuration["Amqp:Addresses"];

            if (string.IsNullOrEmpty(addresses))
                throw new Exception("AMQP addresses are missing");

            string[] addressesSplit = addresses.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (!addressesSplit.Contains(EmailSenderConstants.Address))
                throw new Exception($"AMQP email sender requires the AMQP address '{EmailSenderConstants.Address}' to be defined");

            bool useAmqpEmailSender = configuration.GetValue<bool>("UseAmqpEmailSender");

            if (useAmqpEmailSender)
            {
                Console.WriteLine("Initializing AMQP email sender...");

                services.AddAmqp(configuration);

                DirectEmailSenderImpl.LoadEmailSenderConfiguration(EmailSenderConstants.EmptyConfigurationKeyDefaultKey, configuration);

                services.AddSingleton<IEmailSender, AMQPEmailSenderImpl>();

                Console.WriteLine("AMQP email sender initialized successfully");
            }
            else
            {
                Console.WriteLine("Initializing direct email sender...");

                DirectEmailSenderImpl.LoadEmailSenderConfiguration(EmailSenderConstants.EmptyConfigurationKeyDefaultKey, configuration);

                services.AddSingleton<IEmailSender, DirectEmailSenderImpl>();

                Console.WriteLine("Direct email sender initialized successfully");
            }

            return services;
        }        
    }
}