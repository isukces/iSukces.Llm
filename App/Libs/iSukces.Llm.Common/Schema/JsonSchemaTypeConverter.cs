using System.Collections.Generic;

namespace iSukces.Llm.Common.Schema;

internal static class JsonSchemaTypeConverter
{
    static JsonSchemaTypeConverter()
    {
        Add<bool>(JSchemaType.Boolean);

        Add<string>(JSchemaType.String);

        Add<int>(JSchemaType.Integer);
        Add<uint>(JSchemaType.Integer);
        Add<long>(JSchemaType.Integer);
        Add<ulong>(JSchemaType.Integer);
        Add<short>(JSchemaType.Integer);
        Add<ushort>(JSchemaType.Integer);
        Add<byte>(JSchemaType.Integer);
        Add<sbyte>(JSchemaType.Integer);

        Add<float>(JSchemaType.Number);
        Add<double>(JSchemaType.Number);
        Add<decimal>(JSchemaType.Number);
        Add<object>(JSchemaType.Object);

        return;

        static void Add<T>(JSchemaType x)
        {
            _dict.Add(typeof(T), x);
            Primitive.Add(typeof(T));
        }
    }

    public static JSchemaType FindSchemaType(Type t, out Type? elementType)
    {
        elementType = null;
        var c = JsonCollectionFinder.TryFindElementType(t);
        if (c is not null)
        {
            elementType = c;
            return JSchemaType.Array;
        }

        if (t.IsGenericType)
        {
            var def = t.GetGenericTypeDefinition();
            if (def == typeof(Nullable<>))
                t = t.GenericTypeArguments[0];
        }

        if (_dict.TryGetValue(t, out var x))
            return x;
        if (t.IsEnum)
            return JSchemaType.String;
        if (t.IsClass)
            return JSchemaType.Object;
        throw new NotImplementedException();
    }

    public static bool IsPrimitive(Type t, out JSchemaType type)
    {
        var contains = Primitive.Contains(t);
        type = contains ? _dict[t] : JSchemaType.None;
        return contains;
    }

    private static readonly Dictionary<Type, JSchemaType> _dict = new();
    private static readonly HashSet<Type> Primitive = [];
}
