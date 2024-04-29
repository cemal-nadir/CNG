using System.Reflection;

namespace CNG.Extensions;
public static class ObjectExtensions
{
    public static bool IsNull(this object? obj)
    {
        return (obj == null || obj == DBNull.Value);
    }
    public static int? ToInt32Nullable(this object? obj)
    {
        int? result = null;
        if (obj.IsNull())
            return result;

        result = obj is Enum ? (int)obj : Convert.ToInt32(obj);
        return result;
    }
    public static Dictionary<string, dynamic> ToDictionary(this object? obj)
    {
        var result = new Dictionary<string, dynamic>();
        try
        {
            var properties = obj?.GetType().GetProperties().Where(t => t.DeclaringType != null && !t.PropertyType.ToString().Contains("Collection") && !t.DeclaringType.Name.Contains("Proxy")).ToArray();
            if (properties != null)
                foreach (var property in properties)
                {
                    result.Add(property.Name, property.GetValue(obj) ?? string.Empty);
                }
        }
        catch (Exception)
        {

        }
        return result;
    }
    public static int ToInt32(this object obj, int defaultValue = 0)
    {
        var result = obj is Enum ? (int)obj : ToInt32Nullable(obj) ?? defaultValue;
        return result;
    }
    public static bool CheckPropertyName(this object? instance, string propertyName)
    {
        if (instance == null || string.IsNullOrEmpty(propertyName))
            return false;

        var bindingAttr = BindingFlags.Public | BindingFlags.Instance;

        var pi = instance.GetType().GetProperty(propertyName, bindingAttr);
        return pi != null;
    }
    public static bool SetPropertyValue(this object? instance, string propertyName, object newValue, bool changeChildObject = false)
    {
        if (instance == null || string.IsNullOrEmpty(propertyName))
            return false;

        var pi = instance.GetType().GetProperty(propertyName);
        if (pi != null)
        {
            if (pi.CanWrite)
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(pi.PropertyType);
                if (converter.CanConvertFrom(newValue.GetType()))
                {
                    var convertedValue = converter.ConvertFrom(newValue);
                    pi.SetValue(instance, convertedValue);
                }
                else
                {
                    pi.SetValue(instance, newValue);
                }
            }
        }

        if (!changeChildObject) return pi != null;
        var properties = instance.GetType().GetProperties();
        foreach (var p in properties.Where(prop => prop.CanRead && prop.CanWrite))
        {
            if ((!p.IsClass()) || (p.PropertyType == typeof(string)) || (p.PropertyType.IsValueType)) continue;
            if (!p.PropertyType.IsList()) continue;
            dynamic copyValue = p.GetValue(instance)!;
            if (copyValue == null) continue;
            foreach (object currentRow in copyValue)
            {
                currentRow.SetPropertyValue(propertyName, newValue, true);
            }
        }
        return pi != null;
    }
    public static bool SetPropertiesValue(this object? instance, string propertyName, object newValue)
    {
        if (instance == null ||string.IsNullOrEmpty(propertyName))
            return false;

        var properties = instance.GetType().GetProperties().Where(t => t.Name.Contains(propertyName));
        foreach (var property in properties)
        {
            if (!property.CanWrite) continue;
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(property.PropertyType);
            if (converter.CanConvertFrom(newValue.GetType()))
            {
                var convertedValue = converter.ConvertFrom(newValue);
                property.SetValue(instance, convertedValue);
            }
            else
            {
                property.SetValue(instance, newValue);
            }
        }
        return true;
    }
    public static bool IsList(this Type type)
    {
        if (null == type)
            throw new ArgumentNullException(nameof(type));
        if (type.Name.Contains("Collection"))
            return true;
        return typeof(System.Collections.IList).IsAssignableFrom(type) || type.GetInterfaces()
            .Any(it => it.IsGenericType && typeof(IList<>) == it.GetGenericTypeDefinition());
    }

    public static object? GetPropertyValue(this object? instance, string propertyName)
    {
        object? result = null;

        if (instance == null || string.IsNullOrEmpty(propertyName) )
            return result;

        var bindingAttr = BindingFlags.Public | BindingFlags.Instance;

        var pi = instance.GetType().GetProperty(propertyName, bindingAttr) ??
                 instance.GetType().GetProperty(propertyName);

        result = pi != null ? pi.GetValue(instance, null) : "";

        return result;
    }
    public static bool IsClass(this PropertyInfo property)
    {
        var result = false;
        if (property.DeclaringType != null)
        {
            if (property.DeclaringType.IsClass)
                result = true;
        }
        else if (property.PropertyType.IsClass)
        {
            result = true;
        }
        return result;
    }
    public static TDestination? ToObject<TDestination>(this object? sourceObject, TDestination? destinationObject) where TDestination : class
    {

        if (sourceObject == null || destinationObject == null)
            return destinationObject;


        var properties = destinationObject.GetType().GetProperties();

        foreach (var p in properties.Where(prop => prop is { CanRead: true, CanWrite: true }))
        {

            var sourceProperty = sourceObject.GetType().GetProperty(p.Name);
            if (sourceProperty == null) continue;

            if ((p.IsClass()) && (p.PropertyType != typeof(string)) && (!p.PropertyType.IsValueType))
            {
                try
                {
                    dynamic destinationClass = Activator.CreateInstance(p.PropertyType)!;
                    dynamic copyValue = sourceProperty.GetValue(sourceObject)!;
                    if (copyValue == null) continue;
                    if (p.PropertyType.IsList())
                    {
                        foreach (object currentRow in copyValue)
                        {
                            foreach (var childProperty in p.PropertyType.GetProperties())
                            {
                                if ("Item" != childProperty.Name || typeof(object) == p.PropertyType) continue;
                                var addRow = Activator.CreateInstance(childProperty.PropertyType);
                                addRow = currentRow.ToObject(addRow);
                                destinationClass.GetType().GetMethod("Add").Invoke(destinationClass, new[] { addRow });
                            }
                        }
                    }
                    else
                    {
                        ToObject(copyValue, destinationClass);
                    }
                    p.SetValue(destinationObject, destinationClass);

                }
                catch (Exception)
                {

                }
            }
            else
            {
                var copyValue = sourceProperty.GetValue(sourceObject);
                if (p.PropertyType.Name != sourceProperty.PropertyType.Name) continue;
                if (copyValue == null)
                {
                    p.SetValue(destinationObject, null);
                    continue;
                }
                try
                {
                    var converter = System.ComponentModel.TypeDescriptor.GetConverter(p.PropertyType);
                    if (converter.CanConvertFrom(copyValue.GetType()))
                    {
                        var convertedValue = converter.ConvertFrom(copyValue);
                        p.SetValue(destinationObject, convertedValue);
                    }
                    else
                    {
                        p.SetValue(destinationObject, copyValue);
                    }
                }
                catch (Exception)
                {
                    p.SetValue(destinationObject, copyValue.ToString());
                }
            }
        }
        return destinationObject;
    }

}
