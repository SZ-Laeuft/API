using Microsoft.AspNetCore.Mvc;
using SZL_Backend.DTO;
using System.Collections.Generic;
using System.Linq;

namespace SZL_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecievesController : ControllerBase
    {
        private static List<RecievesDTO> _recieves = new List<RecievesDTO>();

        // GET: api/recieves
        [HttpGet]
        public ActionResult<IEnumerable<RecievesDTO>> GetAll()
        {
            return Ok(_recieves);
        }

        // GET: api/recieves/{giftId}/{participateId}
        [HttpGet("{giftId}/{participateId}")]
        public ActionResult<RecievesDTO> Get(int giftId, int participateId)
        {
            var recieve = _recieves.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (recieve == null)
                return NotFound();
            return Ok(recieve);
        }

        // POST: api/recieves
        [HttpPost]
        public ActionResult<RecievesDTO> Create([FromBody] RecievesDTO dto)
        {
            if (_recieves.Any(r => r.Giftid == dto.Giftid && r.Participateid == dto.Participateid))
                return Conflict("This record already exists.");

            _recieves.Add(dto);
            return CreatedAtAction(nameof(Get), new { giftId = dto.Giftid, participateId = dto.Participateid }, dto);
        }

        // PUT: api/recieves/{giftId}/{participateId}
        [HttpPut("{giftId}/{participateId}")]
        public IActionResult Update(int giftId, int participateId, [FromBody] RecievesDTO dto)
        {
            var recieve = _recieves.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (recieve == null)
                return NotFound();
            
            recieve.Giftid = dto.Giftid;
            recieve.Participateid = dto.Participateid;

            return NoContent();
        }

        // DELETE: api/recieves/{giftId}/{participateId}
        [HttpDelete("{giftId}/{participateId}")]
        public IActionResult Delete(int giftId, int participateId)
        {
            var recieve = _recieves.FirstOrDefault(r => r.Giftid == giftId && r.Participateid == participateId);
            if (recieve == null)
                return NotFound();

            _recieves.Remove(recieve);
            return NoContent();
        }
    }
}
