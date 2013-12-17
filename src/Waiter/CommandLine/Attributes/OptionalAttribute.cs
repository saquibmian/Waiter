using System;
using System.Collections.Generic;
using System.Reflection;
using Waiter.CommandLine.Parser;
using Waiter.Logging;

namespace Waiter.CommandLine.Attributes {
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class OptionalAttribute : Attribute, ICommandAttribute  {

        public string Command { get; set; }
        public object Default { get; set; }
        public string Description { get; set; }

        public void Process<T>(List<string> args, T model, PropertyInfo property) {
            if ( args.Contains( Command ) ) {
                var index = args.IndexOf( Command );
                object value = args[index + 1];

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