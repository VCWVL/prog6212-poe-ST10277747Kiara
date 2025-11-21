// CMCSP3.Reports/InvoiceDocument.cs
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CMCSP3.Models;
using System;

namespace CMCSP3.Reports
{
    public class InvoiceDocument : IDocument
    {
        public Claim Claim { get; set; }
        public User Lecturer { get; set; }

        public InvoiceDocument(Claim claim, User user)
        {
            Claim = claim;
            Lecturer = user;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(txt =>
                {
                    txt.Span("Page ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" of ").FontSize(10);
                    txt.TotalPages().FontSize(10);
                });
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem(2).Column(col =>
                {
                    col.Item().Text("CMCS University").Bold().FontSize(18).FontColor(Colors.Blue.Medium);
                    col.Item().Text("HR Department").FontSize(12);
                    col.Item().Text("hr@cmcs.com").FontSize(12);
                });

                row.RelativeItem(3).Column(col =>
                {
                    col.Item().Text("LECTURER PAYMENT INVOICE").Bold().FontSize(22).FontColor(Colors.Blue.Darken1);
                    col.Item().Text($"Invoice Date: {DateTime.Now:yyyy-MM-dd}").FontSize(11);
                    col.Item().Text($"Claim ID: {Claim.ClaimId}").FontSize(11);
                    col.Item().Text($"Lecturer ID: {Lecturer.LecturerId}").FontSize(11);
                    col.Item().Text($"Processed By HR: {Claim.ProcessedDate:yyyy-MM-dd}").FontSize(11);
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Spacing(20);

                // Lecturer Info
                column.Item().Text("BILL TO:").Bold().FontSize(12);
                column.Item().Text($"{Lecturer.FirstName} {Lecturer.LastName}").FontSize(12);
                column.Item().Text(Lecturer.Email).FontSize(12);

                // Claim Table
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(4); // Description
                        columns.RelativeColumn(1); // Hours
                        columns.RelativeColumn(1); // Rate
                        columns.RelativeColumn(1); // Amount
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Description").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Hours").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Rate").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Amount").Bold();
                    });

                    // Claim row
                    table.Cell().Padding(5).Text($"Lecturing Claim for period ending {Claim.ClaimDate:yyyy-MM-dd}");
                    table.Cell().Padding(5).AlignRight().Text($"{Claim.HoursWorked:F2}");
                    table.Cell().Padding(5).AlignRight().Text($"{Claim.HourlyRate:C2}"); // Use C2 for currency formatting
                    table.Cell().Padding(5).AlignRight().Text($"{Claim.ClaimAmount:C2}").Bold();
                });

                // Summary Box
                column.Item().Padding(10).Border(1).Column(col =>
                {
                    col.Item().Text($"Claim Status: {Claim.Status}").FontSize(12);
                    col.Item().Text($"TOTAL PAYABLE: {Claim.ClaimAmount:C2}").Bold().FontSize(16);
                });

                // Payment Notes
                column.Item().PaddingTop(10).Text("Payment Notes:").Bold().Underline().FontSize(12);
                column.Item().Text($"This payment corresponds to {Claim.HoursWorked:F2} approved hours at a fixed rate of {Claim.HourlyRate:C2} per hour.").Italic();
            });
        }
    }
}