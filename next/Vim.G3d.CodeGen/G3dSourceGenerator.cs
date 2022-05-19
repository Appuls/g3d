//# define DEBUG_SOURCE_GENERATOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Vim.G3d;

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

            var (@namespace, attributeSrc) = GetAttributeClassesAndNamespace(syntaxTrees);

            var attributeCollectionSrc = GetAttributeCollectionClass(syntaxTrees);

            // Final source, including using statements and namespace.
            var source = $@"using System;
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
        public (string Namespace, string AttributeSrc) GetAttributeClassesAndNamespace(IEnumerable<SyntaxTree> syntaxTrees)
        {
            var bufferClasses = syntaxTrees
                .GetClassesWithAttribute(nameof(AttributeDescriptor))
                .Select(item =>
                {
                    var args = item.Item2.ArgumentList.Arguments.ToArray();
                    var arg0 = args[0].ToString().Trim('"'); // mandatory argument
                    var arg1 = args.Length > 1 ? SyntaxNodeHelper.TypeofArg(args[1].ToString()) : null; // type argument
                    return (item.ClassDeclarationSyntax, AttrName: arg0, ArrayType: arg1);
                })
                .ToArray();

            if (bufferClasses.Length == 0)
                return default;

            // Collect the namespace.
            if (!SyntaxNodeHelper.TryGetParentSyntax<NamespaceDeclarationSyntax>(bufferClasses.First().Item1, out var namespaceSyntax))
                throw new Exception("Namespace not found.");
            var @namespace = namespaceSyntax.Name.ToString();

            // Generate the code for each class.
            var attrSrc = new StringBuilder();
            foreach (var (cds, attrName, arrayType) in bufferClasses)
            {
                var className = cds.Identifier.ToString();

                var attr = new AttributeDescriptor(attrName);
                if (attr.HasErrors)
                {
                    attrSrc.AppendLine($"(ERROR_{attr.Errors:G}, \"{attrName}\")");
                    continue;
                }

                var attrType = !string.IsNullOrEmpty(arrayType)
                    ? arrayType
                    : attr.DataType.GetManagedType().ToString();

                var classSrc = $@"
    public partial class {className} : {nameof(IAttribute)}<{attrType}>
    {{
        public const string AttributeName = ""{attrName}"";

        public string Name
            => AttributeName;

        public static {nameof(AttributeReader)} CreateAttributeReader()
            => {nameof(AttributeCollection)}.{nameof(AttributeCollection.CreateAttributeReader)}<{className}, {attrType}>();

        public {nameof(IAttributeDescriptor)} {nameof(AttributeDescriptor)} {{ get; }}
            = new {nameof(AttributeDescriptor)}(AttributeName);

        public {attrType}[] TypedData {{ get; set; }}

        public Array Data => TypedData;

        public void Write(Stream stream)
            => stream.Write(TypedData);
    }}";
                attrSrc.AppendLine(classSrc);
            }

            return (@namespace, attrSrc.ToString());
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

        public string GetAttributeCollectionClass(IEnumerable<SyntaxTree> syntaxTrees)
        {
            var collectionClasses = syntaxTrees
                .GetClassesWithAttribute(nameof(AttributeCollection))
                .Select(item => (item.Item1, GetArgumentTypes(item.Item2.ArgumentList.Arguments)))
                .ToArray();

            var attrCollectionSrc = new StringBuilder();
            foreach (var (cds, attributeClasses) in collectionClasses)
            {
                var className = cds.Identifier.ToString();

                attrCollectionSrc.AppendLine(
$@"    public partial class {className}
    {{
        public {className}()
        {{
{string.Join(Environment.NewLine, attributeClasses.Select(c => $"            {nameof(AttributeCollection.AttributeReaders)}.Add({c}.AttributeName, {c}.CreateAttributeReader());"))}
        }}
    }}");
            }

            return attrCollectionSrc.ToString();
        }
    }
}