using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waiter.CommandLine;
using Waiter.Logging;
using Waiter.Networking;
using Waiter.UriExtensions;

namespace Waiter.WaiterClient {
    internal class WaiterOptionsProcessor {

        internal bool ProcessOptions( CommandLineOptions options ) {
            var result = ReplaceLocalHost( options );
            result |= FixPort( options );
            result |= FixUrl( options );

            return result;
        }

        private bool FixUrl( CommandLineOptions options ) {
            Uri url;
            try {
                url = new Uri( options.Url );
            }
            catch ( UriFormatException ex ) {
                Logger.Error( "Invalid URL format: {0}", options.Url );
                Logger.Error( ex.Message );
                Logger.Error( ex.StackTrace );
                return false;
            }
            if ( url.Port != options.Port ) {
                url = url.ChangePort( options.Port );
            }
            options.Url = url.AbsoluteUri;
            return true;
        }

        private bool FixPort( CommandLineOptions options ) {
            if ( options.Port == 0 ) {
                options.Port = PortFinder.GetFreePort();
            }
            return true;
        }

        private bool ReplaceLocalHost( CommandLineOptions options ) {
            var localIp = IpFinder.GetLocalIp();
            options.Url = options.Url
                .Replace( "127.0.0.1", localIp )
                .Replace( "localhost", localIp );

            return true;
        }
    }
}