﻿using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReinhardHolzner.Core.AMQP;

namespace ReinhardHolzner.Core.Emailing.Impl
{
    public class AMQPMessageProcessorImpl : DirectEmailSenderImpl, IAMQPMessageProcessor
    {
        private readonly ILogger<AMQPMessageProcessorImpl> _logger;

        public AMQPMessageProcessorImpl(ILogger<AMQPMessageProcessorImpl> logger, IConfiguration configuration)
            : base(logger, configuration)
        {
            _logger = logger;
        }

        public virtual async Task<bool> ProcessMessageAsync(string address, string messageBodyJson)
        {
            if (!string.Equals(address, EmailSenderConstants.Address))
                return false;

            EmailSenderTask emailSenderTask = JsonConvert.DeserializeObject<EmailSenderTask>(messageBodyJson);

            await SendEmailAsync(emailSenderTask.ConfigurationKey, emailSenderTask.To, emailSenderTask.Cc, emailSenderTask.Bcc, emailSenderTask.Subject, emailSenderTask.HtmlMessage).ConfigureAwait(false);
            
            return true;
        }
    }
}
