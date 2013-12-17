using System.Collections.Generic;

namespace Waiter.CommandLine.Parser {
    public class CommandsParserReponse<T> {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T Options { get; set; }

        internal CommandsParserReponse( T options ) {
            Options = options;
            Success = true;
        }

        internal CommandsParserReponse( IEnumerable<string> errors ) {
            Errors = errors;
            Success = false;
        }
    }
}