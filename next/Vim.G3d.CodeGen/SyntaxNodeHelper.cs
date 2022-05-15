using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Vim.G3d.CodeGen
{
    // From: https://stackoverflow.com/a/23249021

    public static class SyntaxNodeHelper
    {
        /// <summary>
        /// Allows the caller to query the namespace of the given syntax node.
        public static bool TryGetParentSyntax<T>(SyntaxNode syntaxNode, out T result)
            where T : SyntaxNode
        {
            // set defaults
            result = null;

            if (syntaxNode == null)
            {
                return false;
            }

            try
            {
                syntaxNode = syntaxNode.Parent;

                if (syntaxNode == null)
                {
                    return false;
                }

                if (syntaxNode.GetType() == typeof(T))
                {
                    result = syntaxNode as T;
                    return true;
                }

                return TryGetParentSyntax<T>(syntaxNode, out result);
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<(ClassDeclarationSyntax, AttributeSyntax)> GetClassesWithAttribute(
            this IEnumerable<SyntaxTree> syntaxTrees,
            string attributeName)
        {
            var classDeclarationSyntaxes = syntaxTrees
                .SelectMany(st => st.GetRoot().DescendantNodes()
                    .Where(n => n is ClassDeclarationSyntax)
                    .Select(n => n as ClassDeclarationSyntax))
                .ToArray();

            // Collect the class declarations which have the attribute name.
            var cdsWithAttr = new List<(ClassDeclarationSyntax Cds, AttributeSyntax Attr)>();
            foreach (var cds in classDeclarationSyntaxes)
            {
                if (cds.AttributeLists.Count == 0)
                    continue;

                foreach (var attr in cds.AttributeLists.SelectMany(a => a.Attributes))
                {
                    if (attr.Name.GetText().ToString() != attributeName)
                        continue;

                    cdsWithAttr.Add((cds, attr));
                    break;
                    //var arg0 = attr.ArgumentList.Arguments[0].ToString().Trim('"');
                    //cdsWithAttr.Add((cds, arg0));
                }
            }
            return cdsWithAttr;
        }
    }
}
