using CommentAssembly;
using NUnit.Framework;
using System;
using System.IO;

namespace CommentAssemblyTests
{
    [TestFixture]
    class AssemblyInfoTest
    {
        [TestCase("0.1.0.941", "// [assembly: AssemblyVersion(\"0.1.* \")]\n[assembly: AssemblyVersion(\"0.1.0.941\")]\n[assembly: AssemblyFileVersion(\"0.1.0.941\")]\n////  \"0.1.0.941\"  Compiled 11.02.2016 20:26]")]
        [TestCase("0.1.0.941", "// [assembly: AssemblyVersion(\"0.1.* \")]\n[assembly: AssemblyVersion(\"0.1.0.941\")]\n[assembly: AssemblyFileVersion(\"0.1.0.941\")]")]
        public void AssemblyInfo_IsCorrectlyInstantiated_WithProperFile(string expectedVersion, string assemblyFile)
        {
            AssemblyVersion expected = new AssemblyVersion(expectedVersion);

            TextReader reader = new StringReader(assemblyFile);
            AssemblyInfoProcessor assemblyInfo = new AssemblyInfoProcessor(reader);

            Assert.AreEqual(expected.Major , assemblyInfo.CurrentVersion.Major);
            Assert.AreEqual(expected.Minor, assemblyInfo.CurrentVersion.Minor);
            Assert.AreEqual(expected.Build, assemblyInfo.CurrentVersion.Build);
            Assert.AreEqual(expected.Revision, assemblyInfo.CurrentVersion.Revision);
        }

        [TestCase(new string[] { "Comment1","Comment2" }, "// [assembly: AssemblyVersion(\"0.1.* \")]\n[assembly: AssemblyVersion(\"0.1.0.941\")]\n[assembly: AssemblyFileVersion(\"0.1.0.941\")]\nComment1\nComment2")]
        [TestCase(new string[] { "Comment1", "Comment2" }, "// [assembly: AssemblyVersion(\"0.1.* \")]\n[assembly: AssemblyVersion(\"0.1.0.941\")]\n[assembly: AssemblyFileVersion(\"0.1.0.941\")]\n\nComment1\nComment2")]
        [TestCase(new string[] { }, "// [assembly: AssemblyVersion(\"0.1.* \")]\n[assembly: AssemblyVersion(\"0.1.0.941\")]\n[assembly: AssemblyFileVersion(\"0.1.0.941\")]\n")]
        public void Assemblyinfo_CorrectlyLoads_TheLastCommensts(string[] expected, string assemblyFile)
        {
            TextReader reader = new StringReader(assemblyFile);
            AssemblyInfoProcessor assemblyInfo = new AssemblyInfoProcessor(reader);

            Assert.AreEqual(expected, assemblyInfo.LastComments);
        }

        [Test]
        public void AssemblyInfo_ThrowsFileFormatException_IfNoVersionIsContainedÎnFile()
        {
            TextReader reader = new StringReader("// [assembly: AssemblyVersion(\"0.1.* \")]\n[assembly: AssemblyFileVersion(\"0.1.0.941\")]\nComment1\nComment2");

            Exception exception = Assert.Catch(() => new AssemblyInfoProcessor(reader));
            StringAssert.Contains("not contain", exception.Message);
        }

        [Test]
        public void AssemblyInfo_ThrowsFileFormatException_IfTwoVersionsAreContainedInFile()
        {
            TextReader reader = new StringReader("// [assembly: AssemblyVersion(\"0.1.* \")]\n[assembly: AssemblyVersion(\"0.1.0.941\")]\n[assembly: AssemblyFileVersion(\"0.1.0.941\")]\nComment1\nComment2\n[assembly: AssemblyVersion(\"0.1.0.941\")]\n");

            Exception exception = Assert.Catch(() => new AssemblyInfoProcessor(reader));
            StringAssert.Contains("more than one", exception.Message);
        }
    }
}
