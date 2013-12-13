using System;

namespace Waiter.Exceptions {
    internal class WaiterTimeoutException : WaiterException {
        internal WaiterTimeoutException() {}

        internal WaiterTimeoutException( string msg, Exception ex )
            : base( msg, ex ) {}

        internal WaiterTimeoutException( Exception ex, string msg, params object[] args )
            : base( string.Format( msg, args ), ex ) {}

        internal WaiterTimeoutException( string msg, params object[] args )
            : base( string.Format( msg, args ) ) {}
    }
}