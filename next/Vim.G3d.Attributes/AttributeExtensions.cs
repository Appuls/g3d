using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Vim.Math3d;

namespace Vim.G3d.Attributes
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// Merges the given list of VimAttributeCollection into a new VimAttributeCollection.
        /// </summary>
        public static VimAttributeCollection Merge(this IReadOnlyList<VimAttributeCollection> collections)
        {
            if (collections.Count == 0)
                return new VimAttributeCollection();

            var firstCollection = collections.First();
            if (collections.Count == 1)
                return firstCollection;

            var result = new VimAttributeCollection();

            // Use the first collection's corners per face attribute in the result.
            result.CornersPerFaceAttribute = firstCollection.CornersPerFaceAttribute;

            // Merge the append-only attributes.
            var mergedAppendOnly = collections.MergeAppendOnlyAttributes();
            foreach (var mergedAttr in mergedAppendOnly)
                result.Attributes[mergedAttr.Name] = mergedAttr;

            // Merge the indexing attributes.
            var mergedIndexing = collections.MergeIndexingAttributes();
            foreach (var mergedAttr in mergedIndexing)
                result.Attributes[mergedAttr.Name] = mergedAttr;

            return result;
        }

        public static readonly IReadOnlyList<string> AppendOnlyAttributeNames = new[]
        {
            VertexAttribute.AttributeName,
            InstanceTransformAttribute.AttributeName,
            MaterialColorAttribute.AttributeName,
            MaterialGlossinessAttribute.AttributeName,
            MaterialSmoothnessAttribute.AttributeName,
            ShapeVertexAttribute.AttributeName,
            ShapeColorAttribute.AttributeName,
            ShapeWidthAttribute.AttributeName,
        };

        public static readonly IReadOnlyList<string> IndexingAttributes = new[]
        {
            IndexAttribute.AttributeName,
            InstanceParentAttribute.AttributeName,
            InstanceMeshAttribute.AttributeName,
            MeshSubmeshOffsetAttribute.AttributeName,
            SubmeshIndexOffsetAttribute.AttributeName,
            SubmeshMaterialAttribute.AttributeName,
            ShapeVertexOffsetAttribute.AttributeName,
        };

        /// <summary>
        /// Returns a grouping of the attributes by name.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IGrouping<string, (int CollectionIndex, IAttribute Attribute)>> GetGroupedAttributesByName(
            this IReadOnlyList<VimAttributeCollection> collections,
            IReadOnlyList<string> attributeNames)
        {
            var attributeNameSet = new HashSet<string>(attributeNames);

            return collections
                .SelectMany((collection, collectionIndex) => collection.Attributes.Values
                    .Where(attr => attributeNameSet.Contains(attr.Name))
                    .Select(attr => (collectionIndex, attr)))
                .GroupBy(t => t.attr.Name);
        }

        /// <summary>
        /// Merge the append-only attributes contained in the collections.
        /// </summary>
        public static IEnumerable<IAttribute> MergeAppendOnlyAttributes(this IReadOnlyList<VimAttributeCollection> collections)
        {
            var result = new List<IAttribute>();

            var groupedAttributes = collections.GetGroupedAttributesByName(AppendOnlyAttributeNames);
            foreach (var group in groupedAttributes)
            {
                var attributeName = group.Key;
                var attributes = group.OrderBy(t => t.CollectionIndex);

                IAttribute merged = null;
                switch (attributeName)
                {
                    case VertexAttribute.AttributeName:
                        merged = attributes.OfType<VertexAttribute>().ToList().MergeAppendOnlyAttributes<VertexAttribute, Vector3>();
                        break;
                    case InstanceTransformAttribute.AttributeName:
                        merged = attributes.OfType<InstanceTransformAttribute>().ToList().MergeAppendOnlyAttributes<InstanceTransformAttribute, Matrix4x4>();
                        break;
                    case MaterialColorAttribute.AttributeName:
                        merged = attributes.OfType<MaterialColorAttribute>().ToList().MergeAppendOnlyAttributes<MaterialColorAttribute, Vector4>();
                        break;
                    case MaterialGlossinessAttribute.AttributeName:
                        merged = attributes.OfType<MaterialGlossinessAttribute>().ToList().MergeAppendOnlyAttributes<MaterialGlossinessAttribute, float>();
                        break;
                    case MaterialSmoothnessAttribute.AttributeName:
                        merged = attributes.OfType<MaterialSmoothnessAttribute>().ToList().MergeAppendOnlyAttributes<MaterialSmoothnessAttribute, float>();
                        break;
                    case ShapeVertexAttribute.AttributeName:
                        merged = attributes.OfType<ShapeVertexAttribute>().ToList().MergeAppendOnlyAttributes<ShapeVertexAttribute, Vector3>();
                        break;
                    case ShapeColorAttribute.AttributeName:
                        merged = attributes.OfType<ShapeColorAttribute>().ToList().MergeAppendOnlyAttributes<ShapeColorAttribute, Vector4>();
                        break;
                    case ShapeWidthAttribute.AttributeName:
                        merged = attributes.OfType<ShapeWidthAttribute>().ToList().MergeAppendOnlyAttributes<ShapeWidthAttribute, float>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("No merge case found for append-only attribute", nameof(attributeName));
                }
                result.Add(merged);
            }

            return result;
        }

        /// <summary>
        /// Merges the append-only attributes.
        /// </summary>
        public static TAttr MergeAppendOnlyAttributes<TAttr, U>(this IReadOnlyList<TAttr> attributes) where TAttr : IAttribute<U>, new()
        {
            if (attributes.Count == 0)
                return new TAttr();

            // Check that all attributes have the same descriptor 
            var first = attributes.First();
            if (!attributes.All(attr => attr.Name.Equals(first.Name)))
                throw new Exception($"All attributes must have the same descriptor ({first.Name}) to be merged.");

            var data = attributes.SelectMany(attr => attr.TypedData).ToArray();
            return new TAttr { TypedData = data };
        }

        /// <summary>
        /// Merges the indexing attributes in the collection.
        /// </summary>
        public static IEnumerable<IAttribute> MergeIndexingAttributes(this IReadOnlyList<VimAttributeCollection> collections)
        {
            var result = new List<IAttribute>();

            foreach (var attributeName in IndexingAttributes)
            {
                IAttribute merged = null;
                switch (attributeName)
                {
                    case IndexAttribute.AttributeName:
                        // Merge the index attribute
                        // numVertices:               [X],       [Y],             [Z],                   ...
                        // valueOffsets:              [0],       [X],             [X+Y],                 ...
                        // indices:                   [A, B, C], [D,     E,   F], [G,         H,     I], ...
                        // mergedIndices:             [A, B, C], [X+D, X+E, X+F], [X+Y+G, X+Y+H, X+Y+I], ...
                        merged = collections.MergeIndexingAttribute(c => c.IndexAttribute, c => c.NumVertices);
                        break;
                    case InstanceParentAttribute.AttributeName:
                        throw new NotImplementedException();
                        break;
                    case InstanceMeshAttribute.AttributeName:
                        throw new NotImplementedException(); // TODO: IMPLEMENT ME
                        break;
                    case MeshSubmeshOffsetAttribute.AttributeName:
                        throw new NotImplementedException(); // TODO: IMPLEMENT ME
                        break;
                    case SubmeshIndexOffsetAttribute.AttributeName:
                        // numCorners:                [X],       [Y],           [Z],                 ...
                        // valueOffsets:              [0]        [X],           [X+Y],               ...
                        // submeshIndexOffsets:       [0, A, B], [0,   C,   D], [0,       E,     F], ...
                        // mergedSubmeshIndexOffsets: [0, A, B], [X, X+C, X+D], [X+Y, X+Y+E, X+Y+F], ...
                        merged = collections.MergeIndexingAttribute(c => c.SubmeshIndexOffsetAttribute, c => c.NumCorners);
                        break;
                    case SubmeshMaterialAttribute.AttributeName:
                        throw new NotImplementedException(); // TODO: IMPLEMENT ME
                        break;
                    case ShapeVertexOffsetAttribute.AttributeName:
                        throw new NotImplementedException(); // TODO: IMPLEMENT ME
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("No merge case found for indexing attribute", nameof(attributeName));
                }
                result.Add(merged);
            }

            return result;
        }

        /// <summary>
        /// Merges the attributes based on the given transformations.
        /// </summary>
        public static TAttr MergeAttributes<TAttr, U>(
            this IReadOnlyList<VimAttributeCollection> collections,
            Func<VimAttributeCollection, TAttr> getAttributeFunc,
            Func<(VimAttributeCollection Parent, TAttr Attribute)[], TAttr> mergeFunc)
            where TAttr : IAttribute<U>, new()
        {
            var tuples = collections
                .Select(c => (Parent: c, Attribute: getAttributeFunc(c)))
                .Where(tuple => tuple.Attribute != null)
                .ToArray();

            if (tuples.Length != collections.Count)
                throw new Exception("The attributes do not all correspond to the same attribute");

            return mergeFunc(tuples);
        }

        /// <summary>
        /// Merges the indexed attributes.
        /// </summary>
        public static TAttr MergeIndexingAttribute<TAttr>(
            this IReadOnlyList<VimAttributeCollection> collections,
            Func<VimAttributeCollection, TAttr> getIndexedAttributeFunc,
            Func<VimAttributeCollection, int> getValueOffsetFunc,
            int initialValueOffset = 0) where TAttr : IAttribute<int>, new()
        {
            var first = collections.FirstOrDefault();
            if (first == null)
                return default;

            var firstAttribute = getIndexedAttributeFunc(first);
            if (firstAttribute == null)
                return default;

            return collections.MergeAttributes<TAttr, int>(
                getIndexedAttributeFunc,
                tuples =>
                {
                    var valueOffset = initialValueOffset;
                    var mergedCount = 0;

                    var mergedData = new int[tuples.Sum(t => t.Attribute.TypedData.Length)];

                    foreach (var (parent, attr) in tuples)
                    {
                        var typedData = attr.TypedData;
                        var typedDataCount = typedData.Length;

                        for (var i = 0; i < typedDataCount; ++i)
                            mergedData[mergedCount + i] = typedData[i] + valueOffset;

                        mergedCount += typedDataCount;
                        valueOffset += getValueOffsetFunc(parent);
                    }

                    return new TAttr { TypedData = mergedData };
                });
        }
    }
}
