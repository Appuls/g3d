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
        private string _name;
        public string Name
            => _name ?? (_name = $"{G3dPrefix}:{AssociationStr}:{Semantic}:{IndexStr}:{DataTypeStr}:{DataArityStr}");

        /// <summary>
        /// The second token in the AttributeDescriptor; designates the object to
        /// which the attribute is conceptually associated
        /// <code>
        /// ex: "g3d:instance:transform:0:float32:16"
        ///          ~~~~~~~~
        /// </code>
        /// </summary>
        public readonly Association Association;
        public readonly string AssociationStr;

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
        public readonly string IndexStr;

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
        public readonly string DataTypeStr;

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
        public readonly string DataArityStr;

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
            if (association == Association.unknown)
                throw new ArgumentException($"The association cannot be '{Association.unknown}'.");

            if (dataType == DataType.unknown)
                throw new ArgumentException($"The data type cannot be '{DataType.unknown}'.");

            if (string.IsNullOrWhiteSpace(semantic))
                throw new ArgumentException($"The semantic cannot be null or whitespace.");

            if (semantic.Contains(Separator))
                throw new ArgumentException($"The semantic must not contain a '{Separator}' character");

            Association = association;
            AssociationStr = Association.ToString("G");

            Semantic = semantic;

            Index = index;
            IndexStr = Index.ToString();

            DataType = dataType;
            DataTypeStr = DataType.ToString("G");

            DataArity = dataArity;
            DataArityStr = DataArity.ToString();

            (DataTypeSize, DataElementSize) = GetDataSizes(DataType, DataArity);
        }

        /// <summary>
        /// Constructor. Parses an input string of the form: "g3d:instance:transform:0:float32:16".
        /// </summary>
        public AttributeDescriptor(string str)
        {
            // TODO: this should be code-generated to let the consuming library declare (and expand) its associations.

            var tokens = str.Split(SeparatorChar);
            
            if (tokens.Length != 6)
            {
                Errors = AttributeDescriptorErrors.UnexpectedNumberOfTokens;
                return;
            }

            if (tokens[0] != G3dPrefix)
            {
                Errors = AttributeDescriptorErrors.PrefixError;
                return;
            }

            AssociationStr = tokens[1];
            if (!Enum.TryParse(AssociationStr, out Association) || Association == Association.unknown)
            {
                Association = Association.unknown;
                Errors |= AttributeDescriptorErrors.AssociationError;
                // do not return; there may be more errors.
            }

            Semantic = tokens[2];
            if (string.IsNullOrWhiteSpace(Semantic))
            {
                Errors |= AttributeDescriptorErrors.SemanticError;
                // do not return; there may be more errors.
            }

            IndexStr = tokens[3];
            if (!int.TryParse(IndexStr, out Index))
            {
                Errors |= AttributeDescriptorErrors.IndexError;
                // do not return; there may be more errors.
            }

            DataTypeStr = tokens[4];
            if (!Enum.TryParse(DataTypeStr, out DataType) || DataType == DataType.unknown)
            {
                DataType = DataType.unknown;
                Errors |= AttributeDescriptorErrors.DataTypeError;
                // do not return; there may be more errors.
            }

            DataArityStr = tokens[5];
            if (!int.TryParse(DataArityStr, out DataArity))
            {
                Errors |= AttributeDescriptorErrors.DataArityError;
            }

            if (!HasDataTypeError && !HasDataArityError)
            {
                (DataTypeSize, DataElementSize) = GetDataSizes(DataType, DataArity);
            }
        }

        public static (int dataTypeSize, int dataElementSize) GetDataSizes(DataType dataType, int dataArity)
        {
            var dataTypeSize = dataType.GetDataTypeSize();
            return (dataTypeSize, dataTypeSize * dataArity);
        }

        /// <summary>
        /// The current error if IsValid is false.
        /// </summary>
        public readonly AttributeDescriptorErrors Errors;

        /// <summary>
        /// Returns true if the Errors contains a data type error.
        /// </summary>
        public bool HasDataTypeError
            => (Errors & AttributeDescriptorErrors.DataTypeError) == AttributeDescriptorErrors.DataTypeError;

        /// <summary>
        /// Returns true if the Errors contains a data arity error.
        /// </summary>
        public bool HasDataArityError
            => (Errors & AttributeDescriptorErrors.DataArityError) == AttributeDescriptorErrors.DataArityError;

        /// <summary>
        /// Returns true if hte attribute descriptor was successfully parsed and its data types were recognized.
        /// </summary>
        public bool IsValid
            => Errors == AttributeDescriptorErrors.None;

        /// <summary>
        /// The size in bytes of the DataType.
        /// </summary>
        public readonly int DataTypeSize;

        /// <summary>
        /// The size in bytes of one semantic element (DataTypeSize * DataArity)
        /// </summary>
        public readonly int DataElementSize;

        /// <summary>
        /// Returns the string representation of the attribute descriptor, in the form "g3d:instance:transform:0:float32:16"
        /// </summary>
        public override string ToString()
            => Name;

        // TODO: Convert this to a unit test
        ///// <summary>
        ///// Validates the current attribute descriptor. Throws an exception if the validation fails.
        ///// </summary>
        //public bool Validate()
        //{
        //    var str = ToString();
        //    var tmp = Parse(str);
        //    if (!str.Equals(tmp.ToString(), StringComparison.InvariantCulture))
        //        throw new Exception("Invalid attribute descriptor (or internal error in the parsing/string conversion");
        //    return true;
        //}
    }
}
