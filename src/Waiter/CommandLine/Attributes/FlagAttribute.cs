using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Waiter.CommandLine.Attributes {
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class FlagAttribute : Attribute, ICommandAttribute {

        public string Command { get; set; }
        public string Description { get; set; }

        public void Process<T>( IEnumerable<string> args, T model, PropertyInfo property ) {
            if ( args.Contains( Command, StringComparer.InvariantCultureIgnoreCase ) ) {
                property.SetValue( model, true, new object[0] );
            }
        }

    }
}
