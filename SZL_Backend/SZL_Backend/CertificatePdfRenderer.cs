using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend;

public class CertificatePdfRenderer
{
    public byte[] Generate(List<CertificatePdf> certificates)
    {
        certificates ??= new List<CertificatePdf>();

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontFamily("Consolas").FontSize(14));

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Gesponsort von: ");
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

    private static void ComposeCertificate(IContainer container, CertificatePdf item)
    {
        var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Certificates", "Logos", "Logo.png");
        var fullName = $"{item.FirstName} {item.LastName}".Trim();

        container
            .AlignCenter()
            .Column(column =>
            {
                column.Item()
                    .PaddingTop(-50)
                    .Width(450)
                    .Image(logoPath);
                
                column.Item()
                    .PaddingTop(-20)
                    .AlignCenter()
                    .Text(string.IsNullOrWhiteSpace(fullName) ? "Unbekannter Teilnehmer" : fullName)
                    .FontSize(28)
                    .Bold();
                
                column.Item()
                    .PaddingTop(10)
                    .AlignCenter()
                    .Text("belegte beim")
                    .FontSize(22);
                
                column.Item()
                    .PaddingTop(10)
                    .AlignCenter()
                    .Text("Charitylauf")
                    .FontSize(22);
                
                column.Item()
                    .AlignCenter()
                    .Text(item.EventName)
                    .FontSize(28)
                    .Bold();
                
                column.Item()
                    .PaddingTop(15)
                    .AlignCenter()
                    .Text($"den {item.Place}. Platz")
                    .FontSize(28);

                column.Item()
                    .PaddingTop(10)
                    .AlignCenter()
                    .Text("mit insgesamt")
                    .FontSize(22);

                column.Item()
                    .PaddingTop(5)
                    .AlignCenter()
                    .Text($"{item.RoundCount} Runden")
                    .FontSize(28)
                    .Bold();
            });
    }
}