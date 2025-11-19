using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using SZL_Backend.Context;

namespace SZL_Backend.Controllers
{
    /*[Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        [HttpPost("Create_Team")]
        [SwaggerOperation(Summary = "Create a new Team", Description = "This endpoint creates a new Team.")]
        [SwaggerResponse(200, "Data inserted successfully.", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error.")]
        public async Task<IActionResult> InsertUserInformation([FromBody]   CompleteRoundVariables userInfo)
        {
            try
            {
                DateTime currentTime = DateTime.UtcNow;

                using (var connection = new NpgsqlConnection(ConnectionString.connectionstring))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO Rounds (UID, Scantime) VALUES (@UID, @Scantime);";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UID", userInfo.uid);
                        command.Parameters.AddWithValue("@Scantime", currentTime);

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return Ok($"Data inserted successfully. Rows affected: {rowsAffected}");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the lap duration based on the last two scan times for the given user ID.
        /// </summary>
        /// <param name="uid">The ID of the user to fetch the lap times for.</param>
        /// <returns>Returns the lap duration (time difference between the last two laps) or an error message.</returns>
        [HttpGet("LapDuration/{uid}")]
        [SwaggerOperation(
            Summary = "Get the lap duration based on the last two scan times",
            Description = "Fetches the last two scan times for the given user ID and calculates the lap duration."
        )]
        [SwaggerResponse(200, "Lap duration successfully calculated.", typeof(object))]
        [SwaggerResponse(404, "Not enough data to calculate lap duration.")]
        [SwaggerResponse(500, "Internal Server Error - Database issue or unexpected error.")]
        public async Task<IActionResult> GetLastLapById(decimal uid)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString.connectionstring))
                {
                    await connection.OpenAsync();

                    var query = @"
                WITH last_two_rounds AS (
                    SELECT
                        scantime,
                        ROW_NUMBER() OVER (ORDER BY scantime DESC) AS rn
                    FROM public.rounds
                    WHERE uid = @uid
                )
                SELECT
                    t1.scantime - t2.scantime AS laptime
                FROM last_two_rounds t1
                JOIN last_two_rounds t2 ON t1.rn = 1 AND t2.rn = 2;
            ";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@uid", uid);

                        var result = await command.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            if (result != null && result != DBNull.Value)
                            {
                                var laptime = (TimeSpan)result;
                                string formattedLapTime = laptime.ToString(@"hh\:mm\:ss");
                                return Ok(formattedLapTime);
                            }
                            return NotFound("Not enough rounds to calculate lap time.");
                        }
                        else
                        {
                            return NotFound("Not enough rounds to calculate lap time.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
*/
}