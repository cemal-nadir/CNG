#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CNG.Extensions
{
  public static class JsonExtensions
  {
    public static string ToJson(this object source, bool camelCase = true)
    {
      string json;
      if (!camelCase)
        json = JsonConvert.SerializeObject(source);
      else
        json = JsonConvert.SerializeObject(source, new JsonSerializerSettings()
        {
          ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
      return json;
    }

    public static T? FromJson<T>(this string source, bool camelCase = true)
    {
      try
      {
        T? obj;
        if (!camelCase)
          obj = JsonConvert.DeserializeObject<T>(source);
        else
          obj = JsonConvert.DeserializeObject<T>(source, new JsonSerializerSettings()
          {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
          });
        return obj;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}
