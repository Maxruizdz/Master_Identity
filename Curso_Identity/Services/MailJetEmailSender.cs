using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;
using System;

namespace Curso_Identity.Services
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public OpcionesMailJet _opcionesMailJet;
        public MailJetEmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async  Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            _opcionesMailJet= _config.GetSection("MailJet").Get<OpcionesMailJet>();
            MailjetClient client = new MailjetClient(_opcionesMailJet.ApiKey, _opcionesMailJet.SecretpKey )
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      { 
       "From",
       new JObject {
        {"Email", "maxruizdz@hotmail.com"},
        {"Name", "Maximiliano"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Maximiliano"
         }
        }
       }
      }, {
       "Subject",
      subject
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
      htmlMessage
      }, 
     }
             });
           await client.PostAsync(request);
        }
    }
}

        
    

