using System.Text;
using System.Xml.Serialization;

namespace CNG.Http.Helpers
{
	public static class XmlHelper
	{

		public static string? SerializeObject<T>(T data)
		{
			if (data is null) return null;


			try
			{
				using var sw = new StringWriter();
				var serializer = new XmlSerializer(data.GetType());
				serializer.Serialize(sw, data, null);
				return sw.ToString().Replace("utf-16", "utf-8");
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("XML serialization failed", ex);
			}

		}

		public static T? DeserializeObject<T>(string xmlString)
		{
			try
			{
				XmlSerializer xs = new(typeof(T));
				using var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
				return (T?)xs.Deserialize(ms);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("XML deserialization failed", ex);
			}
		}

		public static object? DeserializeObject(string xmlString)
		{
			try
			{
				XmlSerializer xs = new(typeof(object));
				using var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
				return xs.Deserialize(ms);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("XML deserialization failed", ex);
			}
		}


	}
}
