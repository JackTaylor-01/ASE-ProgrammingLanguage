using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Factory class responsible for creating command and method objects
    /// </summary>
    public interface ICommand
    {   
        /// <summary>
        /// Displays any objects attributes
        /// </summary>
        void DisplayInfo();
    }

    internal class Command : ICommand
    {
        private string name;
        private List<object> arguments;

        public Command(string name, List<object> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }
        public string GetName()
        {
            return name;
        }
        public List<object> GetArguments()
        {
            return arguments;
        }


        public void DisplayInfo()
        {
            Console.WriteLine($"Command: {name}, Arguments: {string.Join(", ", arguments)}");
        }
    }

    internal class Method : ICommand
    {
        private string name;
        private string parameters;
        private string block;

        public Method(string name, string parameters, string block)
        {
            this.name = name;
            this.parameters = parameters;
            this.block = block;
        }
        public string GetName()
        {
            return name;
        }
        public string GetParameters()
        {
            return parameters;
        }
        public string GetBlock()
        {
            return block;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Method: {name}, Parameters: {parameters}, Block: {block}");
        }
    }

    // Factory class creating ICommand objects
    internal class Factory
    {
        public static ICommand CreateProduct(string type, string name, List<object> arguments)
        {
            if (type.Equals("Command", StringComparison.OrdinalIgnoreCase))
            {
                return new Command(name, arguments);
            }
            if (arguments.Count >= 2 && arguments[0] is string parameters && arguments[1] is string block)
            {
                return new Method(name, parameters, block);
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }
        }
    }
}
