using System;

namespace Vim.G3d
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttributeDescriptorAttribute : Attribute
    {
        private readonly string _name;
        private readonly Type _arrayType;

        public AttributeDescriptorAttribute(string name, Type arrayType)
            => (_name, _arrayType) = (name, arrayType);

        public AttributeDescriptorAttribute(string name)
            : this(name, null)
        { }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AttributeCollectionAttribute : Attribute
    {
        private readonly Type[] _bufferClasses;

        public AttributeCollectionAttribute(params Type[] bufferClasses)
            => _bufferClasses = bufferClasses;
    }
}