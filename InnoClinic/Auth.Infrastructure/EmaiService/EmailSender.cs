using Auth.Application.Common;
using MailKit.Net.Smtp;
using MimeKit;

namespace Auth.Infrastructure.EmaiService
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            SmtpClient smtp = new();
            smtp.Connect("smtp.yandex.ru", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("agendacalendar@yandex.ru", "qchwnwthulwrsirq");
/*            smtp.Connect(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.SslOnConnect); only for ASP .NET, change to settings
            smtp.Authenticate(emailSettings.ServiceEmail, emailSettings.Password);*/

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("agendacalendar@yandex.ru"));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;
            BodyBuilder messageBody = new BodyBuilder();
            messageBody.HtmlBody = body;

            email.Body = messageBody.ToMessageBody();

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
