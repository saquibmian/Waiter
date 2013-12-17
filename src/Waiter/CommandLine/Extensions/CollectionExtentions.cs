using System;
using System.Collections.Generic;

namespace Waiter.CommandLine.Extensions {
    internal static class CollectionExtentions {

        internal static int IndexOf(this IEnumerable<string> coll, string value, bool ignoreCase = true) {
            var count = 0;
            foreach ( var val in coll ) {
                var contains = ignoreCase
                                   ? string.Equals( val, value, StringComparison.InvariantCultureIgnoreCase )
                                   : string.Equals( val, value );
                if ( contains ) {
                    return count;
                }
                count++;
            }
            return -1;
        }

    }
}
