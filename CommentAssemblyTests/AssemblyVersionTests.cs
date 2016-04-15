namespace CommentAssemblyTests
{
    using CommentAssembly;
    using NUnit.Framework;
    using System.IO;

    [TestFixture]
    public class AssemblyVersionTests
    {
        [TestCase("1..1.0")]
        [TestCase("-1.00.0.-1")]
        [TestCase("")]
        [TestCase("a.c.s.d")]
        public void AssemblyVersionConstructor_ThrowsFormatException_WhenCalledWithImproperString(string version)
        {
            var exception = Assert.Catch(() => new AssemblyVersion(version));

            StringAssert.Contains("Unable to parse", exception.Message);
        }

        [Test]
        public void AssemblyVersionOperatorIncrements_ThrowsOverflowException_WhenRevisionNumberIsTooHigh()
        {
            AssemblyVersion underTest = new AssemblyVersion(1, 1, 1, uint.MaxValue);

            var exception = Assert.Catch(() => underTest.Next());
        }

        [Test]
        public void AssemblyVersionConstructor_CreatesAGoodObject_WhenCalledWithProperString()
        {
            AssemblyVersion underTest = new AssemblyVersion("1.2.3.4");

            Assert.AreEqual(1, underTest.Major);
            Assert.AreEqual(2, underTest.Minor);
            Assert.AreEqual(3, underTest.Build);
            Assert.AreEqual(4, underTest.Revision);
        }

        [Test]
        public void AssemblyVersionOperatorIncrement_CorrectlyOperates_WhenCalledWithProperObject()
        {
            AssemblyVersion underTest = new AssemblyVersion("1.2.3.4").Next();

            Assert.AreEqual(1, underTest.Major);
            Assert.AreEqual(2, underTest.Minor);
            Assert.AreEqual(3, underTest.Build);
            Assert.AreEqual(5, underTest.Revision);

        }
    }
}
