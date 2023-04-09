
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace CNG.Core.Helpers
{
    public static class EnumHelper
    {
        public static Dictionary<int, string?> List<T>()
        {
            var dictionary = new Dictionary<int, string?>();
            foreach (var obj in Enum.GetValues(typeof(T)))
            {
                var key = (int)obj;
                var str = obj.ToString();
                var displayValue = GetDisplayValue((T)Enum.ToObject(typeof(T), key));
                dictionary.Add(key, (string.IsNullOrEmpty(displayValue) ? str : displayValue));
            }
            return dictionary;
        }

        public static List<SelectList<int>> ToList(
          this Dictionary<int, string> source)
        {
            return source.Select((Func<KeyValuePair<int, string>, SelectList<int>>)(item => new SelectList<int>(item.Key, item.Value))).ToList();
        }

        public static string? GetDisplayValue<T>(T value)
        {
            var field = value?.GetType().GetField(value.ToString() ?? string.Empty);
            if (field == null)
                return value?.ToString() ?? string.Empty;


            return (field.GetCustomAttributes(typeof(DisplayAttribute), false) is DisplayAttribute[] customAttributes
                ? customAttributes[0].ResourceType
                : null) != null
                ? LookupResource(
                    (field.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[])?[0]
                    .ResourceType,
                    (field.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[])?[0].Name)
                : (((DisplayAttribute[])field.GetCustomAttributes(typeof(DisplayAttribute), false)).Length == 0
                    ? value?.ToString()
                    : (field.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[])?[0].Name);


        }

        private static string? LookupResource(IReflect? resourceManagerProvider, string? resourceKey)
        {
            foreach (var property in resourceManagerProvider?.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)??new PropertyInfo[]{})
            {
                if (!(property.PropertyType != typeof(ResourceManager)))
                    return (property.GetValue(null, null) as ResourceManager)?.GetString(resourceKey??"");
            }
            return resourceKey;
        }
    }
}
