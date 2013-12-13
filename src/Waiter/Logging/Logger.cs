using System;
using System.IO;
using System.Xml.Serialization;
using Waiter.CommandLine;
using Waiter.WaiterClient;

namespace Waiter.Logging {
    internal static class Logger {
        
        internal static void Info( string msg, params object[] args ) {
            Console.WriteLine( "INFO: {0}", string.Format( msg, args ) );
        }

		internal static void Error( string msg, params object[] args ) {
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine( "ERROR: {0}", string.Format( msg, args ) );
			Console.ForegroundColor = previousColor;
		}

		internal static void Request( Request request ) {
			if( !CommandLineOptions.Global.Log ) {
				return;
			}

			var serializer = new XmlSerializer( typeof( Request ) );
			var fileName = string.Format( "request-{0}.xml", request.Id );
			var path = Path.Combine( CommandLineOptions.Global.LogDirectory, fileName );

			if( !Directory.Exists( CommandLineOptions.Global.LogDirectory ) )
				Directory.CreateDirectory( CommandLineOptions.Global.LogDirectory );

			using( var stream = File.OpenWrite( path ) ) {
				serializer.Serialize( stream, request );
			}
			Info( "Logged request to {0}", path );
		}  
  
    }
}
