using System;
using Waiter.CommandLine;
using Waiter.Logging;
using Waiter.Networking;
using Waiter.UriExtensions;
using Waiter.WaiterClient;

namespace Waiter.Programs {
    internal class WaiterProgram : Program {

        private static CommandLineOptions _options { get { return CommandLineOptions.Global; } }

        internal new static void Main(string[] args) {
            var parser = new CommandsParser<CommandLineOptions>();
            var result = parser.Parse( args );

            if ( !result.Success ) {
                foreach ( var error in result.Errors ) {
                    Logger.Error( error );
                }
                Exit( -1 );
            }
            CommandLineOptions.Global = result.Options;

            if ( !_options.NoLogo ) {
                ShowLogo();
            }

            if ( _options.Url.Contains( "127.0.0.1" ) || _options.Url.Contains( "localhost" ) ) {
                var localIp = IpFinder.GetLocalIp();
                _options.Url = _options.Url
                    .Replace( "127.0.0.1", localIp )
                    .Replace( "localhost", localIp );
            }

            Uri url = null;
            try {
                url = new Uri( _options.Url );
            }
            catch ( UriFormatException ex ) {
                Logger.Error( "Invalid URL format: {0}", _options.Url );
                Logger.Error( ex.Message );
                Logger.Error( ex.StackTrace );
                Exit( -1 );
            }

            if ( _options.Port == 0 ) {
                _options.Port = PortFinder.GetFreePort();
            }
            if ( url.Port != _options.Port ) {
                url = url.ChangePort( _options.Port );
            }

            var listener = new Listener( _options );

            try {
                listener.Listen( url.AbsoluteUri );
            }
            catch ( Exception ex ) {
                Logger.Error( ex.Message );
                Logger.Error( ex.StackTrace );
                Exit( -1 );
            }

            if ( _options.Interactive ) {
                Console.WriteLine( "Done!" );
                Console.ReadLine();
            }

            Exit( 0 );
        }
    }
}
