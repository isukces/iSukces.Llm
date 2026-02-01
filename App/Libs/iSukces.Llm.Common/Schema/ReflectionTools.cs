using System.Collections.Generic;
using System.Linq;

namespace iSukces.Llm.Common.Schema;

internal static class ReflectionTools
{
    public static PropertyInfo[] GetPropertiesWithBaseFirst(Type type,
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    {
        var hierarchy = new List<Type>();

        // Zbierz hierarchię typów od najbardziej bazowego do najbardziej pochodnego
        while (type != null && type != typeof(object))
        {
            hierarchy.Add(type);
            type = type.BaseType;
        }

        // Odwróć, aby mieć od bazowego do pochodnego
        hierarchy.Reverse();

        // Zbierz właściwości w kolejności od klas bazowych
        var properties = new Dictionary<int, PropertyInfo>();
        var addedNames = new HashSet<string>();

        foreach (var t in hierarchy)
        {
            var declaredProps = t.GetProperties(bindingFlags);
            foreach (var prop in declaredProps)
            {
                // Unikaj duplikatów (przesłonięte właściwości)
                if (addedNames.Add(prop.Name))
                    properties.Add(properties.Count, prop);
            }
        }

        var result = properties
            .OrderBy(a =>
            {
                var s = a.Value.GetCustomAttribute<SortOrderAttribute>();
                return s?.Order ?? 0;
            })
            .ThenBy(a => a.Key)
            .Select(a => a.Value)
            .ToArray();

        return result.ToArray();
    }
}
