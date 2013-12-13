
namespace Waiter.CommandLine {
    internal class CommandLineOptions {

        internal int Port { get; set; }
        internal int Timeout { get; set; }
        internal int NumberOfRequests { get; set; }
        internal string Url { get; set; }
        internal HttpMethod Method { get; set; }

    }
}
