using G3d;

namespace Vim.G3d
{
    [AttributeDescriptor("g3d:vertex:position:0:float32:3")] public partial class VertexBuffer { }
    [AttributeDescriptor("g3d:corner:index:0:int32:1")] public partial class IndexBuffer { }
    [AttributeDescriptor("g3d:instance:transform:0:float32:16")] public partial class InstanceTransformBuffer { }
    [AttributeDescriptor("g3d:instance:parent:0:int32:1")] public partial class InstanceParentBuffer { }
    [AttributeDescriptor("g3d:instance:mesh:0:int32:1")] public partial class InstanceMeshBuffer { }
    [AttributeDescriptor("g3d:mesh:submeshoffset:0:int32:1")] public partial class MeshSubmeshOffsetBuffer { }
    [AttributeDescriptor("g3d:submesh:indexoffset:0:int32:1")] public partial class SubmeshIndexOffsetBuffer { }
    [AttributeDescriptor("g3d:submesh:material:0:int32:1")] public partial class SubmeshMaterialBuffer { }
    [AttributeDescriptor("g3d:material:color:0:float32:4")] public partial class MaterialColorBuffer { }
    [AttributeDescriptor("g3d:material:glossiness:0:float32:1")] public partial class MaterialGlossinessBuffer { }
    [AttributeDescriptor("g3d:material:smoothness:0:float32:1")] public partial class MaterialSmoothnessBuffer { }
    [AttributeDescriptor("g3d:shapevertex:position:0:float32:3")] public partial class ShapeVertexBuffer { }
    [AttributeDescriptor("g3d:shape:vertexoffset:0:int32:1")] public partial class ShapeVertexOffsetBuffer { }
    [AttributeDescriptor("g3d:shape:color:0:float32:4")] public partial class ShapeColorBuffer { }
    [AttributeDescriptor("g3d:shape:width:0:float32:1")] public partial class ShapeWidthBuffer { }
}