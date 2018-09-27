﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReinhardHolzner.Core.Templating.Emails.ViewModels;
using ReinhardHolzner.Core.Templating.Generic;

namespace ReinhardHolzner.Core.Templating.Emails.Impl
{
    public abstract class EmailTemplateProviderImpl : IEmailTemplateProvider
    {
        private readonly ITemplateRenderer _templateRenderer;

        public EmailTemplateProviderImpl(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        public abstract string GetConfirmAccountEmailView();
        public abstract string GetConfirmAccountEmailSubject();
        
        public async Task<EmailTemplate> GetConfirmAccountEmailAsync(ConfirmAccountEmailViewModel confirmAccountEmailViewModel)
        {
            string view = GetConfirmAccountEmailView();
            if (string.IsNullOrEmpty(view))
                throw new Exception("Confirm account email view model path is empty");

            string subject = GetConfirmAccountEmailSubject();
            if (string.IsNullOrEmpty(subject))
                throw new Exception("Confirm account email subject is empty");

            string body = await _templateRenderer.RenderViewAsync(view, confirmAccountEmailViewModel).ConfigureAwait(false);

            return new EmailTemplate(subject, body);
        }
    }
}