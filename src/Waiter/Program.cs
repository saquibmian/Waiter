using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Waiter.CommandLine;
using Waiter.Logging;
using Waiter.Programs;

namespace Waiter {
    public class Program {

        public static void Main( string[] args ) {

            if ( !args.Any() ) {
                Logger.Error( "You must specify an operation!" );
                Exit( -1 );
            }

            var arguments = args.Skip( 1 ).ToArray();
            RunProgram( args[0], arguments );

            Exit( 0 );
        }

        private static void RunProgram( string operation, string[] arguments  ) {
            switch (operation) {
                case "usage":
                    UsageProgram.Main( arguments );
                    break;
                case "port":
                    PortProgram.Main( arguments );
                    break;
                case "wait":
                    WaiterProgram.Main( arguments );
                    break;
                default:
                    Logger.Error( "Incorrect operation: {0}", operation );
                    Exit( -1 );
                    break;
            }
        }

        protected static void Exit( int exitCode ) {
            bool debug = false;            
#if DEBUG
            debug = true; 
#endif

            if (CommandLineOptions.Global.Interactive || debug) {
                Console.ReadLine();
            }

            Environment.Exit( exitCode );
        }

        protected static void ShowLogo() {
            var version = FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).FileVersion;
            Console.WriteLine( "****************************************************" );
            Console.WriteLine("                  Waiter v{0}", version);
            Console.WriteLine("                   (c) The Peeps");
            Console.WriteLine( "****************************************************" );
        }

    }
}
