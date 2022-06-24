using System;
using System.Linq;
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

        public AttributeType AttributeType
            => Enum.TryParse<AttributeType>(AttributeTypeArg.Replace($"{nameof(AttributeType)}.", ""), out var val)
            ? val
            : throw new Exception("Could not parse attribute type arg");

        public string GetTypedDataType()
        {
            if (!string.IsNullOrEmpty(ArrayTypeArg))
                return ArrayTypeArg;

            if (!AttributeDescriptor.TryParse(AttributeNameArg, out var attr))
                throw new Exception("Could not parse attribute name arg");

            return attr.DataType.GetManagedType().ToString();
        } 
    }
}
