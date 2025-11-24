using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.DTO;
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
        public async Task<ActionResult<IEnumerable<DonationsDTO>>> GetDonations()
        {
            return await _context.Donations
                .Select(d => new DonationsDTO
                {
                    Donationid = d.Donationid,
                    Participateid = d.Participateid,
                    Amount = d.Amount
                })
                .ToListAsync();
        }

        // GET: api/donations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonationsDTO>> GetDonation(int id)
        {
            var donation = await _context.Donations
                .Where(d => d.Donationid == id)
                .Select(d => new DonationsDTO
                {
                    Donationid = d.Donationid,
                    Participateid = d.Participateid,
                    Amount = d.Amount
                })
                .FirstOrDefaultAsync();

            if (donation == null)
                return NotFound();

            return donation;
        }

        // POST: api/donations
        [HttpPost]
        public async Task<ActionResult<DonationsDTO>> PostDonation(DonationsCreateDTO dto)
        {
            var donation = new Donation
            {
                Participateid = dto.Participateid,
                Amount = dto.Amount
            };

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            var result = new DonationsDTO
            {
                Donationid = donation.Donationid,
                Participateid = donation.Participateid,
                Amount = donation.Amount
            };

            return CreatedAtAction(nameof(GetDonation), new { id = donation.Donationid }, result);
        }

        // PUT: api/donations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonation(int id, DonationsCreateDTO dto)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();

            donation.Participateid = dto.Participateid;
            donation.Amount = dto.Amount;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/donations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
