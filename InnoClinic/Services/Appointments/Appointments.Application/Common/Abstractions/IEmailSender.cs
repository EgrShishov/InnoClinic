using Microsoft.AspNetCore.Http;

public interface IEmailSender
{
    public Task SendEmailAsync(string userEmail, string subject, string body);
    public Task SendEmailWithAttachmentAsync(string userEmail, string subject, string body, IFormFile file);
}
