using System;
using System.Net;
using System.Threading;
using Waiter.CommandLine;
using Waiter.Exceptions;
using Waiter.Logging;

namespace Waiter.WaiterClient {
    internal class Listener {

	    private HttpListener _listener;
        private int _currentRequest = 1;
		readonly CommandLineOptions _options;

	    public Listener( CommandLineOptions options ) {
		    _options = options;
	    }

	    internal void Listen( string urlToListenTo ) {
            _listener = new HttpListener();
            _listener.Prefixes.Add( urlToListenTo );
            try {
                _listener.Start();
            } catch (HttpListenerException ex) {
                throw new IncorrectUrlException(ex, "Incorrect url {0}", urlToListenTo);
            }
			Logger.Info( "Listening for {0} requests matching {1} ...", _options.Method.ToString().ToUpper(), urlToListenTo );

			while( _currentRequest <= _options.NumberOfRequests ) {
				Logger.Info( "Waiting for request #{0} out of {1} ...", _currentRequest, _options.NumberOfRequests );

                var context = _listener.BeginGetContext( ProcessRequest, _listener );
                var response = context.AsyncWaitHandle.WaitOne( _options.Timeout*1000 );
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
			if( !_options.Method.Accepts( request.HttpMethod ) ) {
                context.Response.Ignore();
                return;
            }
            _currentRequest++;
	        string body = null;
            if ( request.HasEntityBody ) {
	            body = request.GetBody();
                Logger.Info( "\tRequest body is '{0}'", body );
            }

            context.Response.Accept();

	        var dto = new Request {
		        Time = DateTime.Now,
		        Id = _currentRequest-1,
		        From = request.RemoteEndPoint.ToString(),
		        To = request.LocalEndPoint.ToString(),
		        Method = request.HttpMethod,
		        Content = body
	        };
	        Logger.Request( dto );
        }

    }
}
