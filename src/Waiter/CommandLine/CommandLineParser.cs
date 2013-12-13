using System;
using System.Collections.Generic;
using System.Linq;

namespace Waiter.CommandLine {
    internal class CommandLineParser {

        private CommandLineOptions _default;
        private List<string> _errors;

        private void Initialize() {
            _default = new CommandLineOptions {
                Port = 1234,
                Method = HttpMethod.All,
                Timeout = 1200,
                Url = "*"
            };

            _errors = new List<string>();
        }

        internal CommandLineParserReponse ParseCommandLineOptions(string[] args) {
            Initialize();

            if ( args.Count() % 2 != 0 ) {
                _errors.Add( "Invalid usage" );
                return new CommandLineParserReponse( _errors );
            }

            var argPairs = args.SplitPairs();
            foreach ( var pair in argPairs ) {
                ParsePair(_default, pair);
            }

            if ( _errors.Count > 0 ) {
                return new CommandLineParserReponse( _errors );
            } else {
                return new CommandLineParserReponse( _default );
            }
        }

        private void ParsePair( CommandLineOptions options, KeyValuePair<string, string> pair ) {
            switch (pair.Key) {
                case "-PORT":
                    int port;
                    if ( int.TryParse( pair.Value, out port ) ) {
                        options.Port = port;
                    }
                    else {
                        _errors.Add( string.Format( "{0} is not a valid port number", pair.Value ) );
                    }
                    break;
                case "-URL":
                    options.Url = pair.Value;
                    break;
                case "-TIMEOUT":
                    int timeout;
                    if ( int.TryParse( pair.Value, out timeout ) ) {
                        options.Timeout = timeout;
                    }
                    else {
                        _errors.Add( string.Format( "{0} is not a valid timeout", pair.Value ) );
                    }
                    break;
                case "-METHOD":
                    HttpMethod method;
                    if ( Enum.TryParse( pair.Value, true, out method ) ) {
                        options.Method = method;
                    }
                    else {
                        _errors.Add( string.Format( "{0} is not a valid http method", pair.Value ) );
                    }
                    break;
                default:
                    _errors.Add( string.Format( "The specified argument {0} is not valid.", pair.Key ) );
                    break;
            }
        }
    }
}