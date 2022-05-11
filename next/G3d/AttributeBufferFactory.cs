using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Vim.BFast;

namespace G3d
{
    public delegate IAttributeBuffer AttributeBufferReader(Stream stream, long size);

    public class AttributeBufferFactory
    {
        /// <summary>
        /// The mapping from attribute descriptor name to attribute reader.
        /// </summary>
        protected readonly IReadOnlyDictionary<string, AttributeBufferReader> AttributeReaders;

        ///// <summary>
        ///// Constructor.
        ///// </summary>
        //public AttributeBufferFactory(IReadOnlyDictionary<string, AttributeReader> attributeReaders)
        //    => AttributeReaders = attributeReaders;

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

        /// <summary>
        /// Creates an attribute reader
        /// </summary>
        public static AttributeBufferReader CreateAttributeBufferReader<T, U>()
            where U : unmanaged
            where T : IAttributeBuffer<U>, new()
        {
            return (stream, size) =>
            {
                var attributeBuffer = new T();
                var attributeDescriptor = attributeBuffer.AttributeDescriptor;
                var numElements = size / attributeDescriptor.DataElementSize;

                if (size % attributeDescriptor.DataElementSize != 0 ||
                    numElements > int.MaxValue)
                {
                    stream.ReadFailure(size); // consume the stream
                    return attributeBuffer;
                }

                var data = stream.ReadArray<U>((int)numElements);
                attributeBuffer.TypedData = data;

                return attributeBuffer;
            };
        }
    }
}
