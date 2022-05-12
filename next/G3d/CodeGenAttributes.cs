using System;

namespace G3d
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttributeDescriptorAttribute : Attribute
    {
        private readonly string _name;

        public AttributeDescriptorAttribute(string name)
            => _name = name;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AttributeCollectionAttribute : Attribute
    {
        private readonly Type[] _bufferClasses;

        public AttributeCollectionAttribute(params Type[] bufferClasses)
            => _bufferClasses = bufferClasses;
    }
}