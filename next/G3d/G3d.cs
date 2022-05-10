﻿using System.IO;
using Vim.BFast;

namespace G3d
{
    public class G3d
    {
        public readonly Header Header;
        public readonly AttributeBufferCollection AttributeBuffers;

        /// <summary>
        /// Constructor.
        /// </summary>
        public G3d(Header header, AttributeBufferCollection attributeBuffers)
        {
            Header = header;
            AttributeBuffers = attributeBuffers;
        }

        public static bool TryReadG3d(Stream stream, AttributeBufferFactory factory, out G3d g3d)
        {
            g3d = null;
            Header? header = null;
            var attributeBuffers = new AttributeBufferCollection(factory);

            object OnG3dSegment(Stream s, string name, long size)
            {
                if (Header.IsSegmentHeader(name, size) && Header.TryRead(s, size, out var outHeader))
                {
                    // Assign to the header variable in the closure.
                    return header = outHeader;
                }

                // The segment is not the header so treat it as an attribute buffer.
                return attributeBuffers.ReadAttributeBuffer(s, name, size);
            }

            _ = stream.ReadBFast(OnG3dSegment);

            // Failure case if the header was not found.
            if (!header.HasValue)
                return false;

            // Instantiate the object and return.
            g3d = new G3d(header.Value, attributeBuffers);
            return g3d != null;
        }
    }
}
