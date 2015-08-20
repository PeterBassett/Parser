using NUnit.Framework;

namespace Parser.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ParserIsAbstractAndCannotBeInstanciated()
        {
            Assert.IsTrue(typeof(Parser).IsAbstract, "Parser base type should be abstract");
        }
    }
}
