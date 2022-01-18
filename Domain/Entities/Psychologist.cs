using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Psychologist : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public int CostPerHour { get; set; }
        public string Description { get; set; }
        
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string  UserId { get; set; }
        public virtual User User { get; set; }
    }
}