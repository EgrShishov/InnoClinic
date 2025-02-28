using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    public EmailSender(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendEmailAsync(string userEmail, string subject, string body)
    {
        var email = new MimeMessage();
       
        MailboxAddress emailFrom = new MailboxAddress(_settings.Name, _settings.EmailId);
        email.From.Add(emailFrom);

        MailboxAddress emailTo = new MailboxAddress("", userEmail);
        email.To.Add(emailTo);

        email.Subject = subject;
        BodyBuilder messageBody = new BodyBuilder();
        messageBody.HtmlBody = body;
        email.Body = messageBody.ToMessageBody();

        using var client = new SmtpClient();
        client.Connect(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
        client.Authenticate(_settings.Email, _settings.Password);

        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }

    public async Task SendEmailWithAttachmentAsync(string userEmail, string subject, string body, IFormFile file)
    {
        var email = new MimeMessage();

        MailboxAddress emailTo = new MailboxAddress("", userEmail);
        email.To.Add(emailTo);

        email.Subject = subject;
        BodyBuilder messageBody = new BodyBuilder();
        messageBody.HtmlBody = body;

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        messageBody.Attachments.Add(file.FileName, stream.ToArray());

        email.Body = messageBody.ToMessageBody();

        using var client = new SmtpClient();
        client.Connect(_settings.Host, _settings.Port, _settings.UseSSL);
        client.Authenticate(_settings.Email, _settings.Password);

        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }
}
