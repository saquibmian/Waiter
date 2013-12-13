using System;
using Waiter.CommandLine;
using Waiter.Logging;
using Waiter.WaiterClient;

namespace Waiter {
    public class Program {
        private static CommandLineOptions _options;

        public static void Main( string[] args ) {
            var parser = new CommandLineParser();
            var result = parser.ParseCommandLineOptions( args );

            if ( !result.Success ) {
                foreach ( var error in result.Errors ) {
                    Logger.Error( error );
                }
                Environment.Exit( -1 );
            } 
            
            _options = result.Options;

            Uri url = null;
            try {
                url = new Uri( _options.Url );
            } catch(UriFormatException ex ) {
                Logger.Error( "Invalid URL format: {0}", _options.Url );
                Logger.Error( ex.Message );
                Logger.Error( ex.StackTrace );
                Environment.Exit( -1 );
            }

            // if port == 0, get latest free port
            if ( url.Port != _options.Port ) {
                var builder = new UriBuilder( url );
                builder.Port = _options.Port;
                url = builder.Uri;
            }

            var listener = new Listener {
                Method = _options.Method,
                RequestsToProcess = _options.NumberOfRequests,
                Timeout = _options.Timeout
            };

            listener.Listen( url.AbsoluteUri );

            Environment.Exit( 0 );
        }

    }
}
