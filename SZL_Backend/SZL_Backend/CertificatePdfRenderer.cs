using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SZL_Backend.Dto;

namespace SZL_Backend;

public class CertificatePdfRenderer
{
    public byte[] Generate(List<CertificateDataDto> certificates)
    {
        certificates ??= new List<CertificateDataDto>();

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(14));

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Seite ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });

                page.Content()
                    .Column(column =>
                    {
                        if (certificates.Count == 0)
                        {
                            column.Item()
                                .AlignCenter()
                                .Text("Keine Teilnehmerdaten vorhanden.")
                                .FontSize(20)
                                .Bold();

                            return;
                        }

                        for (int i = 0; i < certificates.Count; i++)
                        {
                            if (i > 0)
                                column.Item().PageBreak();

                            var item = certificates[i];
                            column.Item().Element(container => ComposeCertificate(container, item));
                        }
                    });
            });
        }).GeneratePdf();
    }

    private static void ComposeCertificate(IContainer container, CertificateDataDto item)
    {
        var fullName = $"{item.FirstName} {item.LastName}".Trim();

        container
            .Border(1)
            .Padding(30)
            .Column(column =>
            {
                column.Spacing(12);

                column.Item()
                    .AlignCenter()
                    .Text("TEST-URKUNDE")
                    .FontSize(28)
                    .Bold();

                column.Item()
                    .AlignCenter()
                    .Text(item.EventName)
                    .FontSize(18);

                column.Item()
                    .PaddingTop(20)
                    .AlignCenter()
                    .Text("Verliehen an")
                    .FontSize(14);

                column.Item()
                    .AlignCenter()
                    .Text(string.IsNullOrWhiteSpace(fullName) ? "Unbekannter Teilnehmer" : fullName)
                    .FontSize(24)
                    .Bold();

                column.Item()
                    .PaddingTop(15)
                    .AlignCenter()
                    .Text($"Runden: {item.RoundCount}")
                    .FontSize(16);

                column.Item()
                    .AlignCenter()
                    .Text("Platzierung: TEST")
                    .FontSize(16);

                column.Item()
                    .PaddingTop(30)
                    .AlignCenter()
                    .Text("Nur Testlayout - Vorlage folgt später")
                    .FontSize(12);
            });
    }
}