using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Waiter.Networking {
    internal class IpFinder {
        
        internal static string GetLocalIp() {
            var localIp = Dns
                .GetHostEntry( Dns.GetHostName() )
                .AddressList
                .FirstOrDefault(
                    ip => ip.AddressFamily == AddressFamily.InterNetwork
                );
            return string.Format( "{0}", localIp );
        }
    }
}
