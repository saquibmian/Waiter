using System;

namespace Waiter.Exceptions {
    internal class IncorrectUrlException : WaiterException {
        internal IncorrectUrlException() {}

        internal IncorrectUrlException( string msg, Exception ex )
            : base( msg, ex ) {}

        internal IncorrectUrlException( Exception ex, string msg, params object[] args )
            : base( string.Format( msg, args ), ex ) {}

        internal IncorrectUrlException( string msg, params object[] args )
            : base( string.Format( msg, args ) ) {}
    }
}