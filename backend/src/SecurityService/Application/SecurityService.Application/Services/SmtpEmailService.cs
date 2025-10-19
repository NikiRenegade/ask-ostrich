using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SecurityService.Application.Interfaces;

namespace SecurityService.Application.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _configuration["Smtp:FromName"], 
                _configuration["Smtp:From"]));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _configuration["Smtp:Host"], 
                int.Parse(_configuration["Smtp:Port"]), 
                MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _configuration["Smtp:Username"], 
                _configuration["Smtp:Password"]);
            
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
