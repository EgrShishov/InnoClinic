
namespace InnoClinic.Domain.Entities
{
    public class Receptionist : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int AccountId { get; set; }
        public int OfficeId { get; set; }
    }
}
