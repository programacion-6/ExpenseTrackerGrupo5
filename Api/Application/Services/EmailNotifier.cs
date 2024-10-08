using Api.Domain;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.IdentityModel.Tokens;

using MimeKit;
using MimeKit.Text;

public class EmailNotifier : INotifier<EmailContent>
{
    private string _smtpServer = string.Empty;
    private string _smtpUser = string.Empty;
    private string _smtpPassword = string.Empty;
    private int _smtpPort = 587;

    public EmailNotifier(IConfiguration configuration)
    {
        ConfigSmtpServer(configuration);
    }

    public async Task Notify(EmailContent emailContent)
    {
        var email = BuildEmail(emailContent);
        await SendEmailFromSmtpServer(email);
    }

    private MimeMessage BuildEmail(EmailContent emailContent)
    {
        if (emailContent.To.IsNullOrEmpty())
        {
            throw new EmailNotificationException("Recipient's email was not found");
        }

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_smtpUser));
        email.To.Add(MailboxAddress.Parse(emailContent.To));
        email.Subject = emailContent.Subject;
        email.Body = new TextPart(TextFormat.Plain)
        {
            Text = emailContent.Body
        };

        return email;
    }

    private async Task SendEmailFromSmtpServer(MimeMessage email)
    {
        using var smtp = new SmtpClient();
        smtp.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(_smtpUser, _smtpPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    private void ConfigSmtpServer(IConfiguration configuration)
    {
        _smtpServer = configuration.GetSection("SmtpHost").Value ?? string.Empty;
        _smtpUser = configuration.GetSection("SmtpUsername").Value ?? string.Empty;
        _smtpPassword = configuration.GetSection("SmtpPassword").Value ?? string.Empty;
        _smtpPort = Int32.Parse(configuration.GetSection("SmtpPort").Value ?? "587");

        var isInvalidSmtpConfiguration = _smtpServer.IsNullOrEmpty() ||
                                        _smtpUser.IsNullOrEmpty() ||
                                        _smtpPassword.IsNullOrEmpty() ||
                                        _smtpPort == 0;

        if (isInvalidSmtpConfiguration)
        {
            throw new EmailNotificationException("Invalid SMTP server credentials");
        }
    }

}
