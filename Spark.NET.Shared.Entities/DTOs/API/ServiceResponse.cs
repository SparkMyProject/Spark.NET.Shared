using System.Net;

namespace Spark.NET.Shared.Entities.DTOs.API;

public class ServiceResponse
{
    public bool Failed { get; set; } = false;
    public string Message { get; set; }
    public Object Payload { get; set; }

    public ServiceResponse(string message, bool boolean)
    {
        Message = message;
        Failed = boolean;
    }
    public ServiceResponse(string message, bool boolean, Object payload)
    {
        Message = message;
        Failed = boolean;
        Payload = payload;
    }
    
    public ServiceResponse(Object payload, bool boolean)
    {
        Payload = payload;
        Failed = boolean;
    }
    public ServiceResponse(Object payload)
    {
        Payload = payload;
    }
}