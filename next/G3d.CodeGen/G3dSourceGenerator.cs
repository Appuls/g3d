// # define DEBUG_SOURCE_GENERATOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using G3d;

namespace G3d.CodeGen
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

            var (@namespace, attributeBufferSrc) = GetAttributeBufferClassesAndNamespace(syntaxTrees);

            // Final source, including using statements and namespace.
            var source = $@"using System;
using G3d;
using Vim.BFast;

namespace {@namespace}
{{
{attributeBufferSrc}
}}
";

            // Add the source code to the compilation
            context.AddSource($"AttributeBuffers.g.cs", source);
        }

        /// <summary>
        /// Returns the source code which implement the attribute buffers.
        /// </summary>
        public (string Namespace, string AttributeBufferSrc) GetAttributeBufferClassesAndNamespace(IEnumerable<SyntaxTree> syntaxTrees)
        {
            var bufferClasses = syntaxTrees
                .GetClassesWithAttribute(nameof(AttributeDescriptor))
                .Select(item => (item.Item1, item.Item2.ArgumentList.Arguments[0].ToString().Trim('"')))
                .ToArray();

            if (bufferClasses.Length == 0)
                return default;

            // Collect the namespace.
            if (!SyntaxNodeHelper.TryGetParentSyntax<NamespaceDeclarationSyntax>(bufferClasses.First().Item1, out var namespaceSyntax))
                throw new Exception("Namespace not found.");
            var @namespace = namespaceSyntax.Name.ToString();

            // Generate the code for each class.
            var attrBufferSrc = new StringBuilder();
            foreach (var (cds, attrName) in bufferClasses)
            {
                var className = cds.Identifier.ToString();

                var attr = new AttributeDescriptor(attrName);
                if (attr.HasErrors)
                {
                    attrBufferSrc.AppendLine($"(ERROR_{attr.Errors:G}, \"{attrName}\")");
                    continue;
                }

                var attrType = attr.DataType.GetManagedType();

                attrBufferSrc.AppendLine($@"
    public partial class {className} : {nameof(IAttributeBuffer)}<{attrType}>
    {{
        public static {nameof(AttributeBufferReader)} CreateAttributeBufferReader()
            => {nameof(AttributeBufferFactory)}.{nameof(AttributeBufferFactory.CreateAttributeBufferReader)}<{className}, {attrType}>();

        public {nameof(IAttributeDescriptor)} {nameof(AttributeDescriptor)} {{ get; }}
            = new {nameof(AttributeDescriptor)}(""{ attrName}"");

        public {attrType}[] TypedData {{ get; set; }}

        public Array Data => TypedData;
    }}");
            }

            return (@namespace, attrBufferSrc.ToString());
        }

        //public string GetAttributeFactory
    }
}