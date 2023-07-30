using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Security.Encryption;
using Volo.Abp.TextTemplating;

namespace Diganto.Email
{
    public class EmailService : ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;

        public EmailService(IEmailSender emailSender, ITemplateRenderer templateRenderer)
        {
            _emailSender = emailSender;
            _templateRenderer = templateRenderer;
        }
        

        public async Task SendEmailAsync(string email)
        {

            var body = await _templateRenderer.RenderAsync(
           StandardEmailTemplates.Message,
               new
               {
                   message = "This is email body..."
               }
           );
            try
            {
                await _emailSender.SendAsync(
                    email,
                    "Email subject",
                    body
                );
            }
            catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            
        }
    }
}