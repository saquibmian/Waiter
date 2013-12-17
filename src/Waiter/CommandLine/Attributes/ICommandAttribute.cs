using System.Collections.Generic;
using System.Reflection;

namespace Waiter.CommandLine.Attributes {
    public interface ICommandAttribute {
        string Command { get; }
        string Description { get; }

        void Process<T>( IEnumerable<string> args, T model, PropertyInfo property );
    }
}