using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public DonationsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/donations
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DonationsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDonations()
        {
            try
            {
                var data = await _context.Donations
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
        [ProducesResponseType(typeof(DonationsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDonation(int id)
        {
            try
            {
                var donation = await _context.Donations
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

                _context.Donations.Add(donation);
                await _context.SaveChangesAsync();

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
                var donation = await _context.Donations.FindAsync(id);
                if (donation == null)
                    return NotFound();

                donation.Participateid = dto.Participateid;
                donation.Amount = dto.Amount;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/donations/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            try
            {
                var donation = await _context.Donations.FindAsync(id);
                if (donation == null)
                    return NotFound();

                _context.Donations.Remove(donation);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
