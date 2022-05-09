using System;
using G3d;

namespace Vim.G3d
{
    public partial class VertexBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:vertex:position:0:float32:3");
    }

    public partial class IndexBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:corner:index:0:int32:1");
    }

    public partial class InstanceTransformBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:instance:transform:0:float32:16");
    }

    public partial class InstanceParentBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:instance:parent:0:int32:1");
    }

    public partial class InstanceMeshBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:instance:mesh:0:int32:1");
    }

    public partial class MeshSubmeshOffsetBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:mesh:submeshoffset:0:int32:1");
    }

    public partial class SubmeshIndexOffsetBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:submesh:indexoffset:0:int32:1");
    }

    public partial class SubmeshMaterialBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:submesh:material:0:int32:1");
    }

    public partial class MaterialColorBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:material:color:0:float32:4");
    }

    public partial class MaterialGlossinessBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:material:glossiness:0:float32:1");
    }

    public partial class MaterialSmoothnessBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:material:smoothness:0:float32:1");
    }

    public partial class ShapeVertexBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:shapevertex:position:0:float32:3");
    }

    public partial class ShapeVertexOffsetBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:shape:vertexoffset:0:int32:1");
    }

    public partial class ShapeColorBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:shape:color:0:float32:4");
    }

    public partial class ShapeWidthBuffer : IAttributeBuffer
    {
        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptorBase("g3d:shape:width:0:float32:1");
    }
}
