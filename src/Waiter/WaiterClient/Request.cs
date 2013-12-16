using System;

namespace Waiter.WaiterClient {
    public class Request {

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Method { get; set; }
        public string Content { get; set; }

    }
}