using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Vim.G3d.Attributes
{
    public static class AttributeExtensions
    {
        /* TODO: IMPLEMENT MERGING LOGIC PER KNOWN ATTRIBUTE
        public static VimAttributeCollection Merge(this IReadOnlyList<VimAttributeCollection> collections)
        {
            if (collections.Count == 0)
                return new VimAttributeCollection();

            var firstCollection = collections.First();

            if (collections.Count == 1)
                return firstCollection;

            var numCornersPerFace = firstCollection.NumCornersPerFace;
            if (!collections.All(g => g.NumCornersPerFace == numCornersPerFace))
                throw new Exception("Cannot merge meshes with different numbers of corners per faces");

            // Merge all of the attributes of the different geometries
            // Except: indices, group indexes, subgeo, and instance attributes
            var attributes = firstCollection.Attributes.Values.Where(a =>
            {
                switch (a)
                {
                    case VertexAttribute _:
                    case SubmeshMaterialAttribute _:
                        return true;
                    

                    default:
                        return false;
                }
            })
            .ToList();
            // OLD:
            //var attributes = first.VertexAttributes()
            //    .Concat(first.CornerAttributes())
            //    .Concat(first.EdgeAttributes())
            //    .Concat(first.NoneAttributes())
            //    .Concat(first.FaceAttributes())
            //    .Append(first.GetAttributeSubmeshMaterial())
            //    // Skip the index semantic because things get re-ordered
            //    .Where(attr => attr != null && attr.Descriptor.Semantic != Semantic.Index)
            //    .ToArray();

            // Merge the non-indexed attributes
            var others = collections.Skip(1);
            var attributeList = attributes.Select(attr =>
                attr.Merge(others.Select(c =>
                    c.Attributes.TryGetValue(attr.Name, out var a) ? a : null)
                .Where(a => a != null)
                .ToList()));

            // Merge the index attribute
            // numVertices:               [X],       [Y],             [Z],                   ...
            // valueOffsets:              [0],       [X],             [X+Y],                 ...
            // indices:                   [A, B, C], [D,     E,   F], [G,         H,     I], ...
            // mergedIndices:             [A, B, C], [X+D, X+E, X+F], [X+Y+G, X+Y+H, X+Y+I], ...
            var mergedIndexAttribute = collections.MergeIndexedAttribute(
                c => c.IndexAttribute,
                c => c.NumVertices);

            if (mergedIndexAttribute != null)
                attributeList.Add(mergedIndexAttribute);

            // Merge the submesh index offset attribute
            // numCornersPerFace:         [X],       [Y],           [Z],                 ...
            // valueOffsets:              [0]        [X],           [X+Y],               ...
            // submeshIndexOffsets:       [0, A, B], [0,   C,   D], [0,       E,     F], ...
            // mergedSubmeshIndexOffsets: [0, A, B], [X, X+C, X+D], [X+Y, X+Y+E, X+Y+F], ...
            var mergedSubmeshIndexOffsetAttribute = collections.MergeIndexedAttribute(
                    c => c.SubmeshIndexOffsetAttribute,
                    c => c.NumCornersPerFace);
            if (mergedSubmeshIndexOffsetAttribute != null)
                attributeList.Add(mergedSubmeshIndexOffsetAttribute);

            return attributeList.ToGeometryAttributes();
        }
        */

        /// <summary>
        /// Merges the attributes based on the given transformations and returns an array of merged values.
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
        public static TAttr MergeIndexedAttribute<TAttr>(
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

        public static TAttr MergeAttributes<TAttr, U>(this IReadOnlyList<TAttr> attributes)
            where TAttr : IAttribute<U>, new()
        {
            // TODO: Improve this precondition
            var t = typeof(TAttr);
            if (t == typeof(IndexAttribute) ||
                t == typeof(MeshSubmeshOffsetAttribute) ||
                t == typeof(InstanceMeshAttribute) ||
                t == typeof(InstanceParentAttribute) ||
                t == typeof(InstanceTransformAttribute))
            {
                throw new ArgumentException($"Unable to merge attributes of type {typeof(TAttr)}");
            }

            if (attributes.Count == 0)
                return new TAttr();

            // Check that all attributes have the same descriptor 
            var first = attributes.First();
            if (!attributes.All(attr => attr.Name.Equals(first.Name)))
                throw new Exception($"All attributes must have the same descriptor ({first.Name}) to be merged.");

            var data = attributes.SelectMany(attr => attr.TypedData).ToArray();
            return new TAttr { TypedData = data };
        }
    }
}
