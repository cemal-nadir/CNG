using Newtonsoft.Json;

namespace CNG.Http.Helpers
{
	public static class JsonHelper
	{
		public static string SerializeObject<T>(T data) =>
			JsonConvert.SerializeObject(data);

		public static T? DeserializeObject<T>(string jsonString) => JsonConvert.DeserializeObject<T>(jsonString);
	}
}
