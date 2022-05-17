using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Vim.BFast;
using Vim.G3d.Attributes;

using static Vim.BFast.BFast;

namespace Vim.G3d.Tests
{
    [TestFixture]
    public static class VimG3dTests
    {
        public static readonly string ProjectFolder = new DirectoryInfo(Properties.Resources.ProjDir.Trim()).FullName;
        public static string RootFolder = Path.Combine(ProjectFolder, "..");
        public static string TestInputFolder = Path.Combine(RootFolder, "data");
        public static string TestOutputFolder = Path.Combine(RootFolder, "data", "out");

        public static string? PrepareTestDir([CallerMemberName] string? testName = null)
        {
            if (testName == null)
                throw new ArgumentException(nameof(testName));

            var testDir = Path.Combine(TestOutputFolder, testName);

            // Prepare the test directory
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);

            return testDir;
        }

        [Test]
        public static void UnexpectedG3dTest()
        {
            var testDir = PrepareTestDir();
            var input = new FileInfo(Path.Combine(TestInputFolder, "unexpected.g3d"));

            var vimAttributeCollection = new VimAttributeCollection();
            using var stream = input.OpenRead();

            var result = G3d.TryRead(stream, vimAttributeCollection, out var g3d);
            Assert.IsTrue(result);

            var cornerAttribute = g3d.Attributes.Attributes[IndexAttribute.AttributeName];
            Assert.IsNotNull(cornerAttribute);
        }

        [Test]
        public static void ReadVimTest()
        {
            var testDir = PrepareTestDir();
            var inputVim = new FileInfo(Path.Combine(TestInputFolder, "Mechanical_Room.r2017.vim"));

            using var stream = inputVim.OpenRead();

            G3d? g3d = null;
            stream.ReadBFast<object?>((s, name, size) =>
            {
                if (name == "geometry")
                    G3d.TryRead(s, new VimAttributeCollection(), out g3d);
                else
                    stream.Seek(size, SeekOrigin.Current);

                return null;
            });

            Assert.IsNotNull(g3d);
        }
    }
}