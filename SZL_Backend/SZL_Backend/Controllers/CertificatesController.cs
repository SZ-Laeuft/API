using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using SZL_Backend.Context;
using SZL_Backend.Dto;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController(SZLDbContext context) : ControllerBase
    {
        // GET: api/certificates/5/pdf
        [HttpGet("{eventId:int}/pdf")]
        [SwaggerOperation(
            Summary = "Generate certificates PDF for an event",
            Description = "Generates one combined PDF containing all certificates for the specified event."
        )]
        [Produces("application/pdf")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCertificatesPdf(int eventId)
        {
            try
            {
                var eventExists = await context.Events
                    .AnyAsync(e => e.Eventid == eventId);

                if (!eventExists)
                    return NotFound("Event not found");

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
                    return NotFound("No participants found for this event");

                var renderer = new CertificatePdfRenderer();
                var pdfBytes = renderer.Generate(certificateData);

                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}