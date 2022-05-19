﻿using System.IO;
using Vim.BFast;
using System.Linq;
using System.Collections.Generic;

namespace Vim.G3d
{
    /// <summary>
    /// A G3d is composed of a header and a collection of attributes containing descriptors and their data.
    /// </summary>
    public class G3d
    {
        /// <summary>
        /// The header of the G3d. Corresponds to the "meta" segment.
        /// </summary>
        public readonly Header Header;

        /// <summary>
        /// The attributes of the G3d.
        /// </summary>
        public readonly AttributeCollection Attributes;

        /// <summary>
        /// Constructor.
        /// </summary>
        public G3d(Header header, AttributeCollection attributes)
        {
            Header = header;
            Attributes = attributes;
        }

        /// <summary>
        /// Constructor. Uses the default header.
        /// </summary>
        public G3d(AttributeCollection attributes) 
            : this(Header.Default, attributes)
        { }

        /// <summary>
        /// Reads the stream using the attribute collection's readers and outputs a G3d upon success.
        /// </summary>
        public static bool TryRead(Stream stream, AttributeCollection collection, out G3d g3d)
        {
            g3d = null;
            Header? header = null;

            object OnG3dSegment(Stream s, string name, long size)
            {
                if (Header.IsSegmentHeader(name, size) && Header.TryRead(s, size, out var outHeader))
                {
                    // Assign to the header variable in the closure.
                    return header = outHeader;
                }

                // The segment is not the header so treat it as an attribute.
                return collection.ReadAttribute(s, name, size);
            }

            _ = stream.ReadBFast(OnG3dSegment);

            // Failure case if the header was not found.
            if (!header.HasValue)
                return false;

            // Instantiate the object and return.
            g3d = new G3d(header.Value, collection);
            return true;
        }

        /// <summary>
        /// Returns the G3d BFast header information, including buffer names and buffer sizes in bytes.
        /// </summary>
        private static (BFastHeader BFastHeader, string[] BufferNames, long[] BufferSizesInBytes ) GetBFastHeaderInfo(
            Header header,
            IReadOnlyList<IAttribute> attributes)
        {
            var nameList = new List<string>();
            var sizesInBytesList = new List<long>();

            var metaBuffer = header.ToBytes().ToNamedBuffer(Header.SegmentName);
            nameList.Add(metaBuffer.Name);
            sizesInBytesList.Add(metaBuffer.NumBytes());

            foreach (var attribute in attributes)
            {
                nameList.Add(attribute.Name);
                sizesInBytesList.Add(attribute.GetSizeInBytes());
            }

            var bufferNames = nameList.ToArray();
            var bufferSizesInBytes = sizesInBytesList.ToArray();

            return (
                BFast.BFast.CreateBFastHeader(bufferSizesInBytes, bufferNames),
                bufferNames,
                bufferSizesInBytes
            );
        }

        private delegate void StreamWriter(Stream stream);

        /// <summary>
        /// Writes the G3d to the given stream.
        /// </summary>
        public void Write(Stream stream)
        {
            var metaBuffer = Header.ToBytes().ToNamedBuffer(Header.SegmentName);
            var attributes = Attributes.Attributes.Values.OrderBy(n => n.Name).ToArray(); // Order the attributes by name for consistency

            // Prepare the bfast header, which describes the names and ranges.
            var (bfastHeader, bufferNames, bufferSizesInBytes) = GetBFastHeaderInfo(Header, attributes);

            // Prepare the stream writers.
            var streamWriters = new StreamWriter[1 + attributes.Length];
            streamWriters[0] = s => s.Write(metaBuffer); // First stream writer is the "meta" buffer.
            for (int i = 0; i < attributes.Length; i++)
                streamWriters[i + 1] = s => attributes[i].Write(s);

            stream.WriteBFastHeader(bfastHeader);
            stream.WriteBFastBody(bfastHeader, bufferNames, bufferSizesInBytes, (s, index, name, size) =>
            {
                streamWriters[index](s);
                return size;
            });
        }
    }
}