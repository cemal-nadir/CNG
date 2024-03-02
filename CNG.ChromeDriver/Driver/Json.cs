using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CNG.ChromeDriver.Driver
{
    public static class Json
    {
        public static Dictionary<string, object> DeserializeData(string data)
        {
            var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            return obj == null
                ? throw new Exception("Json data cannot be null.")
                : (Dictionary<string, object>)DeserializeData(obj);
        }

        private static IDictionary<string, object> DeserializeData(JToken data)
        {
            return DeserializeData(
                data.ToObject<Dictionary<string, object>>() ?? new());
        }

        private static IList<object> DeserializeData(JArray data)
        {
            var list = data.ToObject<List<object>>() ?? new();

            for (var i = 0; i < list.Count; i++)
            {
                var value = list[i];

                list[i] = value switch
                {
                    JObject jObject => DeserializeData(jObject),
                    JArray array => DeserializeData(array),
                    _ => list[i]
                };
            }
            return list;
        }

        private static IDictionary<string, object> DeserializeData(IDictionary<string, object> data)
        {
            foreach (var key in data.Keys.ToArray())
            {
                var value = data[key];

                data[key] = value switch
                {
                    JObject jObject => DeserializeData(jObject),
                    JArray array => DeserializeData(array),
                    _ => data[key]
                };
            }
            return data;
        }
    }
}
