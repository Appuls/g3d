using System;
using Vim.BFast;
using Vim.G3d;

namespace Vim.G3d.Attributes
{

    public partial class CornersPerFaceAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:all:facesize:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<CornersPerFaceAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class VertexAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:vertex:position:0:float32:3";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<VertexAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class IndexAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:corner:index:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<IndexAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class InstanceTransformAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:instance:transform:0:float32:16";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<InstanceTransformAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class InstanceParentAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:instance:parent:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<InstanceParentAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class InstanceMeshAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:instance:mesh:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<InstanceMeshAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MeshSubmeshOffsetAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:mesh:submeshoffset:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<MeshSubmeshOffsetAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class SubmeshIndexOffsetAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:submesh:indexoffset:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<SubmeshIndexOffsetAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class SubmeshMaterialAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:submesh:material:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<SubmeshMaterialAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MaterialColorAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:material:color:0:float32:4";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<MaterialColorAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MaterialGlossinessAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:material:glossiness:0:float32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<MaterialGlossinessAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class MaterialSmoothnessAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:material:smoothness:0:float32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<MaterialSmoothnessAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeVertexAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:shapevertex:position:0:float32:3";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<ShapeVertexAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeVertexOffsetAttribute : IAttribute<System.Int32>
    {
        public const string AttributeName = "g3d:shape:vertexoffset:0:int32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<ShapeVertexOffsetAttribute, System.Int32>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Int32[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeColorAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:shape:color:0:float32:4";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<ShapeColorAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class ShapeWidthAttribute : IAttribute<System.Single>
    {
        public const string AttributeName = "g3d:shape:width:0:float32:1";

        public string Name
            => AttributeName;

        public static AttributeReader CreateAttributeReader()
            => AttributeCollection.CreateAttributeReader<ShapeWidthAttribute, System.Single>();

        public IAttributeDescriptor AttributeDescriptor { get; }
            = new AttributeDescriptor(AttributeName);

        public System.Single[] TypedData { get; set; }

        public Array Data => TypedData;
    }

    public partial class VimAttributeCollection
    {
        public VimAttributeCollection()
        {
            AttributeReaders.Add(CornersPerFaceAttribute.AttributeName, CornersPerFaceAttribute.CreateAttributeReader());
            AttributeReaders.Add(VertexAttribute.AttributeName, VertexAttribute.CreateAttributeReader());
            AttributeReaders.Add(IndexAttribute.AttributeName, IndexAttribute.CreateAttributeReader());
            AttributeReaders.Add(InstanceTransformAttribute.AttributeName, InstanceTransformAttribute.CreateAttributeReader());
            AttributeReaders.Add(InstanceParentAttribute.AttributeName, InstanceParentAttribute.CreateAttributeReader());
            AttributeReaders.Add(InstanceMeshAttribute.AttributeName, InstanceMeshAttribute.CreateAttributeReader());
            AttributeReaders.Add(MeshSubmeshOffsetAttribute.AttributeName, MeshSubmeshOffsetAttribute.CreateAttributeReader());
            AttributeReaders.Add(SubmeshIndexOffsetAttribute.AttributeName, SubmeshIndexOffsetAttribute.CreateAttributeReader());
            AttributeReaders.Add(SubmeshMaterialAttribute.AttributeName, SubmeshMaterialAttribute.CreateAttributeReader());
            AttributeReaders.Add(MaterialColorAttribute.AttributeName, MaterialColorAttribute.CreateAttributeReader());
            AttributeReaders.Add(MaterialGlossinessAttribute.AttributeName, MaterialGlossinessAttribute.CreateAttributeReader());
            AttributeReaders.Add(MaterialSmoothnessAttribute.AttributeName, MaterialSmoothnessAttribute.CreateAttributeReader());
            AttributeReaders.Add(ShapeVertexAttribute.AttributeName, ShapeVertexAttribute.CreateAttributeReader());
            AttributeReaders.Add(ShapeVertexOffsetAttribute.AttributeName, ShapeVertexOffsetAttribute.CreateAttributeReader());
            AttributeReaders.Add(ShapeColorAttribute.AttributeName, ShapeColorAttribute.CreateAttributeReader());
            AttributeReaders.Add(ShapeWidthAttribute.AttributeName, ShapeWidthAttribute.CreateAttributeReader());
        }
    }

}
