﻿namespace HCore.Templating.Emails
{
    public class EmailTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailTemplate(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
    }
}
