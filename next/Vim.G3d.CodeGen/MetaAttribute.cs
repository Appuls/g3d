using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Vim.G3d.CodeGen
{
    public class MetaAttribute
    {
        public ClassDeclarationSyntax ClassDeclarationSyntax { get; set; }
        public string ClassName { get; set; }
        public string AttributeNameArg { get; set; }
        public string AttributeTypeArg { get; set; }
        public string ArrayTypeArg { get; set; }
        public string IndexIntoArg { get; set; }

        public MetaAttribute(ClassDeclarationSyntax cds, AttributeSyntax attributeSyntax)
        {
            ClassDeclarationSyntax = cds;

            ClassName = ClassDeclarationSyntax.Identifier.ToString();

            var args = attributeSyntax.ArgumentList.Arguments.Select(a => a.ToString().Trim()).ToArray();
            AttributeNameArg = args[0].Trim('"');
            AttributeTypeArg = args[1];

            if (args.TryGetOptionalArgValue(nameof(AttributeDescriptorAttribute.ArrayType), out var arrayTypeArg))
                ArrayTypeArg = SyntaxNodeHelper.TypeofArg(arrayTypeArg);


            if (args.TryGetOptionalArgValue(nameof(AttributeDescriptorAttribute.IndexInto), out var indexIntoArg))
                IndexIntoArg = SyntaxNodeHelper.TypeofArg(indexIntoArg);
        }
    }
}
