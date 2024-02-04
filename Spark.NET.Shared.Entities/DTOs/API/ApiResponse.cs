using System.Net;
using System.Runtime.InteropServices.JavaScript;

namespace Spark.NET.Shared.Entities.DTOs.API;

public class ApiResponse
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }
    public Object Payload { get; set; } = new { };
    // public bool Failed => Status != HttpStatusCode.OK;
    public bool Failed { get; set; } = false;

    public ApiResponse(HttpStatusCode code, string message)
    {
        Message = message;
        Status = code;
    }

    public ApiResponse( HttpStatusCode code, string message, Object payload)
    {
        Message = message;
        Payload = payload;

        Status = code;
    }

    public ApiResponse( HttpStatusCode code, string message, bool failed)
    {
        Message = message;
        Status = code;
        Failed = failed;
    }

    public ApiResponse(Object payload)
    {
        Payload = Payload;
    }

    public ApiResponse( HttpStatusCode code, Object payload)
    {
        Payload = payload;
        Status = code;
    }

}