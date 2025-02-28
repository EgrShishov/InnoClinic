public class AppointmentResultsChangedEmailTemplate
{
    public string PatientsFullName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string GetTemplate()
    {
        return @"
            <html>
            <head>
                <style>
                    body { font-family: Arial, sans-serif; }
                    .content { max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ccc; }
                    .header { background-color: #f4f4f4; padding: 10px; text-align: center; }
                    .details { margin-top: 20px; }
                </style>
            </head>
            <body>
                <div class='content'>
                    <div class='header'>
                        <h2>Your appointments results changed:</h2>
                    </div>
                    <div class='details'>
                        <p>Dear {PatientsFullName},</p>
                        <p><strong>Appointment Date:</strong> {AppointmentDate}</p>
                    </div>
                </div>
            </body>
            </html>";
    }

    public string GetContent()
    {
        return GetTemplate()
            .Replace("{PatientsFullName}", PatientsFullName)
            .Replace("{AppointmentDate}", AppointmentDate.ToString("yyyy-MM-dd"));
    }
}
