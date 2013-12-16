using System;
using Waiter.Networking;

namespace Waiter.Programs {
    internal class PortProgram : Program {
        internal new static void Main( string[] args ) {
            Console.WriteLine( PortFinder.GetFreePort() );
            Exit( 0 );
        }
    }
}