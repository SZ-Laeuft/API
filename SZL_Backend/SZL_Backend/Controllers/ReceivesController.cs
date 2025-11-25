using Microsoft.AspNetCore.Mvc;
using SZL_Backend.DTO;
using System.Collections.Generic;
using System.Linq;

namespace SZL_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceivesController : ControllerBase
    {
        private static List<ReceivesDTO> _receives = new List<ReceivesDTO>();

        // GET: api/receives
        [HttpGet]
        public ActionResult<IEnumerable<ReceivesDTO>> GetAll()
        {
            return Ok(_receives);
        }

        // GET: api/receives/{giftId}/{participateId}
        [HttpGet("{giftId}/{participateId}")]
        public ActionResult<ReceivesDTO> Get(int giftId, int participateId)
        {
            var receive = _receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (receive == null)
                return NotFound();
            return Ok(receive);
        }

        // POST: api/receives
        [HttpPost]
        public ActionResult<ReceivesDTO> Create([FromBody] ReceivesDTO dto)
        {
            if (_receives.Any(r => r.Giftid == dto.Giftid && r.Participateid == dto.Participateid))
                return Conflict("This record already exists.");

            _receives.Add(dto);
            return CreatedAtAction(nameof(Get), new { giftId = dto.Giftid, participateId = dto.Participateid }, dto);
        }

        // PUT: api/receives/{giftId}/{participateId}
        [HttpPut("{giftId}/{participateId}")]
        public IActionResult Update(int giftId, int participateId, [FromBody] ReceivesDTO dto)
        {
            var receive = _receives.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (receive == null)
                return NotFound();
            
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
                return NotFound();

            _receives.Remove(receive);
            return NoContent();
        }
    }
}
