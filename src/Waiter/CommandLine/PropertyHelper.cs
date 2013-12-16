﻿using System;
using System.Reflection;

namespace Waiter.CommandLine {
    internal static class PropertyHelper {
        internal static bool TrySet( PropertyInfo prop, object model, object value, out string error ) {
            var convertedValue = value;

            if (prop.PropertyType == typeof(int)) {
                int temp;
                if ( !int.TryParse( ( string ) value, out temp ) ) {
                    error = string.Format( "Cannot convert {0} to an integer!", value );
                    return false;
                }
                convertedValue = temp;
            } else if (prop.PropertyType == typeof(bool)) {
                bool temp;
                if (!bool.TryParse((string)value, out temp))
                {
                    error = string.Format("Cannot convert {0} to a boolean!", value);
                    return false;
                }
                convertedValue = temp;
            } else if (prop.PropertyType.IsEnum) {
                try {
                     convertedValue = Enum.Parse( prop.PropertyType, ( string ) value, true );
                } catch ( ArgumentException ) {
                    error = string.Format( "Cannot convert {0} to a boolean!", value );
                    return false;
                }
            }

            prop.SetValue( model, convertedValue, new object[0] );
            error = null;
            return true;
        }
    }
}
