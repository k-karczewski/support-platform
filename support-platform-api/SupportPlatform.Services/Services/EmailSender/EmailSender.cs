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

        /// <summary>
        /// Sends email with account confirmation link
        /// </summary>
        /// <param name="username">Recipent name</param>
        /// <param name="emailAddress">Email address of recipent</param>
        /// <param name="confirmationLink">Confirmation link</param>
        public async Task SendAccountConfirmation(string username, string emailAddress, string confirmationLink)
        {
            string messageContent = $"<h1>Witaj {username}</h1>" +
                $"<p>Aktywuj swoje konto klikając w poniższy link:</p>" +
                $"<a href={confirmationLink}>Aktywuj konto tutaj!</a>" +
                $"<p>Z poważaniem,</p>" +
                $"<p>Zespół SupportPlatform</p>";

            string subject = "Potwierdź swoje konto @ SupportPlatform";

            await SendEmail(subject, messageContent, username, emailAddress);
        }

        /// <summary>
        /// Sends email with information that status of report has been changed
        /// </summary>
        /// <param name="username">Recipent name</param>
        /// <param name="emailAddress">Email address of recipent</param>
        /// <param name="reportHeading">Heading / Title of modified report</param>
        /// <param name="reportStatus">Status of report after modification</param>
        public async Task SendStatusChanged(string username, string emailAddress, string reportHeading, string reportStatus)
        {
            string messageContent = $"<h1>Witaj {username}</h1>" +
                $"<p>Status Twojego zgłoszenia '{reportHeading}' zostal zmieniony na {reportStatus}</p>" +
                $"<p>Z poważaniem,</p>" +
                $"<p>Zespół SupportPlatform</p>";

            string subject = "Status Twojego zgłoszenia został zmieniony";

            await SendEmail(subject, messageContent, username, emailAddress);
        }

        /// <summary>
        /// Sends email with information that user's report has new response
        /// </summary>
        /// <param name="username">Recipent name</param>
        /// <param name="emailAddress">Email address of recipent</param>
        /// <param name="reportHeading">Heading / Title of report</param>
        public async Task SendEmployeeResponded(string username, string emailAddress, string reportHeading)
        {
            string messageContent = $"<h1>Witaj {username}</h1>" +
                $"<p>Jeden z naszych pracowników odpowiedział na Twoje zgłoszenie '{reportHeading}'</p>" +
                $"<p>Z poważaniem,</p>" +
                $"<p>Zespół SupportPlatform</p>";

            string subject = "Nowa odpowiedź na zgłoszenie";

            await SendEmail(subject, messageContent, username, emailAddress);
        }

        /// <summary>
        /// Sends email message
        /// </summary>
        /// <param name="subject">Subject of email</param>
        /// <param name="messageContent">Email content</param>
        /// <param name="username">Recipent name</param>
        /// <param name="emailAddress">Email address of recipent</param>
        private async Task SendEmail(string subject, string messageContent,string username, string emailAddress)
        {
            SendGridMessage message = new SendGridMessage
            {
                From = new EmailAddress("SupportPlatform@app.com", "no-reply"),
                Subject = subject,
                HtmlContent = messageContent,
            };

            var recipent = new EmailAddress("kkarczewski94@gmail.com", username);
            message.AddTo(recipent);

            SendGridClient client = new SendGridClient(_configuration.GetSection("SendGridKeys:DefaultKey").Value);

            await client.SendEmailAsync(message);
        }
    }
}
