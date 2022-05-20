using System;
using System.IO;

namespace Vim.G3d
{
    public interface IAttribute
    {
        string Name { get; }
        IAttributeDescriptor AttributeDescriptor { get; }
        Array Data { get; }
        void Write(Stream stream);
    }

    public interface IAttribute<T> : IAttribute
    {
        T[] TypedData { get; set; }
    }

    public static class AttributeExtensions
    {
        public static long GetSizeInBytes(this IAttribute attribute)
            => attribute.AttributeDescriptor.DataElementSize * (attribute.Data?.Length ?? 0);
    }
}
