using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ApiControllerBase
    {
        private readonly SZLDbContext _context;

        public DonationsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/donations
        [HttpGet]
        public async Task<IActionResult> GetDonations()
        {
            var donations = await _context.Donations
                .Select(d => new DonationsDto
                {
                    Donationid = d.Donationid,
                    Participateid = d.Participateid,
                    Amount = d.Amount
                })
                .ToListAsync();

            return Success<IEnumerable<DonationsDto>>(donations);
        }

        // GET: api/donations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonation(int id)
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
                return Error<DonationsDto>("Donation not found", 404);

            return Success<DonationsDto>(donation);
        }

        // POST: api/donations
        [HttpPost]
        public async Task<IActionResult> PostDonation(DonationsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<DonationsDto>("Invalid donation data", 422);

            // Check if Participate exists
            if (!await _context.Participates.AnyAsync(p => p.Participateid == dto.Participateid))
                return Error<DonationsDto>("Participate not found", 404);

            // Optional: check for duplicate donation for the same participate
            if (await _context.Donations.AnyAsync(d => d.Participateid == dto.Participateid))
                return Error<DonationsDto>("Donation for this participate already exists", 409);

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

            return CreatedAtAction(
                nameof(GetDonation),
                new { id = donation.Donationid },
                new ApiResponse<DonationsDto> { Success = true, Data = result }
            );
        }

        // PUT: api/donations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonation(int id, DonationsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<object>("Invalid donation data", 422);

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return Error<object>("Donation not found", 404);

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
                return Error<object>("Donation not found", 404);

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
