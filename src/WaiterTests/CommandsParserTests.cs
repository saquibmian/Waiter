using NUnit.Framework;
using Waiter.CommandLine;
using Waiter.CommandLine.Parser;

namespace WaiterTests {
    
    [TestFixture]
    public class CommandsParserTests {
        private CommandsParser<CommandLineOptions> _parser;

        [SetUp]
        public void SetUp() {
            _parser = new CommandsParser<CommandLineOptions>();
        }

        [Test]
        public void Parse_ValidArgs_ReturnsTrue() {
            var args = new[] {"-port", "500", "-url", "http://hello/"};

            var result = _parser.Parse( args );

            Assert.IsTrue( result.Success );
        }

        [Test]
        public void Parse_InvalidPortType_ReturnsTrueAndSetsPortToDefault() {
            var args = new[] {"-port", "hello", "-url", "http://hello/"};

            var result = _parser.Parse( args );

            Assert.IsTrue( result.Success );
            Assert.AreEqual( 0, result.Options.Port );
        }

        [Test]
        public void Parse_InvalidMethodType_ReturnsTrueAndSetsMethodToDefault() {
            var args = new[] {"-method", "hello", "-url", "http://hello/"};

            var result = _parser.Parse( args );

            Assert.IsTrue( result.Success );
            Assert.AreEqual( HttpMethod.All, result.Options.Method );
        }

        [Test]
        public void Parse_OptionalWithNoValue_ReturnsTrueAndSetsMethodToDefault() {
            var args = new[] {"-method", "-url", "http://hello/"};

            var result = _parser.Parse( args );

            Assert.IsTrue( result.Success );
            Assert.AreEqual( HttpMethod.All, result.Options.Method );
        }
        [Test]
        public void Parse_NoArgs_ReturnsTrueAndSetsEverythingToDefault() {
            var args = new string[0];

            var result = _parser.Parse( args );

            Assert.IsTrue( result.Success );
            Assert.AreEqual(HttpMethod.All, result.Options.Method);
            Assert.AreEqual(0, result.Options.Port);
        }

    }
}
