using System;
using System.Net;
using System.Text;
using Waiter.Exceptions;

namespace Waiter.WaiterClient {
    internal class Listener {

        private HttpListener _listener;
        internal int RequestsToProcess { get; set; }
        internal int Timeout { get; set; }

        internal void Listen( string urlToListenTo ) {
            _listener = new HttpListener();
            _listener.Prefixes.Add( urlToListenTo );
            try {
                _listener.Start();
            } catch (HttpListenerException ex) {
                throw new IncorrectUrlException(ex, "Incorrect url {0}", urlToListenTo);
            }

            for (var i = 1; i <= RequestsToProcess; i++) {
                // log that we are waiting for request #x
                var context = _listener.BeginGetContext( ProcessRequest, _listener );
                var response = context.AsyncWaitHandle.WaitOne( Timeout*1000 );
                if ( !response ) {
                    throw new WaiterTimeoutException( "Timed out while waiting for request #{0}.", i );
                }
            }

            _listener.Stop();
            _listener.Close();
            _listener = null;
        }

        private static void ProcessRequest(IAsyncResult result) {
            var listener = (HttpListener)result.AsyncState;

            var context = listener.EndGetContext(result);
            
            var request = context.Request;
            // log request received from to with payload

            var response = context.Response;
            var buffer = Encoding.UTF8.GetBytes( "Recieved" );
            response.ContentLength64 = buffer.Length;
            using ( var output = response.OutputStream ) {
                output.Write(buffer, 0, buffer.Length);
            }
        }

    }
}
