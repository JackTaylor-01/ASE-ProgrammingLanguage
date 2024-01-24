using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASE_ProgrammingLanguage
{
    internal class CommandFactory
    {
        public static CommandParser.Command CreateCommand(string name, List<object> arguments)
        {
            return new CommandParser.Command(name, arguments);
        }
    }
}
