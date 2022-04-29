using System;

namespace G3d.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttributeDescriptorAttribute : Attribute
    {
        private readonly string _name;

        public AttributeDescriptorAttribute(string name)
        {
            _name = name;
        }
    }
}