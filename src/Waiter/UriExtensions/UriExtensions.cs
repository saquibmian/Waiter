using System;

namespace Waiter.UriExtensions {
    internal static class UriExtensions {

        internal static Uri ChangePort(this Uri uri, int port) {
            var builder = new UriBuilder( uri ) { Port = port };
            return builder.Uri;
        }

    }
}
