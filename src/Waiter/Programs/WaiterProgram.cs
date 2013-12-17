using System;
using Waiter.CommandLine;
using Waiter.CommandLine.Parser;
using Waiter.Logging;
using Waiter.WaiterClient;

namespace Waiter.Programs {
    internal class WaiterProgram : Program {

        private static CommandLineOptions _options {
            get { return CommandLineOptions.Global; }
        }

        private static readonly WaiterOptionsProcessor _optionsProcessor = new WaiterOptionsProcessor();

        internal new static void Main( string[] args ) {
            ParseArgs( args );
            ProcessOptions();

            if ( !_options.NoLogo ) {
                ShowLogo();
            }

            StartListener();

            Exit( 0 );
        }

        private static void ParseArgs( string[] args ) {
            var parser = new CommandsParser<CommandLineOptions>();
            var result = parser.Parse( args );

            if ( !result.Success ) {
                foreach ( var error in result.Errors ) {
                    Logger.Error( error );
                }
                Exit( -1 );
            }
            CommandLineOptions.Global = result.Options;
        }

        private static void ProcessOptions() {
            var result = _optionsProcessor.ProcessOptions( _options );
            if ( !result ) {
                Exit( -1 );
            }
        }

        private static void StartListener() {
            var listener = new Listener( _options );

            try {
                listener.Listen( _options.Url );
            } catch ( Exception ex ) {
                Logger.Error( ex.Message );
                Logger.Error( ex.StackTrace );
                Exit( -1 );
            }
        }

    }
}
