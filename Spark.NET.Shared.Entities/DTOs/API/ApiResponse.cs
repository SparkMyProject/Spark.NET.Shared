using System.Net;
using System.Runtime.InteropServices.JavaScript;

namespace Spark.NET.Shared.Entities.DTOs.API;

public class ApiResponse
{
    public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }
    public Object Payload { get; set; }

    public ApiResponse(string message, HttpStatusCode code)
    {
        Message = message;
        HttpCode = code;
    }

    public ApiResponse(string message, HttpStatusCode code, Object payload)
    {
        Message = message;
        Payload = payload;

        HttpCode = code;
    }

    public ApiResponse(Object payload)
    {
        Payload = Payload;
    }

    public ApiResponse(Object payload, HttpStatusCode code)
    {
        Payload = payload;
        HttpCode = code;
    }

}