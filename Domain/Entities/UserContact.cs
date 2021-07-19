using Domain.Common;

namespace Domain.Entities
{
    public class UserContact
    {
        public string UserId { get; set; }
        public int ContactId { get; set; }
        
        public virtual Contact Contact { get; set; }
    }
}