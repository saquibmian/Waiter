using System;
using System.Linq;
using NUnit.Framework;
using Waiter.CommandLine;

namespace WaiterTests {
    [TestFixture]
    public class CommandLineParserTests {
        private CommandLineParser _parser;

        [SetUp]
        public void SetUp() {
            _parser = new CommandLineParser();
        }

        [Test]
        public void ParseCommandLineOptions_NullInput_ThrowsArgumentException() {
            Assert.Throws<ArgumentNullException>( () => _parser.ParseCommandLineOptions( null ) );
        }

        [Test]
        public void ParseCommandLineOptions_OddNumberOfPairs_ReturnsInvalidUsage() {
            var args = new[] {"first", "second", "third"};

            var response = _parser.ParseCommandLineOptions( args );

            Assert.AreEqual( false, response.Success );
            Assert.AreEqual( "Invalid usage", response.Errors.First() );
        }

        [Test]
        public void ParseCommandLineOptions_EvenNumberOfPairs_ReturnsSuccess() {
            var args = new[] {"-port", "10"};

            var response = _parser.ParseCommandLineOptions(args);

            Assert.AreEqual( true, response.Success );
            Assert.AreEqual( 10, response.Options.Port );
        }

        [Test]
        public void ParseCommandLineOptions_InvalidInput_ReturnsError() {
            var args = new[] {
                "-port", "hello!",
                "-timeout", "wow",
                "-requests", "noidea",
                "-method", "kill"
            };

            var response = _parser.ParseCommandLineOptions( args );

            Assert.AreEqual( false, response.Success );
            Assert.AreEqual( "hello! is not a valid port number", response.Errors.ElementAt( 0 ) );
            Assert.AreEqual( "wow is not a valid timeout", response.Errors.ElementAt( 1 ) );
            Assert.AreEqual( "noidea is not a valid number", response.Errors.ElementAt( 2 ) );
            Assert.AreEqual( "kill is not a valid http method", response.Errors.ElementAt( 3 ) );
        }

        [Test]
        public void ParseCommandLineOptions_ValidInput_ReturnsSuccess() {
            var args = new[] {
                "-port", "10", 
                "-timeout", "100", 
                "-requests", "100", 
                "-method", "GET"
            };

            var response = _parser.ParseCommandLineOptions( args );

            Assert.AreEqual( true, response.Success );
            Assert.AreEqual(10, response.Options.Port);
            Assert.AreEqual(100, response.Options.Timeout);
            Assert.AreEqual(100, response.Options.NumberOfRequests);
            Assert.AreEqual(HttpMethod.Get, response.Options.Method);
        }
    }
}
