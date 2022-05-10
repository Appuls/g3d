using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace G3d
{
    public interface IAttributeBuffer
    {
        IAttributeDescriptor AttributeDescriptor { get; }
        // TODO: collect all relevant actions an attribute buffer can make.
        
    }

    public interface IAttributeBuffer<T> : IAttributeBuffer
    {
        // TODO: typed data

        // Write
    }
}
