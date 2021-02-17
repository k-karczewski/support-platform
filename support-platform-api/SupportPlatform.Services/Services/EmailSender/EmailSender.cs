using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SupportPlatform.Database;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public class EmailSender : IEmailSender
    { 
        private IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {
            _configuration = null;
        }

        public async Task SendAccountConfirmation(UserEntity receiver, string confirmationLink)
        {
            string messageContent = $"<h1>Witaj {receiver.UserName}</h1>" +
                $"<p>Aktywuj swoje konto klikając w poniższy link:</p>" +
                $"<a href={confirmationLink}>Aktywuj konto tutaj!</a>" +
                $"<p>Z poważaniem,</p>" +
                $"<p>Zespół SupportPlatform</p>";

            SendGridMessage message = new SendGridMessage
            {
                From = new EmailAddress("SupportPlatform@app.com", "no-reply"),
                Subject = "Potwierdź swoje konto @ SupportPlatform",
                HtmlContent = messageContent,
            };

            var recipent = new EmailAddress("kkarczewski94@gmail.com", receiver.UserName);
            message.AddTo(recipent);

            SendGridClient client = new SendGridClient(_configuration.GetSection("SendGridKeys:DefaultKey").Value);

            await client.SendEmailAsync(message);
        }
    }
}
