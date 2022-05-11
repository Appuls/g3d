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

        public void Execute(GeneratorExecutionContext context)
        {
            // Collect all the class declarations
            var classDeclarationSyntaxes = context.Compilation.SyntaxTrees
                .SelectMany(st => st.GetRoot().DescendantNodes()
                    .Where(n => n is ClassDeclarationSyntax)
                    .Select(n => n as ClassDeclarationSyntax))
                .ToArray();

            // Collect the class declarations which have the "AttributeDescriptor" attribute along with the name of the supplied buffer.
            var cdsWithAttr = new List<(ClassDeclarationSyntax Cds, string AttrName)>();
            foreach (var cds in classDeclarationSyntaxes)
            {
                if (cds.AttributeLists.Count == 0)
                    continue;

                foreach (var attr in cds.AttributeLists.SelectMany(a => a.Attributes))
                {
                    if (attr.Name.GetText().ToString() != "AttributeDescriptor")
                        continue;

                    var arg0 = attr.ArgumentList.Arguments[0].ToString().Trim('"');
                    cdsWithAttr.Add((cds, arg0));
                    break;
                }
            }

            if (cdsWithAttr.Count == 0)
                return;

            // Collect the namespace.
            if (!SyntaxNodeHelper.TryGetParentSyntax<NamespaceDeclarationSyntax>(cdsWithAttr.First().Cds, out var namespaceSyntax))
            {
                throw new Exception("Namespace not found.");
            }
            var @namespace = namespaceSyntax.Name.ToString();

            // Generate the code for each class.
            var classBuffers = new StringBuilder();
            foreach (var (cds, attrName) in cdsWithAttr)
            {
                var className = cds.Identifier.ToString();

                var testAttr = new AttributeDescriptor(attrName);
                var ctorStr = testAttr.HasErrors
                    ? $"(ERROR_{testAttr.Errors:G}, \"{attrName}\")"
                    : $"new {nameof(AttributeDescriptor)}(\"{attrName}\")";

                // TODO: more interface implementations for IAttributeBuffer

                classBuffers.AppendLine($@"
    public partial class {className} : IAttributeBuffer
    {{
        public IAttributeDescriptor AttributeDescriptor {{ get; }}
            = {ctorStr};
    }}");
            }

            // TODO: helper class to parse whole g3d AttributeBufferCollection from a bfast stream

            // Final source, including using statements and namespace.
            var source = $@"using System;
using G3d;

namespace {@namespace}
{{{classBuffers}}}
";

            // Add the source code to the compilation
            context.AddSource($"AttributeBuffers.g.cs", source);
        }
    }
}