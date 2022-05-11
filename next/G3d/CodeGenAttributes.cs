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
    public class AttributeBufferFactoryAttribute : Attribute
    {
        private readonly Type[] _bufferClasses;

        public AttributeBufferFactoryAttribute(params Type[] bufferClasses)
            => _bufferClasses = bufferClasses;
    }
}