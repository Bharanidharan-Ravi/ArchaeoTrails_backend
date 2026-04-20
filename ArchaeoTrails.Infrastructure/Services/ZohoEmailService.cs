using ArchaeoTrails.Application.Features.Contact;
using ArchaeoTrails.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ArchaeoTrails.Infrastructure.Services
{
    public class ZohoEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public ZohoEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendContactEmailAsync(ContactRequest request)
        {
            try
            {
                // Read settings from appsettings.json
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]!);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var appPassword = _configuration["EmailSettings:AppPassword"];

                var message = new MailMessage
                {
                    From = new MailAddress(senderEmail!, senderName),
                    Subject = $"New Heritage Trail Inquiry from {request.UserName}",
                    Body = $"Name: {request.UserName}\nEmail: {request.UserEmail}\n\nMessage:\n{request.Message}",
                    IsBodyHtml = false
                };

                // Send to yourself
                message.To.Add(senderEmail!);
                // Reply to the user
                message.ReplyToList.Add(new MailAddress(request.UserEmail));

                using var client = new SmtpClient(smtpServer, smtpPort);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(senderEmail, appPassword);

                await client.SendMailAsync(message);
                return true;
            }
            catch
            {
                // In a production app, you would log the exception here
                return false;
            }
        }
    }
}
