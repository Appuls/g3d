using System;
using G3d;
using Vim.BFast;

namespace Vim.G3d
{

    public partial class VertexBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<VertexBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:vertex:position:0:float32:3");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class IndexBuffer : IAttributeBuffer<System.Int32>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<IndexBuffer, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:corner:index:0:int32:1");

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class InstanceTransformBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<InstanceTransformBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:instance:transform:0:float32:16");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class InstanceParentBuffer : IAttributeBuffer<System.Int32>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<InstanceParentBuffer, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:instance:parent:0:int32:1");

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class InstanceMeshBuffer : IAttributeBuffer<System.Int32>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<InstanceMeshBuffer, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:instance:mesh:0:int32:1");

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MeshSubmeshOffsetBuffer : IAttributeBuffer<System.Int32>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<MeshSubmeshOffsetBuffer, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:mesh:submeshoffset:0:int32:1");

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class SubmeshIndexOffsetBuffer : IAttributeBuffer<System.Int32>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<SubmeshIndexOffsetBuffer, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:submesh:indexoffset:0:int32:1");

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class SubmeshMaterialBuffer : IAttributeBuffer<System.Int32>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<SubmeshMaterialBuffer, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:submesh:material:0:int32:1");

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MaterialColorBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<MaterialColorBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:material:color:0:float32:4");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MaterialGlossinessBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<MaterialGlossinessBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:material:glossiness:0:float32:1");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MaterialSmoothnessBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<MaterialSmoothnessBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:material:smoothness:0:float32:1");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeVertexBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<ShapeVertexBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:shapevertex:position:0:float32:3");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeVertexOffsetBuffer : IAttributeBuffer<System.Int32>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<ShapeVertexOffsetBuffer, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:shape:vertexoffset:0:int32:1");

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeColorBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<ShapeColorBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:shape:color:0:float32:4");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeWidthBuffer : IAttributeBuffer<System.Single>
    {
        public static AttributeBufferReader CreateAttributeBufferReader()
            => AttributeBufferFactory.CreateAttributeBufferReader<ShapeWidthBuffer, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor("g3d:shape:width:0:float32:1");

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

}
