using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Waiter.CommandLine;
using Waiter.Exceptions;
using Waiter.Logging;

namespace Waiter.WaiterClient {
    internal class Listener {

        private HttpListener _listener;
        internal int RequestsToProcess { get; set; }
        internal int Timeout { get; set; }
        internal HttpMethod Method { get; set; }

        internal void Listen( string urlToListenTo ) {
            _listener = new HttpListener();
            _listener.Prefixes.Add( urlToListenTo );
            try {
                _listener.Start();
            } catch (HttpListenerException ex) {
                throw new IncorrectUrlException(ex, "Incorrect url {0}", urlToListenTo);
            }
            Logger.Info( "Listening for URLs matching {0} ...", urlToListenTo );

            for (var i = 1; i <= RequestsToProcess; i++) {
                Logger.Info( "Waiting for request #{0} out of {1}", i, RequestsToProcess );

                var context = _listener.BeginGetContext( ProcessRequest, _listener );
                var response = context.AsyncWaitHandle.WaitOne( Timeout*1000 );
                Thread.Sleep( 1000 ); //synchronizing the logging
                if ( !response ) {
                    throw new WaiterTimeoutException( "Timed out while waiting for request #{0}.", i );
                }
            }

            GC.KeepAlive( _listener );

            _listener.Stop();
            _listener.Close();
            _listener = null;
        }

        private void ProcessRequest(IAsyncResult result) {
            var listener = (HttpListener)result.AsyncState;

            var context = listener.EndGetContext(result);
            
            var request = context.Request;
            Logger.Info(
                "Recieved {0} request from {1} to {2}", 
                request.HttpMethod, 
                request.RemoteEndPoint, 
                request.LocalEndPoint
            );
            if ( !Method.Accepts( request.HttpMethod ) ) {
                Logger.Info( "Skipping the request" );
                return;
            }
            if ( request.HasEntityBody ) {
                using( var stream = request.InputStream )
                using ( var reader = new StreamReader( stream ) ) {
                    Logger.Info( "\tRequest body is '{0}'", reader.ReadToEnd() );
                }
            }

            var response = context.Response;
            var buffer = Encoding.UTF8.GetBytes( "Recieved" );
            response.ContentLength64 = buffer.Length;
            using ( var output = response.OutputStream ) {
                output.Write(buffer, 0, buffer.Length);
            }
        }

    }
}
