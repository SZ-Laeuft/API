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
                        DonationId = d.Donationid,
                        ParticipateId = d.Participateid,
                        Amount = d.Amount
                    })
                    .OrderBy(d => d.DonationId)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/donations/5
        [HttpGet("by-donation/{donationid}")]
        [SwaggerOperation(
            Summary = "Get donation by donationId",
            Description = "Retrieves a specific donation by its unique ID."
        )]
        [ProducesResponseType(typeof(DonationsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDonationDonationId(int id)
        {
            try
            {
                var donation = await context.Donations
                    .Where(d => d.Donationid == id)
                    .Select(d => new DonationsDto
                    {
                        DonationId = d.Donationid,
                        ParticipateId = d.Participateid,
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
        
        // GET: api/donations/5
        [HttpGet("by-participate/{participateid}")]
        [SwaggerOperation(
            Summary = "Get donation by participateId",
            Description = "Retrieves a specific donation by its unique ID."
        )]
        [ProducesResponseType(typeof(DonationsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDonationParticipateId(int participateid)
        {
            try
            {
                var donation = await context.Donations
                    .Where(d => d.Participateid == participateid)
                    .Select(d => new DonationsDto
                    {
                        DonationId = d.Donationid,
                        ParticipateId = d.Participateid,
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

        // Post: api/donations/
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create or Merge a donation",
            Description = "If a donation exists for this participant, add the amount. Otherwise, create a new record."
        )]
        [ProducesResponseType(typeof(DonationsDto), 201)]
        [ProducesResponseType(typeof(DonationsDto), 200)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostDonation(DonationsCreateDto dto)
        {
            if (dto.Amount <= 0)
                return BadRequest("Amount must be greater than zero");
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var existingDonation = await context.Donations
                    .FirstOrDefaultAsync(d => d.Participateid == dto.ParticipateId);

                if (existingDonation != null)
                {
                   
                    existingDonation.Amount += dto.Amount;

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync(); 

                    var updatedResult = new DonationsDto
                    {
                        DonationId = existingDonation.Donationid,
                        ParticipateId = existingDonation.Participateid,
                        Amount = existingDonation.Amount
                    };

                    
                    return Ok(updatedResult);
                }
                
                var donation = new Donation
                {
                    Participateid = dto.ParticipateId,
                    Amount = dto.Amount
                };

                context.Donations.Add(donation);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                var createdResult = new DonationsDto
                {
                    DonationId = donation.Donationid,
                    ParticipateId = donation.Participateid,
                    Amount = donation.Amount
                };

                return CreatedAtAction(nameof(GetDonationDonationId), new { id = donation.Donationid }, createdResult);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred processing the donation.");
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

                donation.Participateid = dto.ParticipateId;
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
