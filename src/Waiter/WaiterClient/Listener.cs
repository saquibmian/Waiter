using System;
using System.Net;
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

        private int _currentRequest = 1;

        internal void Listen( string urlToListenTo ) {
            _listener = new HttpListener();
            _listener.Prefixes.Add( urlToListenTo );
            try {
                _listener.Start();
            } catch (HttpListenerException ex) {
                throw new IncorrectUrlException(ex, "Incorrect url {0}", urlToListenTo);
            }
            Logger.Info( "Listening for {0} requests matching {1} ...", Method.ToString().ToUpper(), urlToListenTo );

            while( _currentRequest <= RequestsToProcess ) {
                Logger.Info( "Waiting for request #{0} out of {1} ...", _currentRequest, RequestsToProcess );

                var context = _listener.BeginGetContext( ProcessRequest, _listener );
                var response = context.AsyncWaitHandle.WaitOne( Timeout*1000 );
                Thread.Sleep( 1000 ); //synchronizing the logging
                if ( !response ) {
                    throw new WaiterTimeoutException( "Timed out while waiting for request #{0}.", _currentRequest );
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
                context.Response.Ignore();
                return;
            }
            _currentRequest++;
            if ( request.HasEntityBody ) {
                Logger.Info( "\tRequest body is '{0}'", request.GetBody() );
            }

            context.Response.Accept();
        }

    }
}
