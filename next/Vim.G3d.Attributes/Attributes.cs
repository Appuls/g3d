using System.Collections.Generic;
using System.Linq;
using Vim.Math3d;

namespace Vim.G3d.Attributes
{
    [AttributeDescriptor("g3d:all:facesize:0:int32:1")] public partial class CornersPerFaceAttribute { }
    [AttributeDescriptor("g3d:vertex:position:0:float32:3", typeof(Vector3))] public partial class VertexAttribute { }
    [AttributeDescriptor("g3d:corner:index:0:int32:1")] public partial class IndexAttribute { }
    [AttributeDescriptor("g3d:instance:transform:0:float32:16", typeof(Matrix4x4))] public partial class InstanceTransformAttribute { }
    [AttributeDescriptor("g3d:instance:parent:0:int32:1")] public partial class InstanceParentAttribute { }
    [AttributeDescriptor("g3d:instance:mesh:0:int32:1")] public partial class InstanceMeshAttribute { }
    [AttributeDescriptor("g3d:mesh:submeshoffset:0:int32:1")] public partial class MeshSubmeshOffsetAttribute { }
    [AttributeDescriptor("g3d:submesh:indexoffset:0:int32:1")] public partial class SubmeshIndexOffsetAttribute { }
    [AttributeDescriptor("g3d:submesh:material:0:int32:1")] public partial class SubmeshMaterialAttribute { }
    [AttributeDescriptor("g3d:material:color:0:float32:4", typeof(Vector4))] public partial class MaterialColorAttribute { }
    [AttributeDescriptor("g3d:material:glossiness:0:float32:1")] public partial class MaterialGlossinessAttribute { }
    [AttributeDescriptor("g3d:material:smoothness:0:float32:1")] public partial class MaterialSmoothnessAttribute { }
    [AttributeDescriptor("g3d:shapevertex:position:0:float32:3", typeof(Vector3))] public partial class ShapeVertexAttribute { }
    [AttributeDescriptor("g3d:shape:vertexoffset:0:int32:1")] public partial class ShapeVertexOffsetAttribute { }
    [AttributeDescriptor("g3d:shape:color:0:float32:4", typeof(Vector4))] public partial class ShapeColorAttribute { }
    [AttributeDescriptor("g3d:shape:width:0:float32:1")] public partial class ShapeWidthAttribute { }

    [AttributeCollection(
        typeof(CornersPerFaceAttribute),
        typeof(VertexAttribute),
        typeof(IndexAttribute),
        typeof(InstanceTransformAttribute),
        typeof(InstanceParentAttribute),
        typeof(InstanceMeshAttribute),
        typeof(MeshSubmeshOffsetAttribute),
        typeof(SubmeshIndexOffsetAttribute),
        typeof(SubmeshMaterialAttribute),
        typeof(MaterialColorAttribute),
        typeof(MaterialGlossinessAttribute),
        typeof(MaterialSmoothnessAttribute),
        typeof(ShapeVertexAttribute),
        typeof(ShapeVertexOffsetAttribute),
        typeof(ShapeColorAttribute),
        typeof(ShapeWidthAttribute))]
    public partial class VimAttributeCollection
    {
        /// <summary>
        /// The number of corners per face. A value of 3 designates that all faces are triangles (a triangle has 3 corners).
        /// </summary>
        public int NumCornersPerFace
            => CornersPerFaceAttribute.TypedData?.FirstOrDefault() ?? 0;

        /// <summary>
        /// The total number of face corners
        /// </summary>
        public int NumCorners
            => IndexAttribute.TypedData?.Length ?? 0;

        /// <summary>
        /// The total number of vertices.
        /// </summary>
        public int NumVertices
            => VertexAttribute.TypedData?.Length ?? 0;
    }
}