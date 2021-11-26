using Domain.Common;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public string ZipCode { get; set; }
        
        
        public int? PsychologistId { get; set; }
        public virtual Psychologist Psychologist { get; set; }
    }
}