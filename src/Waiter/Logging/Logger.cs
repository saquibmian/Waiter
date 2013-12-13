using System;

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
    }
}
