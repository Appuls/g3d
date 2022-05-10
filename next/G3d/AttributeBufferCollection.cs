using System.Collections.Generic;
using System.IO;

namespace G3d
{
    public class AttributeBufferCollection
    {
        private readonly AttributeBufferFactory _factory;

        private readonly Dictionary<IAttributeDescriptor, IAttributeBuffer> _attributeBuffers
            = new Dictionary<IAttributeDescriptor, IAttributeBuffer>();

        public IReadOnlyDictionary<IAttributeDescriptor, IAttributeBuffer> AttributeBuffers
            => _attributeBuffers;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AttributeBufferCollection(AttributeBufferFactory factory)
            => _factory = factory;

        public bool ReadAttributeBuffer(Stream stream, string name, long size)
        {
            if (!AttributeDescriptor.TryParse(name, out var attributeDescriptor))
                return stream.ReadFailure(size); // Skip unknown attribute descriptors.

            if (!_factory.TryRead(stream, size, attributeDescriptor, out var attributeBuffer))
                return false; // Simply return false - the stream is consumed in TryRead.

            _attributeBuffers.Add(attributeDescriptor, attributeBuffer);
            return true;
        }
    }
}
