using System;

namespace G3d.AttributeDescriptor
{
    public enum DataType
    {
        // IMPORTANT: the string values of these data types, including their capitalization,
        // are used in the parsing mechanism of the AttributeDescriptor, so be careful!
        int8,
        int16,
        int32,
        int64,
        float32,
        float64,
    };

    public static class DataTypeExtensions
    {
        /// <summary>
        /// Parses the given string and attempts to return the corresponding data type.
        /// </summary>
        public static DataType ParseDataType(this string str, DataType @default = DataType.int8)
            => Enum.TryParse(str, out DataType dataType)
                ? dataType
                : @default;

        /// <summary>
        /// Returns the size in bytes of the given data type.
        /// </summary>
        public static int GetDataTypeSize(this DataType dt)
        {
            switch (dt)
            {
                case DataType.int8: return 1;
                case DataType.int16: return 2;
                case DataType.int32: return 4;
                case DataType.int64: return 8;
                case DataType.float32: return 4;
                case DataType.float64: return 8;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dt), dt, null);
            }
        }
    }
}
