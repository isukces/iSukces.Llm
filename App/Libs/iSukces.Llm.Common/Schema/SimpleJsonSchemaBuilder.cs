using System.Collections.Generic;
using System.Linq;

namespace iSukces.Llm.Common.Schema;

public class SimpleJsonSchemaBuilder
{
    public static SimpleJsonSchemaBuilder Create<T>()
    {
        return new SimpleJsonSchemaBuilder
        {
            MainType = typeof(T)
        };
    }

    public required Type       MainType     { get; set; }
    public          List<Type> IncludeTypes { get; } = [];

    public SimpleJsonSchemaBuilder Include<T>()
    {
        IncludeTypes.Add(typeof(T));
        return this;
    }

    public JSchema Build(JSchemaFeatures features, SchemaNameStyle nameStyle= SchemaNameStyle.Snake)
    {
        var tv = new TypeVisitor();
        tv.Visit(MainType);
        foreach (var type in IncludeTypes)
            tv.Visit(type);

        var builder = new FluentJsonSchemaBuilder(nameStyle, features);
        var types   = tv.RequiresDefinition.ToArray();
        var q       = new HashSet<Type> { MainType };
        foreach (var i in types)
        {
            if (q.Add(i))
                builder.Definitions.Add(i);
        }

        builder.Add(MainType);
        return builder.Build();
    }
}
