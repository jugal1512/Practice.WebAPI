using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Practice.WebAPI.Utility
{
    public class SendEmail : ISendEmail
    {
        private readonly EmailSettings _emaillSettings;
        private readonly ILogger<SendEmail> _logger; 
        public SendEmail(IOptions<EmailSettings> emaillSettings, ILogger<SendEmail> logger)
        {
            _emaillSettings = emaillSettings.Value;
            _logger = logger;
        }
        public async Task SendMail(string toMail, string subject, string body)
        {
            try {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emaillSettings.FromName, _emaillSettings.FromAddress));
                email.To.Add(new MailboxAddress("", toMail));

                email.Subject = subject;
                var builder = new BodyBuilder();
                var mailBody = body + "<br/><br/>**This is an auto generated Email. Please do not reply to this message**<br/><br/>";
                builder.HtmlBody = mailBody;
                email.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emaillSettings.SmtpServer, _emaillSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emaillSettings.Username, _emaillSettings.Password);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
            }
            catch (SmtpCommandException ex) {
                _logger.LogError($"SMTP error: {ex.Message}, StatusCode: {ex.StatusCode}");
            }
            catch (SmtpProtocolException ex) {
                _logger.LogError($"SMTP Protocol error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occure while sending email: {ex.Message}");
            }
        }
    }
}