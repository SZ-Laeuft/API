using Microsoft.AspNetCore.Mvc;
using SZL_Backend.Dto;

public interface IApiResponse
{
    IActionResult Success<T>(T data);
    IActionResult Error<T>(string message, int statusCode);
}

public abstract class ApiControllerBase : ControllerBase, IApiResponse
{
    public IActionResult Success<T>(T data)
    {
        return Ok(new ApiResponse<T> { Success = true, Data = data });
    }

    public IActionResult Error<T>(string message, int statusCode)
    {
        return StatusCode(statusCode, new ApiResponse<T> { Success = false, Error = message });
    }
}


public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Error { get; set; }
}