using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public virtual List<UserContact> UserContacts { get; set; }
    }
}