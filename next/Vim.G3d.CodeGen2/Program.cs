﻿
namespace Vim.G3d.CodeGen2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var file = args[0];
            
            G3dAttributeCollectionGenerator.WriteDocument(file);
        }
    }
}