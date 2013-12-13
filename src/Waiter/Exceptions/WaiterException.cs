using System;

namespace Waiter.Exceptions {
    internal class WaiterException : Exception {
        internal WaiterException() {}

        internal WaiterException( string msg, Exception ex )
            : base( msg, ex ) {}

        internal WaiterException( Exception ex, string msg, params object[] args )
            : base( string.Format( msg, args ), ex ) {}

        internal WaiterException( string msg, params object[] args )
            : base( string.Format( msg, args ) ) {}
    }
}