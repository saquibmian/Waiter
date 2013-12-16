using System;
using System.IO;
using System.Xml.Serialization;
using Waiter.CommandLine;
using Waiter.WaiterClient;

namespace Waiter.Logging {
    internal static class Logger {

        private static CommandLineOptions _options;

        private static CommandLineOptions Options {
            get { return _options ?? ( _options = CommandLineOptions.Global ); }
        }

        internal static void Initialize( CommandLineOptions options ) {
            _options = options;
        }
        
        internal static void Info( string msg, params object[] args ) {
            Console.WriteLine( "INFO: {0}", string.Format( msg, args ) );
        }

		internal static void Error( string msg, params object[] args ) {
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine( "ERROR: {0}", string.Format( msg, args ) );
			Console.ForegroundColor = previousColor;
		}

        internal static void Request(Request request) {
            if ( !Options.Log ) {
                return;
            }

            var serializer = new XmlSerializer( typeof (Request) );
            var fileName = string.Format( "request-{0}.xml", request.Id );
            var path = Path.Combine( Options.LogDirectory, fileName );

            if ( !Directory.Exists( Options.LogDirectory ) )
                Directory.CreateDirectory( Options.LogDirectory );

            using ( var stream = File.OpenWrite( path ) ) {
                serializer.Serialize( stream, request );
            }
            Info( "Logged request to {0}", path );
        }

    }
}
