using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Waiter.CommandLine.Attributes;
using Waiter.Exceptions;

namespace Waiter.CommandLine.Parser {
    public class CommandsParser<T> where T : new() {

        private readonly Dictionary<PropertyInfo, ICommandAttribute> _commands;

        public CommandsParser() {
            _commands = new Dictionary<PropertyInfo, ICommandAttribute>();
            var props = ( typeof (T) ).GetProperties();
            foreach ( var prop in props ) {
                var attr = prop
                               .GetCustomAttributes( false )
                               .FirstOrDefault( x => x is ICommandAttribute ) as ICommandAttribute;
                if ( attr != null )
                    _commands.Add( prop, attr );
            }
        }

        public CommandsParserReponse<T> Parse(string[] args) {
            var arguments = args.ToList();
            var model = new T();
            var errors = new List<string>();

            foreach ( var command in _commands ) {
                try {
                    command.Value.Process( arguments, model, command.Key );
                } catch ( CommandsParserException ex ) {
                    errors.Add( ex.Message );
                }
            }

            return errors.Any() 
                ? new CommandsParserReponse<T>( errors ) 
                : new CommandsParserReponse<T>( model );
        }

        public string Usage {
            get {
                var builder = new StringBuilder();
                foreach ( var command in _commands ) {
                    builder.AppendFormat(
                        "{0}: {1} -- {2} {3}\n",
                        command.Key.Name.Replace( "Attribute", string.Empty ),
                        command.Value.Command,
                        command.Value.Description,
                        command.Value is OptionalAttribute
                            ? string.Format( "(default {0})", ( ( OptionalAttribute ) command.Value ).Default )
                            : ""
                    );
                }
                return builder.ToString();
            }
        }
    }
}
