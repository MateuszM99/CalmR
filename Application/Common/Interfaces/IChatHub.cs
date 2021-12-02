using System.Threading.Tasks;
using Application.Messages.Queries;

namespace Application.Common.Interfaces
{
    public interface IChatHub
    {
        Task ReceiveMessage(MessageDTO message);
    }
}