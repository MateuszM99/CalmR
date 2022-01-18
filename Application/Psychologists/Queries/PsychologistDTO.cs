using Domain.Entities;
using Domain.Enums;

namespace Application.Psychologists.Queries
{
    public class PsychologistDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public int CostPerHour { get; set; }
        public string Description { get; set; }
       
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
    }
}