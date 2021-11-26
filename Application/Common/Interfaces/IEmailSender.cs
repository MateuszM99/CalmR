using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string to,string subject,string body);
    }
}