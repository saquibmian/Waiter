using System;
using System.Text;
using Waiter.Networking;
using Waiter.CommandLine.Attributes;

namespace Waiter.CommandLine {
    internal class CommandLineOptions {

        [Optional(Command = "-port", Default = 0, Description = "the port to listen to incoming requests on")]
        public int Port { get; set; }

        [Optional(Command = "-timeout", Default = 3600, Description = "the time in seconds to wait for a request to occur")]
        public int Timeout { get; set; }

        [Optional(Command = "-requests", Default = 1, Description = "the number of requests to listen for")]
        public int NumberOfRequests { get; set; }

        [Optional(Command = "-url", Default = "http://localhost/", Description = "the url to listen for")]
        public string Url { get; set; }

        [Optional(Command = "-method", Default = HttpMethod.All, Description = "the type of request to listen for, i.e., GET")]
        public HttpMethod Method { get; set; }

        [Flag(Command = "-interactive", Description = "control whether requests should be processed")]
        public bool Interactive { get; set; }

        [Flag(Command = "-nologo", Description = "control whether requests should be processed")]
        public bool NoLogo { get; set; }

        [Flag(Command = "-log", Description = "log all requests to the filesystem")]
        public bool Log { get; set; }

        [Optional(Command = "-logdirectory", Default = ".\\", Description = "the path to the folder the save requests to")]
        public string LogDirectory { get; set; }

	    static CommandLineOptions _global;

	    internal static CommandLineOptions Global {
		    get { return _global ?? ( _global = Defaults ); }
			set { _global = value; }
	    }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.AppendFormat( "PORT = {0}\n", Port );
            builder.AppendFormat( "TIMEOUT = {0}\n", Timeout );
            builder.AppendFormat( "NUMBEROFREQUESTS = {0}\n", NumberOfRequests );
            builder.AppendFormat( "URL = {0}\n", Url );
            builder.AppendFormat( "METHOD = {0}\n", Method );
            builder.AppendFormat( "INTERACTIVE = {0}\n", Interactive );
            builder.AppendFormat( "LOG = {0}\n", Log );
            builder.AppendFormat( "LOGDIRECTORY = {0}\n", LogDirectory );
            return builder.ToString();
        }

        internal static CommandLineOptions Defaults = new CommandLineOptions {
            Port = 0,
            Method = HttpMethod.All,
            Timeout = 1200,
            NumberOfRequests = 1,
            Url = string.Format("http://{0}/", IpFinder.GetLocalIp()),
			Log = false,
			LogDirectory = AppDomain.CurrentDomain.BaseDirectory,
            Interactive = false
        };

    }
}
