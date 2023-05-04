using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Spark.NET.Shared.Entities.DTOs.API.Response;

public class ApiResponse
{
    public ApiResponse(HttpStatusCode? httpStatusCode, string message, object? data)
    {
        HttpStatusCode = httpStatusCode ?? HttpStatusCode.OK;
        Message = message ?? "OK";
        Data = data ?? new object();
    }
    [Required] public HttpStatusCode HttpStatusCode { get; set; }
    [Required] public string Message { get; set; }
    [Required] public object? Data { get; set; }
}