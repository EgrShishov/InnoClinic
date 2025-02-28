public static class EmailTemplates
{
    public class AppointmentNotificationEmailTemplate
    {
        public string PatientsFullName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string ServiceName { get; set; }
        public string DoctorsFullName { get; set; }

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
                        <h2>Appointment Reminder</h2>
                    </div>
                    <div class='details'>
                        <p>Dear {PatientsFullName},</p>
                        <p>This is a reminder for your upcoming appointment in {ServiceName}.</p>
                        <p><strong>Appointment Date:</strong> {AppointmentDate}, appointment time: {AppointmentTime}</p>
                        <p><strong>Doctor:</strong> {DoctorsFullName}</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        public string GetContent()
        {
            return GetTemplate()
                .Replace("{PatientsFullName}", PatientsFullName)
                .Replace("{DoctorsFullName}", DoctorsFullName)
                .Replace("{ServiceName}", ServiceName)
                .Replace("{AppointmentDate}", AppointmentDate.ToString("yyyy-MM-dd"))
                .Replace("{AppointmentTime}", AppointmentTime.ToString("mm:ss"));
        }
    }

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

    public class NewAppointmentResultsEmailTemplate
    {
        public string PatientsFullName { get; set; }
        public string DoctorsFullName { get; set; }
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
                        <h2>Your results:</h2>
                    </div>
                    <div class='details'>
                        <p>Dear {PatientsFullName},</p>
                        <p><strong>Appointment Date:</strong> {AppointmentDate}</p>
                        <p><strong>Doctor:</strong> {DoctorsFullName}</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        public string GetContent()
        {
            return GetTemplate()
                .Replace("{PatientsFullName}", PatientsFullName)
                .Replace("{DoctorsFullName}", DoctorsFullName)
                .Replace("{AppointmentDate}", AppointmentDate.ToString("yyyy-MM-dd"));
        }
    }

}
