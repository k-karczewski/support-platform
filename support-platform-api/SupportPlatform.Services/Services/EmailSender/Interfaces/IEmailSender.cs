using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends email with account confirmation link
        /// </summary>
        /// <param name="username">Recipent name</param>
        /// <param name="emailAddress">Email address of recipent</param>
        /// <param name="confirmationLink">Confirmation link</param>
        Task SendAccountConfirmation(string username, string emailAddress, string confirmationLink);

        /// <summary>
        /// Sends email with information that status of report has been changed
        /// </summary>
        /// <param name="username">Recipent name</param>
        /// <param name="emailAddress">Email address of recipent</param>
        /// <param name="reportHeading">Heading / Title of modified report</param>
        /// <param name="reportStatus">Status of report after modification</param>
        Task SendStatusChanged(string username, string emailAddress, string reportHeading, string reportStatus);

        /// <summary>
        /// Sends email with information that user's report has new response
        /// </summary>
        /// <param name="username">Recipent name</param>
        /// <param name="emailAddress">Email address of recipent</param>
        /// <param name="reportHeading">Heading / Title of report</param>
        Task SendEmployeeResponded(string username, string emailAddress, string reportHeading);
    }
}
