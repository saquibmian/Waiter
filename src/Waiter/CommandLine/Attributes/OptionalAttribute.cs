using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Waiter.CommandLine.Extensions;
using Waiter.CommandLine.Parser;
using Waiter.Logging;

namespace Waiter.CommandLine.Attributes {
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class OptionalAttribute : Attribute, ICommandAttribute  {

        public string Command { get; set; }
        public object Default { get; set; }
        public string Description { get; set; }

        public void Process<T>(IEnumerable<string> args, T model, PropertyInfo property) {
            if ( args.Contains( Command, StringComparer.InvariantCultureIgnoreCase ) ) {
                var index = args.IndexOf( Command );
                object value = args.ElementAt(index + 1);

                string error;
                if ( !PropertyHelper.TrySet(property, model, value, out error) ) {
                    Logger.Warn( error );
                    Logger.Warn( "Setting {0} to {1}", property.Name, Default );
                    PropertyHelper.TrySet( property, model, Default, out error );
                }
            } else {
                property.SetValue( model, Default, new object[0] );
            }
        }

    }
}