using System;
using System.Collections.Generic;
using System.IO;
using Vim.BFast;

namespace Vim.G3d
{
    /// <summary>
    /// A function which reads an attribute from the given stream and size.
    /// </summary>
    public delegate IAttribute AttributeReader(Stream stream, long sizeInBytes);

    /// <summary>
    /// A collection of attributes and readers which can be used to deserialize attributes from a stream.<br/>
    /// <br/>
    /// A class may implement this interface to define the specialized set of attributes and attribute readers for 
    /// a given context.<br/>
    /// <br/>
    /// For example, the geometry and instance information in a VIM file is defined in a class named VimAttributeCollection.
    /// </summary>
    public interface IAttributeCollection
    {
        /// <summary>
        /// Returns the set of attribute names supported by the AttributeCollection.
        /// </summary>
        IEnumerable<string> AttributeNames { get; }

        /// <summary>
        /// A mapping from attribute name to its corresponding attribute.<br/>
        /// This is populated when reading attributes from a stream.
        /// </summary>
        IDictionary<string, IAttribute> Attributes { get; }

        /// <summary>
        /// A mapping from attribute name to its corresponding attribute reader.
        /// </summary>
        IDictionary<string, AttributeReader> AttributeReaders { get; }

        /// <summary>
        /// Validates the attribute collection. May throw an exception if the collection is invalid.
        /// </summary>
        void Validate();

        /// <summary>
        /// Returns the attribute corresponding to the given type.
        /// </summary>
        IAttribute GetAttribute(Type attributeType);
    }

    /// <summary>
    /// Extension functions and helpers for attribute collections
    /// </summary>
    public static class AttributeCollectionExtensions
    {
        /// <summary>
        /// Reads the attribute buffer and stores it among the Buffers.
        /// </summary>
        public static bool ReadAttribute(
            this IAttributeCollection attributeCollection,
            Stream stream,
            string name,
            long sizeInBytes)
        {
            if (name == null || !attributeCollection.AttributeReaders.TryGetValue(name, out var readAttribute))
                return stream.ReadFailure(sizeInBytes);

            attributeCollection.Attributes[name] = readAttribute(stream, sizeInBytes);

            return true;
        }

        /// <summary>
        /// Creates an attribute reader
        /// </summary>
        public static AttributeReader CreateAttributeReader<T, U>()
            where U : unmanaged
            where T : IAttribute<U>, new()
            => (stream, sizeInBytes) =>
            {
                var attributeBuffer = new T();
                var attributeDescriptor = attributeBuffer.AttributeDescriptor;
                var numElements = sizeInBytes / attributeDescriptor.DataElementSize;

                if (sizeInBytes % attributeDescriptor.DataElementSize != 0 ||
                    numElements > int.MaxValue)
                {
                    stream.ReadFailure(sizeInBytes); // consume the stream
                    return attributeBuffer;
                }

                var data = stream.ReadArray<U>((int)numElements);
                attributeBuffer.TypedData = data;

                return attributeBuffer;
            };
    }
}
