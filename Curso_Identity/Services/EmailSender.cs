using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;
using System;
using MimeKit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Curso_Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var correo_enviar = new MimeMessage();
            correo_enviar.From.Add(MailboxAddress.Parse(_config.GetSection("mail:Correo").Value));
            correo_enviar.To.Add(MailboxAddress.Parse(email));
            correo_enviar.Subject = subject;
            correo_enviar.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("mail:Correo").Value, _config.GetSection("mail:key").Value) ;
            smtp.Send(correo_enviar); 
            smtp.Disconnect(true);

        }



    }
}

        
    

