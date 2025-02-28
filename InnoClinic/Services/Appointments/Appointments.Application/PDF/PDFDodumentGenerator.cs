using Microsoft.AspNetCore.Http;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class PDFDodumentGenerator : IPDFDocumentGenerator
{
    public PDFDodumentGenerator()
    {
        Settings.License = LicenseType.Community;
    }

    public IFormFile GenerateAppointmentResults(GeneratePDFResultsRequest results)
    {
        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(10);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(14));

                page.Header()
                    .AlignCenter()
                    .Text("Appointment Result").SemiBold().FontColor(Colors.Grey.Darken4).FontSize(20);

                page.Content()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Field").SemiBold();
                            header.Cell().Element(CellStyle).Text("Value").SemiBold();

                            IContainer CellStyle(IContainer container)
                            {
                                return container.BorderBottom(1).BorderColor(Colors.Black).PaddingVertical(5);
                            }
                        });

                        table.Cell().Element(CellStyle).Text("Date: ");
                        table.Cell().Element(CellStyle).Text(results.Date.ToShortDateString());
                        
                        table.Cell().Element(CellStyle).Text("Patient full name: ");
                        table.Cell().Element(CellStyle).Text(results.PatientName);

                        table.Cell().Element(CellStyle).Text("Date of Birth:");
                        table.Cell().Element(CellStyle).Text(results.DateOfBirth.ToShortDateString());

                        table.Cell().Element(CellStyle).Text("Doctor Name:");
                        table.Cell().Element(CellStyle).Text(results.DoctorName);

                        table.Cell().Element(CellStyle).Text("Specialization:");
                        table.Cell().Element(CellStyle).Text(results.Specialization);

                        table.Cell().Element(CellStyle).Text("Service Name:");
                        table.Cell().Element(CellStyle).Text(results.ServiceName);

                        table.Cell().Element(CellStyle).Text("Complaints:");
                        table.Cell().Element(CellStyle).Text(results.Complaints);

                        table.Cell().Element(CellStyle).Text("Conclusion:");
                        table.Cell().Element(CellStyle).Text(results.Conclusion);

                        table.Cell().Element(CellStyle).Text("Recommendations:");
                        table.Cell().Element(CellStyle).Text(results.Recommendations);

                        IContainer CellStyle(IContainer container)
                        {
                            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).PaddingHorizontal(2);
                        }
                    });
            });
        }).GeneratePdf();

        var stream = new MemoryStream(pdfBytes);
        string fileName = $"{results.PatientName.Replace("/", "-")}_Results_{results.Date.ToString("yyyy-MM-dd")}.pdf";
        var file = new FormFile(stream, 0, stream.Length, "File", fileName);
        return file;
    }
}
