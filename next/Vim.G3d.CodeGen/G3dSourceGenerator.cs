//# define DEBUG_SOURCE_GENERATOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Vim.G3d.CodeGen
{
    [Generator]
    public class G3dSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG_SOURCE_GENERATOR
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
            Debug.WriteLine("Initalize code generator");
#endif
        }

        /// <summary>
        /// Code generation entrypoint
        /// </summary>
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;

            var (@namespace, attributeSrc, metaAttributes) = GetAttributeClassesAndNamespace(syntaxTrees);

            var attributeCollectionSrc = GetAttributeCollectionClass(syntaxTrees, metaAttributes);

            // Final source, including using statements and namespace.
            var source = $@"using System;
using System.Collections.Generic;
using System.IO;
using Vim.BFast;
using Vim.G3d;
using Vim.Math3d;

namespace {@namespace}
{{
{attributeSrc}
{attributeCollectionSrc}
}}
";

            // Add the source code to the compilation
            context.AddSource($"Attributes.g.cs", source);
        }

        /// <summary>
        /// Returns the source code which implement the attribute buffers.
        /// </summary>
        public (string Namespace, string AttributeSrc, MetaAttribute[] MetaAttributes) GetAttributeClassesAndNamespace(IEnumerable<SyntaxTree> syntaxTrees)
        {
            var metaAttributes = syntaxTrees
                .GetClassesWithAttribute(nameof(AttributeDescriptor))
                .Select(item => new MetaAttribute(item.Item1, item.Item2))
                .ToArray();

            if (metaAttributes.Length == 0)
                return default;

            // Collect the namespace.
            if (!SyntaxNodeHelper.TryGetParentSyntax<NamespaceDeclarationSyntax>(metaAttributes.First().ClassDeclarationSyntax, out var namespaceSyntax))
                throw new Exception("Namespace not found.");
            var @namespace = namespaceSyntax.Name.ToString();

            // Generate the code for each class.
            var attrSrc = new StringBuilder();
            foreach (var ma in metaAttributes)
            {
                var attrName = ma.AttributeNameArg;
                var attrType = ma.AttributeTypeArg;
                var arrayType = ma.ArrayTypeArg;
                var indexInto = ma.IndexIntoArg;

                var className = ma.ClassName;

                var attr = new AttributeDescriptor(attrName);
                if (attr.HasErrors)
                {
                    attrSrc.AppendLine($"(ERROR_{attr.Errors:G}, \"{attrName}\")");
                    continue;
                }

                var typedDataType = !string.IsNullOrEmpty(arrayType)
                    ? arrayType
                    : attr.DataType.GetManagedType().ToString();

                var classSrc = $@"
    public partial class {className} : {nameof(IAttribute)}<{typedDataType}>
    {{
        public const string AttributeName = ""{attrName}"";

        public string Name
            => AttributeName;

        public static {nameof(AttributeReader)} CreateAttributeReader()
            => {nameof(AttributeCollectionExtensions)}.CreateAttributeReader<{className}, {typedDataType}>();

        public {nameof(IAttributeDescriptor)} {nameof(AttributeDescriptor)} {{ get; }}
            = new {nameof(AttributeDescriptor)}(AttributeName);

        public {nameof(AttributeType)} {nameof(IAttribute.AttributeType)} {{ get; }}
            = {attrType};

        public Type {nameof(IAttribute.IndexInto)} {{ get; }}
            = {(indexInto == default ? "null" : $"typeof({indexInto})")};

        public {typedDataType}[] TypedData {{ get; set; }}
            = Array.Empty<{typedDataType}>();

        public Array Data => TypedData;

        public void Write(Stream stream)
        {{
            if (TypedData == null || TypedData.Length == 0)
                return;
            stream.Write(TypedData);
        }}
    }}";
                attrSrc.AppendLine(classSrc);
            }

            return (@namespace, attrSrc.ToString(), metaAttributes);
        }

        private readonly Regex typeofRegex = new Regex(@"\s*typeof\(([a-zA-Z0-9-_.]+)\)\s*");

        private string[] GetArgumentTypes(SeparatedSyntaxList<AttributeArgumentSyntax> argList)
        {
            if (argList == null)
                return Array.Empty<string>();

            return argList
                .Select(arg => SyntaxNodeHelper.TypeofArg(arg.ToString()))
                .Where(a => !string.IsNullOrEmpty(a))
                .ToArray();
        }

        public string GetAttributeCollectionClass(IEnumerable<SyntaxTree> syntaxTrees, MetaAttribute[] metaAttributes)
        {
            var collectionClasses = syntaxTrees
                .GetClassesWithAttribute("AttributeCollection")
                .Select(item => (item.Item1, GetArgumentTypes(item.Item2.ArgumentList.Arguments)))
                .ToArray();

            var attrCollectionSrc = new StringBuilder();
            foreach (var (cds, attributeClasses) in collectionClasses)
            {
                var className = cds.Identifier.ToString();

                attrCollectionSrc.AppendLine(
$@"    public partial class {className} : IAttributeCollection
    {{
        public IEnumerable<string> AttributeNames
            => Attributes.Keys;

        public IDictionary<string, IAttribute> Attributes {{ get; }}
            = new Dictionary<string, IAttribute>
            {{
{string.Join(Environment.NewLine, attributeClasses.Select(c =>
$"                [{c}.AttributeName] = new {c}(),"))}
            }};

        public IDictionary<string, AttributeReader> AttributeReaders {{ get; }}
            = new Dictionary<string, AttributeReader>
            {{
{string.Join(Environment.NewLine, attributeClasses.Select(c =>
$"                [{c}.AttributeName] = {c}.CreateAttributeReader(),"))}
            }};

        // Named Attributes.
{string.Join(Environment.NewLine, attributeClasses.Select(c => $@"
        public {c} {c}
        {{
            get => Attributes.TryGetValue({c}.AttributeName, out var attr) ? attr as {c} : default;
            set => Attributes[{c}.AttributeName] = value as IAttribute;
        }}"))}

        /// <inheritdoc/>
        public IAttribute GetAttribute(Type attributeType)
        {{
{string.Join(Environment.NewLine, attributeClasses.Select(c => $@"
            if (attributeType == typeof({c}))
                return {c};"))}

            throw new ArgumentException(""Type {{attributeType.ToString()}} is not supported."");
        }}
    }}");
            }

            return attrCollectionSrc.ToString();
        }
    }
}