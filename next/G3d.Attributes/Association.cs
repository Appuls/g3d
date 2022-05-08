using System;

namespace G3d.AttributeDescriptor
{
    public enum Association
    {
        /// <summary>
        /// Exists to support future values which are currently unknown.
        /// </summary>
        unknown,  

        // IMPORTANT: the string values of these associations, including their capitalization,
        // are used in the parsing mechanism of the AttributeDescriptor, so be careful!
        all,      // associated with all data in G3d
        none,     // no association
        vertex,   // vertex or point data 
        face,     // face associated data
        corner,   // corner (aka face-vertex) data. A corner is associated with one vertex, but a vertex may be shared between multiple corners  
        edge,     // half-edge data. Each face consists of n half-edges, one per corner. A half-edge, is a directed edge
        instance, // instance information 
        shapevertex, // flattened shape vertex collection.
        shape,    // shape instance
        material, // material properties
        mesh,     // mesh association (a mesh contains submeshes which correspond to slices of materials)
        submesh,  // submesh association (a submesh has indices, vertices, and a material)
    }
}