using System;

namespace Waiter.CommandLine {
    internal enum HttpMethod {
        Get, Patch, Delete, Put, Post, All
    }

    internal static class HttpMethodExtensions {
        internal static bool Accepts( this HttpMethod method, string accepts ) {
            var isAll = method == HttpMethod.All;
            var isSame = string.Equals(method.ToString(), accepts, StringComparison.InvariantCultureIgnoreCase);
            
            return isAll || isSame;
        }
    }
}