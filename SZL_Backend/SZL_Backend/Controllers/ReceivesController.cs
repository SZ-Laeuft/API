using Microsoft.AspNetCore.Mvc;
using SZL_Backend.Dto;


namespace SZL_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceivesController : ApiControllerBase
    {
        private static List<ReceivesDto> _receives = new List<ReceivesDto>();

        // GET: api/receives
        [HttpGet]
        public IActionResult GetAll()
        {
            return Success(_receives);
        }

        // GET: api/receives/{giftId}/{participateId}
        [HttpGet("{giftId}/{participateId}")]
        public IActionResult Get(int giftId, int participateId)
        {
            var receive = _receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (receive == null)
                return Error<ReceivesDto>("Record not found", 404);

            return Success(receive);
        }

        // POST: api/receives
        [HttpPost]
        public IActionResult Create([FromBody] ReceivesDto dto)
        {
            if (_receives.Any(r => r.Giftid == dto.Giftid && r.Participateid == dto.Participateid))
                return Error<ReceivesDto>("This record already exists", 409);

            _receives.Add(dto);

            return CreatedAtAction(
                nameof(Get),
                new { giftId = dto.Giftid, participateId = dto.Participateid },
                new ApiResponse<ReceivesDto> { Success = true, Data = dto }
            );
        }

        // PUT: api/receives/{giftId}/{participateId}
        [HttpPut("{giftId}/{participateId}")]
        public IActionResult Update(int giftId, int participateId, [FromBody] ReceivesDto dto)
        {
            var receive = _receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (receive == null)
                return Error<object>("Record not found", 404);

            receive.Giftid = dto.Giftid;
            receive.Participateid = dto.Participateid;

            return NoContent();
        }

        // DELETE: api/receives/{giftId}/{participateId}
        [HttpDelete("{giftId}/{participateId}")]
        public IActionResult Delete(int giftId, int participateId)
        {
            var receive = _receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (receive == null)
                return Error<object>("Record not found", 404);

            _receives.Remove(receive);

            return NoContent();
        }
    }
}
