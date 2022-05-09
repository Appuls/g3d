using System;
using System.Collections.Generic;
using System.Text;

namespace G3d
{
    public interface IAttributeBuffer
    {
        IAttributeDescriptor AttributeDescriptor { get; }

        // TODO: collect all relevant actions an attribute buffer can make.
        // Write
    }
}
