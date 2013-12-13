using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Waiter.Networking;

namespace Waiter.CommandLine {
    internal class CommandLineParser {

        private CommandLineOptions _default;
        private List<string> _errors;

        private void Initialize() {
            _default = new CommandLineOptions {
                Port = 0,
                Method = HttpMethod.All,
                Timeout = 1200,
                NumberOfRequests = 1,
                Url = string.Format( "http://{0}/", IpFinder.GetLocalIp() ),
                Interactive = false
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
                    } else {
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
                    } else {
                        _errors.Add( string.Format( "{0} is not a valid timeout", pair.Value ) );
                    }
                    break;
                case "-REQUESTS":
                    int numRequests;
                    if (int.TryParse(pair.Value, out numRequests)) {
                        options.NumberOfRequests = numRequests;
                    } else {
                        _errors.Add( string.Format( "{0} is not a valid number", pair.Value ) );
                    }
                    break;
                case "-METHOD":
                    HttpMethod method;
                    if ( Enum.TryParse( pair.Value, true, out method ) ) {
                        options.Method = method;
                    } else {
                        _errors.Add( string.Format( "{0} is not a valid http method", pair.Value ) );
                    }
                    break;
                case "-INTERACTIVE":
                    bool boolean;
                    if ( Boolean.TryParse( pair.Value, out boolean ) ) {
                        options.Interactive = boolean;
                    } else {
                        _errors.Add( string.Format( "{0} is not a valid boolean", pair.Value ) );
                    }
                    break;
                default:
                    _errors.Add( string.Format( "The specified argument {0} is not valid.", pair.Key ) );
                    break;
            }
        }

        public static void ShowUsage() {
            Console.WriteLine();
            Console.WriteLine( "PARAMETER: port -- the port to listen to incoming requests on (default random)" );
            Console.WriteLine( "PARAMETER: url -- the url to listen for (default http://localhost/)" );
            Console.WriteLine( "PARAMETER: timeout -- the time in seconds to wait for a request to occur (default 1200 seconds)" );
            Console.WriteLine( "PARAMETER: requests -- the number of requests to listen for (default 1)" );
            Console.WriteLine( "PARAMETER: method -- the type of request to listen for, i.e., GET (default all)" );
            Console.WriteLine( "PARAMETER: interactive -- control whether requests should be processed (default false)" );
            Console.WriteLine( "USAGE: usage -- displays this message" );

            Console.WriteLine();
            Console.WriteLine( "Press any key to exit..." );
            Console.ReadKey();
        }
    }
}