using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController(SZLDbContext context) : ControllerBase
    {
        [HttpGet("{eventId:int}/pdf")]
        [SwaggerOperation(
            Summary = "Generate certificates PDF for an event",
            Description = "Generates one combined PDF containing all certificates for the specified event."
        )]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCertificatesPdf(int eventId)
        {
            try
            {
                var eventExists = await context.Events
                    .AnyAsync(e => e.Eventid == eventId);

                if (!eventExists)
                    return NotFound(new
                    {
                        error = "EventNotFound",
                        message = $"Event with id {eventId} was not found."
                    });

                var certificateData = await context.Participates
                    .AsNoTracking()
                    .Where(p => p.Eventid == eventId)
                    .Select(p => new CertificateDataDto
                    {
                        ParticipateId = p.Participateid,
                        EventName = p.Event != null ? p.Event.Name ?? string.Empty : string.Empty,
                        FirstName = p.Runner != null ? p.Runner.Firstname ?? string.Empty : string.Empty,
                        LastName = p.Runner != null ? p.Runner.Lastname ?? string.Empty : string.Empty,
                        RoundCount = p.Rounds.Count()
                    })
                    .OrderByDescending(p => p.RoundCount)
                    .ThenBy(p => p.LastName)
                    .ThenBy(p => p.FirstName)
                    .ToListAsync();

                if (certificateData.Count == 0)
                    return NotFound(new
                    {
                        error = "ParticipantsNotFound",
                        message = $"No participants were found for event {eventId}."
                    });

                var rankedCertificates = certificateData
                    .Select((item, index) => new CertificatePdf
                    {
                        ParticipateId = item.ParticipateId,
                        EventName = item.EventName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        RoundCount = item.RoundCount,
                        Place = index + 1
                    })
                    .ToList();

                var renderer = new CertificatePdfRenderer();
                var pdfBytes = renderer.Generate(rankedCertificates);

                if (pdfBytes == null || pdfBytes.Length == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        error = "PdfGenerationFailed",
                        message = "PDF could not be generated."
                    });

                return File(
                    pdfBytes,
                    "application/pdf",
                    $"Urkunden_Event_{eventId}.pdf"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "PdfGenerationFailed",
                    message = "An unexpected error occurred while generating the PDF.",
                    details = ex.Message
                });
            }
        }
    }
}