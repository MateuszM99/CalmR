using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Options;
using AutoMapper.Configuration;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly GmailOptions _gmailOptions;
        public EmailSender(IOptions<GmailOptions> options)
        {
            _gmailOptions = options.Value;
        }
        public async Task SendEmailAsync(string to,string subject, string body)
        {
            var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential("matiapptest@gmail.com",_gmailOptions.Password),
                EnableSsl = true,
            });

            Email.DefaultSender = sender;

            var result = await Email
                .From("matiapptest@gmail.com")
                .To(to)
                .Subject(subject)
                .Body(body)
                .SendAsync();

            if (!result.Successful)
            {
                throw new ApiException("Something went wrong while sending email", "500");
            }
        }
    }
}