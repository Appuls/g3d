using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace G3d
{
    public interface IAttributeBuffer
    {
        IAttributeDescriptor AttributeDescriptor { get; }
        Array Data { get; }
    }

    public interface IAttributeBuffer<T> : IAttributeBuffer
    {
        // TODO: typed data

        T[] TypedData { get; set; }
    }
}
