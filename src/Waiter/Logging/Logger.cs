using System;

namespace Waiter.Logging {
    internal static class Logger {
        
        internal static void Info( string msg, params object[] args ) {
            Console.WriteLine( "INFO: {0}", string.Format( msg, args ) );
        }    
    }
}
