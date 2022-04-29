using System;

namespace G3d.AttributeDescriptor
{
    public class AttributeDescriptor
    {
        public const string G3dPrefix = "g3d";
        public const string Separator = ":";
        public const char SeparatorChar = ':';

        /// <summary>
        /// The string representation of the AttributeDescriptor.
        /// <code>
        /// ex: "g3d:instance:transform:0:float32:16"
        ///      ~~~
        ///       |
        ///     "g3d" is the standard prefix.
        /// </code>
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The second token in the AttributeDescriptor; designates the object to
        /// which the attribute is conceptually associated
        /// <code>
        /// ex: "g3d:instance:transform:0:float32:16"
        ///          ~~~~~~~~
        /// </code>
        /// </summary>
        public readonly Association Association;

        /// <summary>
        /// The third token in the AttributeDescriptor; designates the semantic meaning of the data.
        /// <code>
        /// ex: "g3d:instance:transform:0:float32:16"
        ///                   ~~~~~~~~~
        /// </code>
        /// </summary>
        public readonly string Semantic;

        /// <summary>
        /// The fourth token in the AttributeDescriptor; designates the index in case
        /// the same Association:Semantic combination occurs more than once among the collection of attribute descriptors.
        /// <code>
        /// ex: "g3d:instance:transform:0:float32:16"
        ///                             ^
        /// </code>
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// The fifth token in the AttributeDescriptor; designates the data type which composes the buffer.
        /// <code>
        /// ex: "g3d:instance:transform:0:float32:16"
        ///                               ~~~~~~~
        ///                                  |
        /// a transformation matrix is composed of float32 values
        /// </code>
        /// </summary>
        public readonly DataType DataType;

        /// <summary>
        /// The sixth token in the AttributeDescriptor; designates arity, or the number of values which compose
        /// one semantic element.
        /// <code>
        /// ex: "g3d:instance:transform:0:float32:16"
        ///                                       ~~
        ///                                       |
        /// one transformation matrix is composed of 16 values
        /// </code>
        /// </summary>
        public readonly int DataArity;

        /// <summary>
        /// Constructor.
        /// <code>
        ///  association   semantic   dataType
        ///         |         |          |
        ///      ~~~~~~~~ ~~~~~~~~~   ~~~~~~~
        /// "g3d:instance:transform:0:float32:16"
        ///                         ^         ~~
        ///                         |         |
        ///                       index    dataArity
        /// </code>
        /// </summary>
        public AttributeDescriptor(
            Association association,
            string semantic,
            int index,
            DataType dataType,
            int dataArity)
        {
            Association = association;
            if (semantic.Contains(Separator))
                throw new Exception($"The semantic must not contain a '{Separator}' character");
            Semantic = semantic;
            DataType = dataType;
            DataArity = dataArity;
            Index = index;
            DataTypeSize = DataType.GetDataTypeSize();
            DataElementSize = DataTypeSize * DataArity;
            Name = $"{G3dPrefix}:{Association:G}:{Semantic}:{Index}:{DataType:G}:{DataArity}";
        }

        /// <summary>
        /// The size in bytes of the DataType.
        /// </summary>
        public readonly int DataTypeSize;

        /// <summary>
        /// The size in bytes of one semantic element (DataTypeSize * DataArity)
        /// </summary>
        public readonly int DataElementSize;

        /// <summary>
        /// Generates a URN representation of the attribute descriptor
        /// </summary>
        public override string ToString()
            => Name;

        /// <summary>
        /// Parses a URN representation of the attribute descriptor to generate an actual attribute descriptor 
        /// </summary>
        public static AttributeDescriptor Parse(string str)
        {
            var tokens = str.Split(SeparatorChar);
            if (tokens.Length != 6) throw new Exception("Expected 6 parts to the attribute descriptor");
            if (tokens[0] != G3dPrefix) throw new Exception($"First part of the attribute descriptor must be {G3dPrefix}");
            return new AttributeDescriptor(
                tokens[1].ParseAssociation(),
                tokens[2],
                int.Parse(tokens[3]),
                tokens[4].ParseDataType(),
                int.Parse(tokens[5]));
        }

        /// <summary>
        /// Validates the current attribute descriptor. Throws an exception if the validation fails.
        /// </summary>
        public bool Validate()
        {
            var str = ToString();
            var tmp = Parse(str);
            if (!str.Equals(tmp.ToString(), StringComparison.InvariantCulture))
                throw new Exception("Invalid attribute descriptor (or internal error in the parsing/string conversion");
            return true;
        }
    }
}
