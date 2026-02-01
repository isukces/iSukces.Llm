using System.Collections.Generic;
using System.Linq;

namespace iSukces.Llm.Common.Schema;

public sealed class TypeVisitor
{
    public void Visit(Type? type)
    {
        Start:
        if (type is null || type.IsPrimitive || type == typeof(string) || type == typeof(object))
            return;
        if (!_visited.Add(type))
            return;

        if (type.IsEnum)
        {
            _requiresEnums.Add(type);
            return;
        }

        if (type.IsGenericType)
        {
            var gen = type.GetGenericTypeDefinition();
            if (gen == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
                goto Start;
            }

            if (gen == typeof(List<>))
            {
                type = type.GenericTypeArguments[0];
                goto Start;
            }
        }

        if (type.IsArray)
        {
            type = type.GetElementType();
            goto Start;
        }

        var props = type.GetProperties();
        foreach (var prop in props)
            Visit(prop.PropertyType);
        _requiresObjects.Add(type);
    }

    public IEnumerable<Type> RequiresDefinition => _requiresEnums.Concat(_requiresObjects);

    private readonly HashSet<Type> _visited = [];
    private readonly List<Type> _requiresEnums = [];
    private readonly List<Type> _requiresObjects = [];
}
