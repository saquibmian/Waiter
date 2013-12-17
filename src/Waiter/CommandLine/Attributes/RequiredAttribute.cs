using System;
using System.Collections.Generic;
using System.Reflection;
using Waiter.CommandLine.Parser;
using Waiter.Exceptions;

namespace Waiter.CommandLine.Attributes {
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class RequiredAttribute : Attribute, ICommandAttribute {

        public string Command { get; set; }
        public string Description { get; set; }

        public void Process<T>(List<string> args, T model, PropertyInfo property) {
            if ( !args.Contains( Command ) ) {
                throw new CommandsParserException( "Required parameter not present: {0}", Command );
            }

            var index = args.IndexOf( Command );
            object value = args[index + 1];

            string error;
            if ( !PropertyHelper.TrySet( property, model, value, out error ) ) {
                Logging.Logger.Error( error );
                args.Remove(Command);
            }
        }
    }
}