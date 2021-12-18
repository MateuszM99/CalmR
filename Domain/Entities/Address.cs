using Domain.Common;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        
        
        public int? PsychologistId { get; set; }
        public virtual Psychologist Psychologist { get; set; }
    }
}