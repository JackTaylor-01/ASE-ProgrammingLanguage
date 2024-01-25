using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASE_ProgrammingLanguage
{
    internal class MethodFactory
    {
        public static CommandParser.Method CreateMethod(string name, string parameters, string block)
        {
            return new CommandParser.Method(name, parameters, block);
        }
    }
}
