using Microsoft.AspNetCore.Mvc;
using SZL_Backend.Dto;


namespace SZL_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceivesController : ControllerBase
    {
        private static readonly List<ReceivesDto> Receives = new List<ReceivesDto>();

        // GET: api/receives
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReceivesDto>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(Receives);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/receives/{giftId}/{participateId}
        [HttpGet("{giftId}/{participateId}")]
        [ProducesResponseType(typeof(ReceivesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int giftId, int participateId)
        {
            try
            {
                var receive = Receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
                if (receive == null)
                    return NotFound();

                return Ok(receive);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/receives
        [HttpPost]
        [ProducesResponseType(typeof(ReceivesDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult Create([FromBody] ReceivesDto dto)
        {
            if (dto.Giftid <= 0 || dto.Participateid <= 0)
                return BadRequest("GiftId and ParticipateId must be greater than zero");

            try
            {
                if (Receives.Any(r => r.Giftid == dto.Giftid && r.Participateid == dto.Participateid))
                    return Conflict("This record already exists.");

                Receives.Add(dto);
                return CreatedAtAction(nameof(Get), new { giftId = dto.Giftid, participateId = dto.Participateid }, dto);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/receives/{giftId}/{participateId}
        [HttpPut("{giftId}/{participateId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Update(int giftId, int participateId, [FromBody] ReceivesDto dto)
        {
            if (dto.Giftid <= 0 || dto.Participateid <= 0)
                return BadRequest("GiftId and ParticipateId must be greater than zero");

            try
            {
                var receive = Receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
                if (receive == null)
                    return NotFound();

                receive.Giftid = dto.Giftid;
                receive.Participateid = dto.Participateid;

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/receives/{giftId}/{participateId}
        [HttpDelete("{giftId}/{participateId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int giftId, int participateId)
        {
            try
            {
                var receive = Receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
                if (receive == null)
                    return NotFound();

                Receives.Remove(receive);
                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
