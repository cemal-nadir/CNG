#nullable enable
using System.ComponentModel;

namespace CNG.Extensions
{
  public static class KeyConverterExtensions
  {
    public static TKey ChangeType<TKey>(this object value) => (TKey) KeyConverterExtensions.ChangeType(typeof (TKey), value);

    private static object ChangeType(Type type, object value) => TypeDescriptor.GetConverter(type).ConvertFrom(value) ?? value;

    public static void RegisterTypeConverter<TKey, TConverter>() where TConverter : TypeConverter => TypeDescriptor.AddAttributes(typeof (TKey), new Attribute[]
    {
      new TypeConverterAttribute(typeof (TConverter))
    });
  }
}
