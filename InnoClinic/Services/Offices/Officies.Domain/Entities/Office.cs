using MongoDB.Bson;

public class Office
{
    public ObjectId Id { get; set; }
    public string Address => $"{City}, {Street}, {HouseNumber}, {OfficeNumber}";
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string OfficeNumber { get; set; }
    public string PhotoId { get; set; }
    public string RegistryPhoneNumber { get; set; }
    public bool IsActive { get; set; }
}
