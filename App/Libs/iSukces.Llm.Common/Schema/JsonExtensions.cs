using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;

namespace iSukces.Llm.Common.Schema;

internal static class JsonExtensions
{
    private static JsonNode? ConvertAny(object o)
    {
        return o switch
        {
            // Podstawowe typy
            int i => JsonValue.Create(i),
            double d => JsonValue.Create(d),
            bool b => JsonValue.Create(b),
            string s => JsonValue.Create(s),

            // Dodatkowe typy numeryczne
            long l => JsonValue.Create(l),
            float f => JsonValue.Create(f),
            decimal dec => JsonValue.Create(dec),
            byte by => JsonValue.Create(by),
            sbyte sb => JsonValue.Create(sb),
            short sh => JsonValue.Create(sh),
            ushort ush => JsonValue.Create(ush),
            uint ui => JsonValue.Create(ui),
            ulong ul => JsonValue.Create(ul),

            // DateTime
            DateTime dt => JsonValue.Create(dt),
            DateTimeOffset dto => JsonValue.Create(dto),
            DateOnly dateOnly => JsonValue.Create(dateOnly.ToString("O")),
            TimeOnly timeOnly => JsonValue.Create(timeOnly.ToString("O")),

            // Guid
            Guid guid => JsonValue.Create(guid),

            // Null
            null => JsonValue.Create((string?)null),

            // Kolekcje
            IEnumerable enumerable when o is not string =>
                new JsonArray(enumerable.Cast<object>().Select(ConvertAny).ToArray()),

            // Słowniki
            IDictionary dict => new JsonObject(
                dict.Cast<DictionaryEntry>()
                    .Select(entry => new KeyValuePair<string, JsonNode?>(
                        entry.Key.ToString()!,
                        ConvertAny(entry.Value!)))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)),

            // Obiekty - konwersja przez properties
            _ => ConvertObject(o)
        };
    }

    private static JsonObject ConvertObject(object obj)
    {
        var jsonObject = new JsonObject();
        var properties = ReflectionTools.GetPropertiesWithBaseFirst(obj.GetType());

        foreach (var prop in properties)
        {
            if (!prop.CanRead) continue;

            var value = prop.GetValue(obj);
            var name  = prop.Name[..1].ToLower() + prop.Name[1..]; // camelCase
            jsonObject[name] = value is not null ? ConvertAny(value) : null;
        }

        return jsonObject;
    }

    private static string[] GetEnums(Type type, SchemaNameStyle nameStyle)
    {
        var enums = Enum.GetNames(type)
            .Select(a => JsonName(a, nameStyle))
            .ToArray();
        return enums;
    }

    private static string JsonName(string name, SchemaNameStyle nameStyle)
    {
        name = name[..1].ToLower() + name[1..];
        if (nameStyle == SchemaNameStyle.Camel)
            return name;

        var sb = new StringBuilder();
        sb.Append(name[0]);

        for (int i = 1; i < name.Length; i++)
        {
            char c = name[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLower(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    extension(PropertyInfo prop)
    {
        private void ProcessDefaultValue(JsonSchemaBuilder propBuilder)
        {
            var attr = prop.GetCustomAttribute<DefaultValueAttribute>();
            if (attr is null) return;
            switch (attr.Value)
            {
                case int i:
                    propBuilder.Default(i);
                    break;
                case double d:
                    propBuilder.Default(d);
                    break;
                default: throw new InvalidCastException();
            }
        }

        private void ProcessEnumValues(JsonSchemaBuilder propBuilder)
        {
            var attr = prop.GetCustomAttribute<EnumValuesAttribute>();
            if (attr is null) return;
            propBuilder.Enum = attr.Values;
        }

        private void ProcessExamples(JsonSchemaBuilder propBuilder)
        {
            var attr = prop.GetCustomAttribute<ExamplesAttribute>();
            if (attr is null) return;
            var enumerable = attr.Values.Select(ConvertAny);
            propBuilder.Examples(enumerable);
        }

        private void ProcessMinItemsCount(JsonSchemaBuilder propBuilder)
        {
            var attr = prop.GetCustomAttribute<MinItemsCountAttribute>();
            if (attr is null) return;
            propBuilder.MinItems = attr.MinItemsCount;
        }

        private void ProcessMinProperties(JsonSchemaBuilder propBuilder)
        {
            var attr = prop.GetCustomAttribute<MinPropertiesAttribute>();
            if (attr is null) return;
            propBuilder.MinProperties(attr.MinProperties);
        }
    }

    extension(JsonSchemaBuilder builder)
    {
        private JsonSchemaBuilder WithDescription(MemberInfo prop)
        {
            var at = prop.GetCustomAttribute<DescriptionAttribute>();
            if (at != null)
                builder.Description = at.Description;
            return builder;
        }

        public JsonSchemaBuilder WithProperties<T>(SchemaContext? context = null)
        {
            return builder.WithProperties(typeof(T), context);
        }

        public JsonSchemaBuilder WithProperties(Dictionary<string, JSchema> properties)
        {
            builder.Properties = properties;
            return builder;
        }

        public JsonSchemaBuilder WithProperties(Type type, SchemaContext? context = null)
        {
            {
                var refName = context?.TryGetReference.Invoke(type);
                if (!string.IsNullOrEmpty(refName))
                {
                    builder.Ref(refName);
                    return builder;
                }
            }
            var nameStyle = context?.Style ?? SchemaNameStyle.Snake;
            if (type.IsEnum)
            {
                var enums = GetEnums(type, nameStyle);
                builder.Type = JSchemaType.String;
                builder.Enum = enums;
                return builder;
            }

            List<string> required = [];
            builder.Type = JSchemaType.Object;

            Dictionary<string, JSchema> dict  = [];
            var                         props = ReflectionTools.GetPropertiesWithBaseFirst(type);
            foreach (var prop in props)
            {
                var propBuilder = Json_schema_builder(prop);

                prop.ProcessMinProperties(propBuilder);
                prop.ProcessMinItemsCount(propBuilder);
                prop.ProcessEnumValues(propBuilder);
                prop.ProcessDefaultValue(propBuilder);
                prop.ProcessExamples(propBuilder);

                var propName = JsonName(prop.Name, nameStyle);
                dict[propName] = propBuilder.Build();

                if (prop.GetCustomAttribute<JsonSchemaRequiredAttribute>() is not null)
                    required.Add(propName);
            }

            builder.Properties = dict;

            if (required.Count > 0)
                builder.Required = required.ToArray();
            return builder;

            JsonSchemaBuilder Json_schema_builder(PropertyInfo prop)
            {
                var propBuilder = new JsonSchemaBuilder(context?.Features ?? JSchemaFeatures.Default);
                {
                    var search = context.AllowReferences() || !prop.PropertyType.IsEnum;
                    if (search)
                    {
                        var added = TryAddTypeReference(propBuilder, prop.PropertyType);
                        if (added)
                            return propBuilder;
                    }
                }
                propBuilder
                    .WithType(prop.PropertyType, out var elementType)
                    .WithDescription(prop);

                if (prop.PropertyType.IsEnum)
                {
                    var enums = GetEnums(prop.PropertyType, nameStyle);
                    propBuilder.WithEnum(enums);
                }

                if (elementType is not null)
                {
                    if (JsonSchemaTypeConverter.IsPrimitive(elementType, out var primitiveType))
                        propBuilder.Items.AddType(primitiveType);
                    else
                    {
                        var added = TryAddTypeReference(propBuilder, elementType);
                        if (!added)
                            throw new InvalidOperationException($"Type {elementType.Name} not found in schema");
                    }
                }

                return propBuilder;
            }

            bool TryAddTypeReference(JsonSchemaBuilder propBuilder, Type elementType)
            {
                var allowReferences = context.AllowReferences();
                if (allowReferences)
                {
                    var refName = context?.TryGetReference.Invoke(elementType);
                    if (string.IsNullOrEmpty(refName))
                        return false;
                    propBuilder.Items.AddReference(refName);
                }
                else
                {
                    var schema = context?.TryGetSchema?.Invoke(elementType);
                    if (schema is null)
                        return false;
                    propBuilder.Items.AddSchema(schema);
                }

                return true;
            }
        }

        private JsonSchemaBuilder WithType(Type type, out Type? elementType)
        {
            var schemaType = JsonSchemaTypeConverter.FindSchemaType(type, out elementType);
            builder.Type = schemaType;
            return builder;
        }
    }
}
