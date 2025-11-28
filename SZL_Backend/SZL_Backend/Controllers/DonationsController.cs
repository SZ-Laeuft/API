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
    public class DonationsController(SZLDbContext context) : ControllerBase
    {
        // GET: api/donations
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all donations",
            Description = "Retrieves a list of all donations made."
        )]
        [ProducesResponseType(typeof(IEnumerable<DonationsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDonations()
        {
            try
            {
                var data = await context.Donations
                    .Select(d => new DonationsDto
                    {
                        Donationid = d.Donationid,
                        Participateid = d.Participateid,
                        Amount = d.Amount
                    })
                    .OrderBy(d => d.Donationid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/donations/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get donation by ID",
            Description = "Retrieves a specific donation by its unique ID."
        )]
        [ProducesResponseType(typeof(DonationsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDonation(int id)
        {
            try
            {
                var donation = await context.Donations
                    .Where(d => d.Donationid == id)
                    .Select(d => new DonationsDto
                    {
                        Donationid = d.Donationid,
                        Participateid = d.Participateid,
                        Amount = d.Amount
                    })
                    .FirstOrDefaultAsync();

                if (donation == null)
                    return NotFound();

                return Ok(donation);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/donations
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new donation",
            Description = "Creates a new donation record with the provided details."
        )]
        [ProducesResponseType(typeof(DonationsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostDonation(DonationsCreateDto dto)
        {
            if (dto.Amount <= 0)
                return BadRequest("Amount must be greater than zero");

            try
            {
                var donation = new Donation
                {
                    Participateid = dto.Participateid,
                    Amount = dto.Amount
                };

                context.Donations.Add(donation);
                await context.SaveChangesAsync();

                var result = new DonationsDto
                {
                    Donationid = donation.Donationid,
                    Participateid = donation.Participateid,
                    Amount = donation.Amount
                };

                return CreatedAtAction(nameof(GetDonation), new { id = donation.Donationid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/donations/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing donation",
            Description = "Updates the details of an existing donation identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutDonation(int id, DonationsCreateDto dto)
        {
            if (dto.Amount <= 0)
                return BadRequest("Amount must be greater than zero");

            try
            {
                var donation = await context.Donations.FindAsync(id);
                if (donation == null)
                    return NotFound();

                donation.Participateid = dto.Participateid;
                donation.Amount = dto.Amount;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/donations/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a donation",
            Description = "Deletes an existing donation identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            try
            {
                var donation = await context.Donations.FindAsync(id);
                if (donation == null)
                    return NotFound();

                context.Donations.Remove(donation);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
