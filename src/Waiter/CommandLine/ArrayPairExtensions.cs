using System.Collections.Generic;
using System.Linq;

namespace Waiter.CommandLine {
    internal static class ArrayPairExtensions {
        internal static Dictionary<string, string> SplitPairs( this string[] items ) {
            var count = items.Count();
            var toReturn = new Dictionary<string, string>();

            for (var i = 0; i < count; i += 2) {
                toReturn.Add( items[i].ToUpper(), items[i + 1] );
            }

            return toReturn;
        } 
    }
}