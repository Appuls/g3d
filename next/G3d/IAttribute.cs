using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace G3d
{
    public interface IAttribute
    {
        string Name { get; }
        IAttributeDescriptor AttributeDescriptor { get; }
        Array Data { get; }
    }

    public interface IAttribute<T> : IAttribute
    {
        T[] TypedData { get; set; }
    }
}
