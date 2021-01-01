using System.Threading.Tasks;
using EmployeeMgt.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmployeeMgt.Services
{
    public class MessageService : IMessageService
    {
        private readonly EmailConfig emailConfig;

        public MessageService(IOptions<EmailConfig> emailConfig)
        {
            this.emailConfig = emailConfig.Value;
        }
        public async Task SendEmailAsync
        (   
            string userName, 
            string fromEmailAddress, 
            string toName, 
            string toEmailAddress, 
            string subject, 
            string message 
           /*  params Attachment[] attachments */
        )
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress( emailConfig.FromEmailAddress));
            mimeMessage.To.Add(new MailboxAddress(toName, toEmailAddress));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = message
            };

            

        /*  foreach(var attachment in attachments)
            {
                using(var stream = await attachment.ContentToStreamAsyn())
                {
                    body.Attachments.Add(attachment.FileName, stream);
                }
            } */

            using(var client = new SmtpClient())
            {
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.ConnectAsync(emailConfig.Host , emailConfig.Port, false);
                await client.AuthenticateAsync(emailConfig.UserName , emailConfig.Password);

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

            }

        }
    }
}