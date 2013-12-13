using System.Collections.Generic;

namespace Waiter.CommandLine {
    internal class CommandLineParserReponse {
        internal bool Success { get; set; }
        internal IEnumerable<string> Errors { get; set; }
        internal CommandLineOptions Options { get; set; }

        internal CommandLineParserReponse( CommandLineOptions options ) {
            Options = options;
            Success = true;
        }

        internal CommandLineParserReponse( IEnumerable<string> errors ) {
            Errors = errors;
            Success = false;
        }
    }
}