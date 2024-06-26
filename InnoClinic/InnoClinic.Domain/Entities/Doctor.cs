
namespace InnoClinic.Domain.Entities
{
    public class Doctor : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int SpecializationId { get; set; }
        public int AccountId { get; set; }
        public int OfficeId { get; set; }
        public int CareerStartYear { get; set; }
        public ProfileStatus Status { get; set; }
    }
}
