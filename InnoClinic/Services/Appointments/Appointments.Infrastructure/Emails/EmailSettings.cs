public class EmailSettings
{
    public static string SectionName = "EmailSettings";
    public string EmailId {  get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }    
    public bool UseSSL { get; set; }
}
