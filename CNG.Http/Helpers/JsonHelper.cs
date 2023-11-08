using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CNG.Http.Helpers
{
	public static class JsonHelper
	{
		public static string SerializeObject<T>(T data) =>
			JsonConvert.SerializeObject(data,
				settings: new JsonSerializerSettings()
					{ ContractResolver = new CamelCasePropertyNamesContractResolver() });

		public static T? DeserializeObject<T>(string jsonString) => JsonConvert.DeserializeObject<T>(jsonString);
	}
}
