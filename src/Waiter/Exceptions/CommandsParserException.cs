namespace Waiter.Exceptions {
    public class CommandsParserException : WaiterException {

        public CommandsParserException(string msg, params object[] args)
            : base(msg, args) {}

    }
}
