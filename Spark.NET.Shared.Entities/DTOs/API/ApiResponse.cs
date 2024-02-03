using System.Net;
using System.Runtime.InteropServices.JavaScript;

namespace Spark.NET.Shared.Entities.DTOs.API;

public class ApiResponse
{
    public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }
    public Object Payload { get; set; }

    public ApiResponse(HttpStatusCode code, string message)
    {
        Message = message;
        HttpCode = code;
    }

    public ApiResponse( HttpStatusCode code, string message, Object payload)
    {
        Message = message;
        Payload = payload;

        HttpCode = code;
    }

    public ApiResponse(Object payload)
    {
        Payload = Payload;
    }

    public ApiResponse( HttpStatusCode code, Object payload)
    {
        Payload = payload;
        HttpCode = code;
    }

}