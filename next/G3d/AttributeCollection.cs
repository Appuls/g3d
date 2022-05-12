using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vim.BFast;

namespace G3d
{
    public delegate IAttribute AttributeReader(Stream stream, long size);

    public class AttributeCollection
    {
        public readonly Dictionary<string, IAttribute> Attributes
            = new Dictionary<string, IAttribute>();

        public readonly Dictionary<string, AttributeReader> AttributeReaders
            = new Dictionary<string, AttributeReader>();

        /// <summary>
        /// Reads the attribute buffer and stores it among the Buffers.
        /// </summary>
        public bool ReadAttribute(Stream stream, string name, long size)
        {
            if (name == null ||
                !AttributeReaders.TryGetValue(name, out var bufferReader) ||
                Attributes.ContainsKey(name))
            {
                return stream.ReadFailure(size);
            }

            Attributes.Add(name, bufferReader(stream, size));
            return true;
        }

        /// <summary>
        /// Creates an attribute reader
        /// </summary>
        public static AttributeReader CreateAttributeReader<T, U>()
            where U : unmanaged
            where T : IAttribute<U>, new()
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
