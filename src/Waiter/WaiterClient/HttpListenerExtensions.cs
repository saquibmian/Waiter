using System.IO;
using System.Net;
using System.Text;
using Waiter.Logging;

namespace Waiter.WaiterClient {
    internal static class HttpListenerExtensions {
        
        internal static void Accept( this HttpListenerResponse response ) {
            var buffer = Encoding.UTF8.GetBytes("Recieved");
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream) {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        internal static void Ignore( this HttpListenerResponse response ) {
            Logger.Info("Skipping the request");
            var buffer = Encoding.UTF8.GetBytes("Ignored");
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream) {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        internal static string GetBody( this HttpListenerRequest request ) {
            using (var reader = new StreamReader(request.InputStream)) {
                return reader.ReadToEnd();
            }

        }

    }
}