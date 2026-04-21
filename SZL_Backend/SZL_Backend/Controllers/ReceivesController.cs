using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceivesController(SZLDbContext context) : ControllerBase
    {
        // GET: api/receives/{giftId}/{participateId}
        [HttpGet("{giftId:int}/{participateId:int}")]
        [SwaggerOperation(
            Summary = "Get receive by GiftId and ParticipateId",
            Description = "Gets a specific receive record by its GiftId and ParticipateId.")]
        [ProducesResponseType(typeof(ReceivesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get(int giftId, int participateId)
        {
            try
            {
                var receive = await context.Receives
                    .Where(r => r.Giftid == giftId && r.Participateid == participateId)
                    .Select(ToDto())
                    .FirstOrDefaultAsync();

                return receive is null ? NotFound() : Ok(receive);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/receives
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all receives",
            Description = "Gets a list of all receives records")]
        [ProducesResponseType(typeof(IEnumerable<ReceivesDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await context.Receives
                    .Select(ToDto())
                    .OrderBy(r => r.GiftId)
                    .ThenBy(r => r.ParticipateId)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/receives/gift/{giftId}
        [HttpGet("gift/{giftId:int}")]
        [SwaggerOperation(
            Summary = "Get receive by GiftId",
            Description = "Gets receive records identified by GiftId.")]
        [ProducesResponseType(typeof(IEnumerable<ReceivesDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByGiftId(int giftId)
        {
            try
            {
                var data = await context.Receives
                    .Where(r => r.Giftid == giftId)
                    .Select(ToDto())
                    .OrderBy(r => r.ParticipateId)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/receives/participate/{participateId}
        [HttpGet("participate/{participateId:int}")]
        [SwaggerOperation(
            Summary = "Get receive by ParticipateId",
            Description = "Gets receive records identified by ParticipateId.")]
        [ProducesResponseType(typeof(IEnumerable<ReceivesDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByParticipateId(int participateId)
        {
            try
            {
                var data = await context.Receives
                    .Where(r => r.Participateid == participateId)
                    .Select(ToDto())
                    .OrderBy(r => r.GiftId)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/receives
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new receive record",
            Description = "Creates a new receive record with the provided GiftId and ParticipateId.")]
        [ProducesResponseType(typeof(ReceivesDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] ReceivesDto dto)
        {
            if (dto.GiftId <= 0 || dto.ParticipateId <= 0)
                return BadRequest("GiftId and ParticipateId must be greater than zero.");

            try
            {
                var exists = await context.Receives.AnyAsync(r =>
                    r.Giftid == dto.GiftId && r.Participateid == dto.ParticipateId);
                if (exists)
                    return Conflict("This record already exists.");

                var receive = new Receive
                {
                    Giftid = dto.GiftId,
                    Participateid = dto.ParticipateId,
                    Iscollected = dto.IsCollected
                };

                context.Receives.Add(receive);
                await context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get),
                    new { giftId = dto.GiftId, participateId = dto.ParticipateId }, dto);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/receives/{giftId}/{participateId}
        [HttpPut("{giftId:int}/{participateId:int}")]
        [SwaggerOperation(
            Summary = "Update receive record by GiftId and ParticipateId",
            Description = "Updates a specific receive record identified by GiftId and ParticipateId.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int giftId, int participateId, [FromBody] ReceivesDto dto)
        {
            if (giftId <= 0 || participateId <= 0)
                return BadRequest("GiftId and ParticipateId must be greater than zero.");
            if (dto.GiftId <= 0 || dto.ParticipateId <= 0)
                return BadRequest("Dto GiftId and ParticipateId must be greater than zero.");

            try
            {
                var receive = await context.Receives
                    .FirstOrDefaultAsync(r => r.Giftid == giftId && r.Participateid == participateId);
                if (receive == null)
                    return NotFound();

                var duplicateExists = await context.Receives.AnyAsync(r =>
                    r.Giftid == dto.GiftId &&
                    r.Participateid == dto.ParticipateId &&
                    !(r.Giftid == giftId && r.Participateid == participateId));
                if (duplicateExists)
                    return Conflict("A record with the new GiftId and ParticipateId already exists.");

                receive.Giftid = dto.GiftId;
                receive.Participateid = dto.ParticipateId;
                receive.Iscollected = dto.IsCollected;

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/receives/{giftId}/{participateId}
        [HttpDelete("{giftId:int}/{participateId:int}")]
        [SwaggerOperation(
            Summary = "Delete receive record by GiftId and ParticipateId",
            Description = "Deletes one receive record identified by GiftId and ParticipateId.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int giftId, int participateId)
        {
            try
            {
                var receive = await context.Receives
                    .FirstOrDefaultAsync(r => r.Giftid == giftId && r.Participateid == participateId);
                if (receive == null)
                    return NotFound();

                context.Receives.Remove(receive);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        private static Expression<Func<Receive, ReceivesDto>> ToDto()
        {
            return r => new ReceivesDto
            {
                GiftId = r.Giftid,
                ParticipateId = r.Participateid,
                IsCollected = r.Iscollected
            };
        }
    }
}
