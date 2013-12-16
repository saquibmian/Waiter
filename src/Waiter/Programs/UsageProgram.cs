using System;
using Waiter.CommandLine;

namespace Waiter.Programs {
    internal class UsageProgram : Program {
        internal new static void Main( string[] args ) {
            var parser = new CommandsParser<CommandLineOptions>();
            Console.WriteLine( parser.Usage );
            Exit( 0 );
        }
    }
}
