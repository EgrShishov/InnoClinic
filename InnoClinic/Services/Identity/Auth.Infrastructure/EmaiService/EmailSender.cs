using MailKit.Net.Smtp;
using MimeKit;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;
    public EmailSender(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Email));
        email.To.Add(new MailboxAddress("", to));
        email.Subject = subject;

        BodyBuilder messageBody = new BodyBuilder();    
        messageBody.HtmlBody = body;

        email.Body = messageBody.ToMessageBody();

        using var client = new SmtpClient();
        client.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
        client.Authenticate(_emailSettings.Email, _emailSettings.Password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }
}
