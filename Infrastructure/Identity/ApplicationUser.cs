using System.Collections.Generic;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<Conversation> Conversations { get; set; }
        public virtual List<UserContact> Contacts { get; set; }
    }
}