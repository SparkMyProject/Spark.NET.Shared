namespace Spark.NET.Infrastructure.Utils;

public class UtilsHelper
{
    public static object GetPropertyFromObject(object obj, string propertyName)
    {
        var type = obj.GetType();
        var prop = type.GetProperty(propertyName);
        var val = prop.GetValue(obj);
        return val;
    }
}