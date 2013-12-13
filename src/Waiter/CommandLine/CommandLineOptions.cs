using System;
using Waiter.Networking;

namespace Waiter.CommandLine {
    internal class CommandLineOptions {

        internal int Port { get; set; }
        internal int Timeout { get; set; }
        internal int NumberOfRequests { get; set; }
        internal string Url { get; set; }
        internal HttpMethod Method { get; set; }
        internal bool Interactive { get; set; }
		internal bool Log { get; set; }
		internal string LogDirectory { get; set; }

	    static CommandLineOptions _global;

	    internal static CommandLineOptions Global {
		    get { return _global ?? ( _global = Defaults ); }
			set { _global = value; }
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

        internal static void ShowUsage() {
            Console.WriteLine();
            Console.WriteLine("PARAMETER: port -- the port to listen to incoming requests on (default random)");
            Console.WriteLine("PARAMETER: url -- the url to listen for (default http://localhost/)");
            Console.WriteLine("PARAMETER: timeout -- the time in seconds to wait for a request to occur (default 1200 seconds)");
            Console.WriteLine("PARAMETER: requests -- the number of requests to listen for (default 1)");
            Console.WriteLine("PARAMETER: method -- the type of request to listen for, i.e., GET (default all)");
            Console.WriteLine("PARAMETER: log -- log all requests to the filesystem (default false)");
            Console.WriteLine("PARAMETER: logdirectory -- the path to the folder the save requests to (default is current directory)");
            Console.WriteLine("PARAMETER: interactive -- control whether requests should be processed (default false)");
            Console.WriteLine("USAGE: usage -- displays this message");

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
