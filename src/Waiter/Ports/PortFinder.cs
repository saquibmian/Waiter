using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Waiter.Ports {
    internal static class PortFinder {
        internal static int GetFreePort( int min = 1025, int max = 65356 ) {
            var usedPorts = IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .Select( x => x.Port )
                .ToArray();

            var randomNumberGenerator = new Random();
            int port;
            while (true) {
                port = randomNumberGenerator.Next( min, max );
                if (!usedPorts.Contains(port))
                    break;
            }

            return port;
        }
    }
}
