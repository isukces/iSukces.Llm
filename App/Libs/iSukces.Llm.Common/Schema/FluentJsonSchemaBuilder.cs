using System.Collections.Generic;
using System.Linq;

namespace iSukces.Llm.Common.Schema;

public class FluentJsonSchemaBuilder
{
    private readonly JSchemaFeatures _features;

    public FluentJsonSchemaBuilder(SchemaNameStyle nameStyle, JSchemaFeatures features)
    {
        _features   = features;
        NameStyle   = nameStyle;
        Definitions = new DefinitionsBuilder(this);
        _root       = new JsonSchemaBuilder(features);
        Tools       = new ToolsBuilder(this);
    }

    public void Add<T>()
    {
        _root.WithProperties<T>(GetSchemaContext());
    }

    public void Add(Type type)
    {
        _root.WithProperties(type, GetSchemaContext());
    }

    public JSchema Build()
    {
        var dict = Definitions.Items
            .ToDictionary(a => a.Key, a => a.Value.Build());
        {
            var properties = Tools.Items.ToDictionary(a => a.Key, a => a.Value.Build());
            if (properties.Count > 0)
            {
                var tools = new JsonSchemaBuilder(_features)
                    .WithProperties(properties);
                dict.Add("Tools", tools.Build());
            }
        }
        _root.AppendDefinitions(dict);
        return _root.Build();
    }

    private SchemaContext GetSchemaContext()
    {
        return new SchemaContext
        {
            TryGetReference = Definitions.GetReference,
            Style           = NameStyle,
            Features        = _features,
            TryGetSchema    = Definitions.GetSchema
        };
    }

    public SchemaNameStyle    NameStyle   { get; }
    public DefinitionsBuilder Definitions { get; }
    public ToolsBuilder       Tools       { get; }

    private readonly JsonSchemaBuilder _root;

    public abstract class BaseBuilder
    {
        protected void Add(Type type, string name, SchemaContext? context)
        {
            Items[name] = new JsonSchemaBuilder(_features)
                .WithProperties(type, context);
        }

        protected void Add<T>(string name, SchemaContext? context)
        {
            Items[name] = new JsonSchemaBuilder(_features).WithProperties<T>(context);
        }

        private readonly JSchemaFeatures _features;

        protected BaseBuilder(JSchemaFeatures features)
        {
            _features = features;
        }

        public OrderedDictionary<string, JsonSchemaBuilder> Items { get; } = [];
    }


    public class DefinitionsBuilder(FluentJsonSchemaBuilder owner)
        : BaseBuilder(owner._features)
    {
        public DefinitionsBuilder Add(Type type, string name = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                name = type.Name;
            Add(type, name, owner.GetSchemaContext());
            Names[type] = name;
            return this;
        }

        // public DefinitionsBuilder Add<T>(string name = "") => Add(typeof(T), name);

        internal string? GetReference(Type type)
        {
            if (Names.TryGetValue(type, out var name))
                return $"#/definitions/{name}";
            return null;
        }

        private OrderedDictionary<Type, string> Names { get; } = [];

        public JSchema? GetSchema(Type type)
        {
            if (!Names.TryGetValue(type, out var name)) return null;
            var builder = Items[name];
            var schema= builder.Build();
            return schema;
        }
    }

    public class ToolsBuilder(FluentJsonSchemaBuilder owner) 
        : BaseBuilder(owner._features)
    {
        public ToolsBuilder Add<T>(string name)
        {
            Add<T>(name, owner.GetSchemaContext());
            return this;
        }

        public ToolsBuilder<T> Start<T>()
        {
            return new(this);
        }
    }

    public class ToolsBuilder<T>(ToolsBuilder owner)
    {
        public ToolsBuilder<T> Add<T1>(T name)
        {
            owner.Add<T1>(name!.ToString()!);
            return this;
        }
    }
}