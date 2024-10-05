using System;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace UserInterface.Services
{
    /// <summary>
    /// Mail result class
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indicates operation is done or failed
        /// </summary>
        public bool Success    { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string? Message { get; set; }

        // Extra fields --------------------
        public int Id          { get; set; }
    }

    /// <summary>
    /// Interface for mail service
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends an email to the specified recipient
        /// </summary>
        /// <param name="to">The email address of the recipient</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="content">The body content of the email</param>
        /// <returns>A <see cref="Task{Result}"/> representing the result of the email sending operation</returns>
        Task<Result> SendMailAsync(string to, string subject, string content);
    }

    /// <summary>
    /// Mail configuration class
    /// </summary>
    public class MailConfigurations
    {
        /// <summary>
        /// User name (sender email)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password (sender email)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Sender email address
        /// </summary>
        public string From     { get; set; }

        /// <summary>
        /// Mail client port
        /// </summary>
        public int Port        { get; set; }

        /// <summary>
        /// Mail server. eg. smtp.gmail.com
        /// </summary>
        public string Server   { get; set; }
    }

    /// <summary>
    /// Mail service implementation
    /// </summary>
    public class MailService : IMailService
    {
        // Mail configurations
        public MailConfigurations Config { get; }

        public MailService(MailConfigurations mailconfig)
        {
            Config = mailconfig;
        }

        /// <summary>
        /// Sends an email to the specified recipient
        /// </summary>
        /// <param name="to">The email address of the recipient</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="content">The body content of the email</param>
        /// <returns>A <see cref="Task{Result}"/> representing the result of the email sending operation</returns>
        public async Task<Result> SendMailAsync(string to, string subject, string content)
        {
            // returns result
            return await SetupMailAsync(to, subject, content);
        }

        /// <summary>
        /// Sets up the email with the specified recipient, subject, and content, and prepares it for sending
        /// </summary>
        /// <param name="to">The email address of the recipient</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="content">The body content of the email</param>
        /// <returns>
        /// A <see cref="Task{Result}"/> representing the asynchronous operation, containing the result of the email setup process
        /// </returns>
        private async Task<Result> SetupMailAsync(string to, string subject, string content)
        {
            using (var smtp = new SmtpClient())
            {
                try
                {
                    // use mailkit for send mail
                    var mail     = new MimeMessage();
                    mail.From.Add(MailboxAddress.Parse(Config.From));
                    mail.To.Add(MailboxAddress.Parse(to));
                    mail.Subject = subject;
                    mail.Body    = new TextPart(TextFormat.Html) { Text = content };

                    await smtp.ConnectAsync(Config.Server, Config.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(Config.UserName, Config.Password);
                    await smtp.SendAsync(mail);

                    return new Result
                    {
                        Message = "Success",
                        Success = true
                    };

                }
                catch (Exception ex)
                {
                    return new Result
                    {
                        Message = ex.Message,
                        Success = false
                    };
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }
    }
}