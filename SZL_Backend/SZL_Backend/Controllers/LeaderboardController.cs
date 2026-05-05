using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using SZL_Backend.Context;
using SZL_Backend.Dto;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController(SZLDbContext context) : ControllerBase
    {
        [HttpGet("fastest-by-gender")]
        [SwaggerOperation(
            Summary = "Get the fastest five female and male participants",
            Description = "Retrieves the five fastest participants for female and male runners ordered by best time."
        )]
        [ProducesResponseType(typeof(LeaderboardByGenderDto), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFastestByGender()
        {
            try
            {
                var female = await GetFastestParticipantsByGender("female");
                var male = await GetFastestParticipantsByGender("male");

                return Ok(new LeaderboardByGenderDto
                {
                    Female = female,
                    Male = male
                });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("most-rounds-by-gender")]
        [SwaggerOperation(
            Summary = "Get the five participants with the most rounds by gender",
            Description = "Retrieves the five female and male participants with the most valid rounds. If round counts are equal, the participant who reached that count first is ranked higher."
        )]
        [ProducesResponseType(typeof(LeaderboardRoundsByGenderDto), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMostRoundsByGender()
        {
            try
            {
                var female = await GetMostRoundsParticipantsByGender("female");
                var male = await GetMostRoundsParticipantsByGender("male");

                return Ok(new LeaderboardRoundsByGenderDto
                {
                    Female = female,
                    Male = male
                });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        private async Task<List<LeaderboardEntryDto>> GetFastestParticipantsByGender(string gender)
        {
            var query = context.BestTimeViews
                .Join(
                    context.Participates,
                    bestTime => bestTime.ParticipateId,
                    participate => participate.Participateid,
                    (bestTime, participate) => new { bestTime, participate }
                )
                .Join(
                    context.Runners,
                    data => data.participate.Runnerid,
                    runner => (int?)runner.Runnerid,
                    (data, runner) => new { data.bestTime, data.participate, runner }
                );

            if (gender == "female")
            {
                query = query.Where(data =>
                    data.runner.Gender != null &&
                    (
                        EF.Functions.ILike(data.runner.Gender.Trim(), "w%") ||
                        EF.Functions.ILike(data.runner.Gender.Trim(), "f%")
                    )
                );
            }
            else if (gender == "male")
            {
                query = query.Where(data =>
                    data.runner.Gender != null &&
                    EF.Functions.ILike(data.runner.Gender.Trim(), "m%")
                );
            }

            return await query
                .OrderBy(data => data.bestTime.BestTime)
                .ThenBy(data => data.bestTime.ParticipateId)
                .Take(5)
                .Select(data => new LeaderboardEntryDto
                {
                    ParticipateId = data.bestTime.ParticipateId,
                    RunnerId = data.participate.Runnerid,
                    Firstname = data.runner.Firstname,
                    Lastname = data.runner.Lastname,
                    Gender = data.runner.Gender,
                    RoundId = data.bestTime.RoundId,
                    BestTime = data.bestTime.BestTime
                })
                .ToListAsync();
        }

        private async Task<List<LeaderboardRoundsEntryDto>> GetMostRoundsParticipantsByGender(string gender)
        {
            var query = context.Rounds
                .Where(round =>
                    round.IsValid == "true" &&
                    round.Roundtimestamp != null
                )
                .Join(
                    context.Participates,
                    round => round.Participateid,
                    participate => participate.Participateid,
                    (round, participate) => new { round, participate }
                )
                .Join(
                    context.Runners,
                    data => data.participate.Runnerid,
                    runner => (int?)runner.Runnerid,
                    (data, runner) => new { data.round, data.participate, runner }
                );

            if (gender == "female")
            {
                query = query.Where(data =>
                    data.runner.Gender != null &&
                    (
                        EF.Functions.ILike(data.runner.Gender.Trim(), "w%") ||
                        EF.Functions.ILike(data.runner.Gender.Trim(), "f%")
                    )
                );
            }
            else if (gender == "male")
            {
                query = query.Where(data =>
                    data.runner.Gender != null &&
                    EF.Functions.ILike(data.runner.Gender.Trim(), "m%")
                );
            }

            return await query
                .GroupBy(data => new
                {
                    data.participate.Participateid,
                    data.participate.Runnerid,
                    data.runner.Firstname,
                    data.runner.Lastname,
                    data.runner.Gender
                })
                .Select(group => new LeaderboardRoundsEntryDto
                {
                    ParticipateId = group.Key.Participateid,
                    RunnerId = group.Key.Runnerid,
                    Firstname = group.Key.Firstname,
                    Lastname = group.Key.Lastname,
                    Gender = group.Key.Gender,
                    RoundCount = group.Count(),
                    ReachedAt = group.Max(x => x.round.Roundtimestamp)
                })
                .OrderByDescending(entry => entry.RoundCount)
                .ThenBy(entry => entry.ReachedAt)
                .ThenBy(entry => entry.ParticipateId)
                .Take(5)
                .ToListAsync();
        }
    }
}