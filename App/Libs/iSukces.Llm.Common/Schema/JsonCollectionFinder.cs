using System.Collections.Generic;

namespace iSukces.Llm.Common.Schema;

internal readonly record struct JsonCollectionFinder
{
    public static Type? TryFindElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();

        if (!type.IsGenericType)
           return null;
        if (type == typeof(string))
            return null;

        foreach (var i in type.GetInterfaces())
        {
            if (i.IsGenericType)
            {
                var genericTypeDefinition = i.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IEnumerable<>))
                {
                    return type.GenericTypeArguments[0];
                }
            }
        }

        return null;
    }
}
