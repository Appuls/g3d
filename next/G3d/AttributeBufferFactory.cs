using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace G3d
{
    public class AttributeBufferFactory
    {
        public delegate IAttributeBuffer AttributeReader(Stream stream, long size);

        /// <summary>
        /// The mapping from attribute descriptor name to attribute reader.
        /// </summary>
        public readonly IReadOnlyDictionary<string, AttributeReader> AttributeReaders;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AttributeBufferFactory(IReadOnlyDictionary<string, AttributeReader> attributeReaders)
            => AttributeReaders = attributeReaders;

        /// <summary>
        /// Consumes the stream size and returns the contained attribute buffer.
        /// </summary>
        public bool TryRead(
            Stream stream,
            long size,
            AttributeDescriptor attributeDescriptor,
            out IAttributeBuffer attributeBuffer)
        {
            attributeBuffer = null;

            if (!AttributeReaders.TryGetValue(attributeDescriptor.Name, out var readFunc))
            {
                // Consume the stream and return if we do not have a matching read function.
                return stream.ReadFailure(size);
            }

            attributeBuffer = readFunc(stream, size);
            return attributeBuffer != null;
        }
    }
}
