using NUnit.Framework;
using System.IO;
using Vim.G3d.Attributes;

namespace Vim.G3d.Tests
{
    [TestFixture]
    public static class VimG3dTests
    {
        public static readonly string ProjectFolder = new DirectoryInfo(Properties.Resources.ProjDir.Trim()).FullName;
        public static string RootFolder = Path.Combine(ProjectFolder, "..");
        public static string TestInputFolder = Path.Combine(RootFolder, "data");
        public static string TestOutputFolder = Path.Combine(RootFolder, "data", "out");

        [Test]
        public static void UnexpectedG3dTest()
        {
            var testDir = Path.Combine(TestOutputFolder, nameof(UnexpectedG3dTest));
            
            // Prepare the test directory
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);

            var input = new FileInfo(Path.Combine(TestInputFolder, "unexpected.g3d"));
            // var output = Path.Combine(testDir, "unexpected.g3d");

            var vimAttributeCollection = new VimAttributeCollection();
            using var stream = input.OpenRead();

            var result = G3d.TryReadG3d(stream, vimAttributeCollection, out var g3d);
            Assert.IsTrue(result);

            // TODO: why can't this navigate to the generated code?
            var cornerAttribute = g3d.Attributes.Attributes[IndexAttribute.AttributeName];
            Assert.IsNotNull(cornerAttribute);
        }
    }
}