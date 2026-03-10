using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SZL_Backend.Dto;


namespace SZL_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceivesController : ControllerBase
    {
        private static readonly List<ReceivesDto> Receives = new List<ReceivesDto>();

        // GET: api/receives/{giftId}/{participateId}
        [HttpGet("{giftId}/{participateId}")] 
        [SwaggerOperation 
            ( Summary = "Get receive by GiftId and ParticipateId", 
                Description = "Gets a specific receive record by its GiftId and ParticipateId." )] 
        [ProducesResponseType(typeof(ReceivesDto), 200)] 
        [ProducesResponseType(404)] 
        [ProducesResponseType(500)] 
        public IActionResult Get(int giftId, int participateId) 
        { try { var receive = Receives.FirstOrDefault(r => r.GiftId == giftId && r.ParticipateId == participateId); 
            if (receive == null) return NotFound();
            return Ok(receive); }
            catch { return StatusCode(500); } }
        
        // GET: api/receives
        [HttpGet]
        [SwaggerOperation (
            Summary = "Get all receives",
            Description = "Gets a list of all receives records"
            )]
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

        // GET: api/receives/gift/{giftId}
        [HttpGet("gift/{giftId}")]
        [SwaggerOperation(
            Summary = "Get receive by GiftId",
            Description = "Gets receive records identified by GiftId."
        )]
        [ProducesResponseType(typeof(ReceivesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetByGiftId(int giftId)
        {
            try
            {
                var receive = Receives.FirstOrDefault(r => r.GiftId == giftId);
                if (receive == null)
                    return NotFound();

                return Ok(receive);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        
        // GET: api/receives/participate/{participateId}
        [HttpGet("participate/{participateId}")]
        [SwaggerOperation(
            Summary = "Get receive by ParticipateId",
            Description = "Gets receive records identified by ParticipateId."
        )]
        [ProducesResponseType(typeof(ReceivesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetByParticipateId(int participateId)
        {
            try
            {
                var receive = Receives.FirstOrDefault(r => r.ParticipateId == participateId);
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
        [SwaggerOperation (
            Summary = "Create a new receive record",
            Description = "Creates a new receive record with the provided GiftId and ParticipateId."
            )]
        [ProducesResponseType(typeof(ReceivesDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult Create([FromBody] ReceivesDto dto)
        {
            if (dto.GiftId <= 0 || dto.ParticipateId <= 0)
                return BadRequest("GiftId and ParticipateId must be greater than zero");

            try
            {
                if (Receives.Any(r => r.GiftId == dto.GiftId && r.ParticipateId == dto.ParticipateId))
                    return Conflict("This record already exists.");

                Receives.Add(dto);
                return CreatedAtAction(nameof(Get), new { giftId = dto.GiftId, participateId = dto.ParticipateId }, dto);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        
        // PUT: api/receives/gift/{giftId}
        [HttpPut("gift/{giftId}")]
        [SwaggerOperation(
            Summary = "Update receive record by GiftId",
            Description = "Updates receive records identified by GiftId."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateByGiftId(int giftId, [FromBody] ReceivesDto dto)
        {
            if (giftId <= 0 || dto.GiftId <= 0)
                return BadRequest("GiftId must be greater than zero");

            try
            {
                var receive = Receives.FirstOrDefault(r => r.GiftId == giftId);
                if (receive == null)
                    return NotFound();

                receive.GiftId = dto.GiftId;
                receive.ParticipateId = dto.ParticipateId;

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
        // PUT: api/receives/participate/{participateId}
        [HttpPut("participate/{participateId}")]
        [SwaggerOperation(
            Summary = "Update receive record by ParticipateId",
            Description = "Updates receive records identified by ParticipateId."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateByParticipateId(int participateId, [FromBody] ReceivesDto dto)
        {
            if (participateId <= 0 || dto.ParticipateId <= 0)
                return BadRequest("ParticipateId must be greater than zero");

            try
            {
                var receive = Receives.FirstOrDefault(r => r.ParticipateId == participateId);
                if (receive == null)
                    return NotFound();

                receive.GiftId = dto.GiftId;
                receive.ParticipateId = dto.ParticipateId;

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
        // DELETE: api/receives/gift/{giftId}
        [HttpDelete("gift/{giftId}")]
        [SwaggerOperation(
            Summary = "Delete receive record by GiftId",
            Description = "Deletes receive records identified by GiftId."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteByGiftId(int giftId)
        {
            try
            {
                var receive = Receives.FirstOrDefault(r => r.GiftId == giftId);
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
        // DELETE: api/receives/participate/{participateId}
        [HttpDelete("participate/{participateId}")]
        [SwaggerOperation(
            Summary = "Delete receive record by ParticipateId",
            Description = "Deletes receive records identified by ParticipateId."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteByParticipateId(int participateId)
        {
            try
            {
                var receive = Receives.FirstOrDefault(r => r.ParticipateId == participateId);
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
