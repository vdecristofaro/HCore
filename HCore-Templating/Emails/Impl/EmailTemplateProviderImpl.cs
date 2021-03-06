﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using HCore.Templating.Emails.ViewModels;
using HCore.Templating.Renderer;

namespace HCore.Templating.Emails.Impl
{
    public abstract class EmailTemplateProviderImpl : IEmailTemplateProvider
    {
        private readonly ITemplateRenderer _templateRenderer;

        public EmailTemplateProviderImpl(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        public abstract string GetConfirmAccountEmailView(CultureInfo cultureInfo);
        public abstract string GetConfirmAccountEmailSubject(CultureInfo cultureInfo);

        public abstract string GetForgotPasswordEmailView(CultureInfo cultureInfo);
        public abstract string GetForgotPasswordEmailSubject(CultureInfo cultureInfo);

        public async Task<EmailTemplate> GetConfirmAccountEmailAsync(ConfirmAccountEmailViewModel confirmAccountEmailViewModel, CultureInfo cultureInfo)
        {
            CultureInfo cultureInfoBackup = CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            string view = GetConfirmAccountEmailView(cultureInfo);
            if (string.IsNullOrEmpty(view))
                throw new Exception("Confirm account email view model path is empty");

            string subject = GetConfirmAccountEmailSubject(cultureInfo);
            if (string.IsNullOrEmpty(subject))
                throw new Exception("Confirm account email subject is empty");

            string body = await _templateRenderer.RenderViewAsync(view, confirmAccountEmailViewModel).ConfigureAwait(false);

            CultureInfo.CurrentCulture = cultureInfoBackup;
            CultureInfo.CurrentUICulture = cultureInfoBackup;

            return new EmailTemplate(subject, body);
        }

        public async Task<EmailTemplate> GetForgotPasswordEmailAsync(ForgotPasswordEmailViewModel forgotPasswordEmailViewModel, CultureInfo cultureInfo)
        {
            CultureInfo cultureInfoBackup = CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            string view = GetForgotPasswordEmailView(cultureInfo);
            if (string.IsNullOrEmpty(view))
                throw new Exception("Forgot password email view model path is empty");

            string subject = GetForgotPasswordEmailSubject(cultureInfo);
            if (string.IsNullOrEmpty(subject))
                throw new Exception("Forgot password email subject is empty");

            string body = await _templateRenderer.RenderViewAsync(view, forgotPasswordEmailViewModel).ConfigureAwait(false);

            CultureInfo.CurrentCulture = cultureInfoBackup;
            CultureInfo.CurrentUICulture = cultureInfoBackup;

            return new EmailTemplate(subject, body);
        }
    }
}
