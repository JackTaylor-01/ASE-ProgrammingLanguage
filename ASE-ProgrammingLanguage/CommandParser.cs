using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Class for parsing commands
    /// </summary>
    internal class CommandParser
    {
        private List<Command> commands;
        //Dictionary<string, object> variables;
        Drawer drawer;
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
      
        public CommandParser(Drawer drawer)
        {

            this.drawer = drawer;

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "select program";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "save program";
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

        }

        /// <summary>
        /// Uses file dialog to open files
        /// </summary>
        /// <returns>The content of an opened file or null if an error happens </returns>
        public string OpenFile()
        {
            {
                try
                {
                    // Read the content from the selected file
                    openFileDialog.ShowDialog();
                    string filePath = openFileDialog.FileName;
                    string content = System.IO.File.ReadAllText(filePath);
                    Console.WriteLine($"File opened successfully: {filePath}");
                    return content;
                }
                catch (Exception ex)
                {
                    new OtherException("Error opening file");
                }
            }
            return null;
        }
        /// <summary>
        /// Saves provided program to a file
        /// </summary>
        /// <param name="program"> string to be saved </param>
        public void SaveFile(String program)
        {
            try
            {
                // Write the content to the selected file
                saveFileDialog.ShowDialog();
                string filePath = saveFileDialog.FileName;
                System.IO.File.WriteAllText(filePath, program);
                Console.WriteLine($"File saved successfully: {filePath}");
            }
            catch (Exception ex)
            {
                new OtherException("Error saving file");
            }

        }
        /// <summary>
        /// Executes commands objects  
        /// </summary>
        public void ExecuteCommands()
        {
            Dictionary<object, object> variables = new Dictionary<object, object>();
            foreach (Command command in commands)
            {
                foreach (KeyValuePair<object, object> kvp in variables)
                {
                    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
                }

                switch (command.Name.ToLower()) //to lower so commands arent cast sensative
                {
                    case "drawline":
                        if (command.Arguments.Count == 2)
                        {
                            int arg1 = (int)GetValueOrDefault(command.Arguments[0], variables);
                            int arg2 = (int)GetValueOrDefault(command.Arguments[1], variables);
                            drawer.DrawLine(arg1, arg2);
                        }
                        else
                        {
                            new OtherException("invalid number of argument(s)");
                        }
                        
                        break;

                    case "moveto":
                        if (command.Arguments.Count == 2)
                        {
                            int arg1 = (int)GetValueOrDefault(command.Arguments[0], variables);
                            int arg2 = (int)GetValueOrDefault(command.Arguments[1], variables);
                            drawer.MoveTo(arg1, arg2);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "drawto":
                        if (command.Arguments.Count == 2)
                        {
                            int arg1 = (int)GetValueOrDefault(command.Arguments[0], variables);
                            int arg2 = (int)GetValueOrDefault(command.Arguments[1], variables);
                            drawer.DrawTo(arg1, arg2);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "clear":
                        drawer.Clear();
                        break;

                    case "reset":
                        drawer.Reset();
                        break;

                    case "drawrectangle":
                        if (command.Arguments.Count == 2)
                        {
                            int arg1 = (int)GetValueOrDefault(command.Arguments[0], variables);
                            int arg2 = (int)GetValueOrDefault(command.Arguments[1], variables);
                            drawer.DrawRectangle(arg1, arg2);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "drawcircle":
                        if (command.Arguments.Count == 1)
                        {
                            int arg1 = (int)GetValueOrDefault(command.Arguments[0], variables);
                            Console.WriteLine(arg1);
                            drawer.DrawCircle(arg1);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "drawtriangle":
                        if (command.Arguments.Count == 2)
                        {
                            int arg1 = (int)GetValueOrDefault(command.Arguments[0], variables);
                            int arg2 = (int)GetValueOrDefault(command.Arguments[1], variables);
                            drawer.DrawTriangle(arg1, arg2);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "setpencolour":
                        if (command.Arguments.Count == 1)
                        {
                            String arg1 = (String)GetValueOrDefault(command.Arguments[0], variables);
                            drawer.SetPenColour(arg1);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "setbrushcolour":
                        if (command.Arguments.Count == 1 && command.Arguments[0] is String)
                        {
                            String arg1 = (String)GetValueOrDefault(command.Arguments[0], variables);
                            drawer.SetBrushColour(arg1);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "enablefill":
                        drawer.EnableFill();
                        break;

                    case "disablefill":
                        drawer.DisableFill();
                        break;

                    case "var":
                        if (variables.ContainsKey(command.Arguments[0])) //checks if variable already exists
                        {
                            //changes value of variable
                            variables[command.Arguments[0]] = command.Arguments[1];
                        }
                        else
                        {
                            //creates new variable
                            variables.Add(command.Arguments[0], command.Arguments[1]);
                        }
                        break;

                    default:
                        new OtherException(command + " is not a valid command");
                        break;
                }

            }
        }
        /// <summary>
        /// Parses commands
        /// </summary>
        /// <param name="input"></param>
        public void ParseCommands(string input)
        {
            List<Command> parsedCommands = new List<Command>();

            // Split input into lines and iterate through each line
            string[] lines = input.Split('\n');
            bool skipLine = false;
            Match aconditionalCommandMatch = Regex.Match(input, @"^\s*(If)\s+(?<condition>.+?)\s*(Endif)\s*$");
            int ifCount = aconditionalCommandMatch.Groups.Count;
            Console.WriteLine(ifCount);
            foreach (string line in lines)
            {
                // Use regular expression to match command structure and variable structure
                Match commandMatch = Regex.Match(line, @"^(?!If|Endif)(?<name>\w+)\s*(?:(?<args><\w+>)|(?<args>\w+(?:,\s*\w+)*))?[^=]*$");
                Match variableMatch = Regex.Match(line, @"(?<name>\w+)\s*=\s*(?<value>[^;]*)");
                Match conditionalCommandMatch = Regex.Match(line, @"^\s*(If)\s+(?<condition>.+?)\s*(Endif)\s*$");

                if (conditionalCommandMatch.Success && skipLine == true)
                {
                    Console.WriteLine("ENDIF");
                    if (string.Equals((conditionalCommandMatch.Groups["Endif"].Value), "Endif"))
                    {
                        skipLine = false;
                    }

                }
                else if (skipLine == false)
                {
                    if (commandMatch.Success)
                    {
                        string name = commandMatch.Groups["name"].Value;
                        string[] argsArray = commandMatch.Groups["args"].Value.Split(',');
                        //Console.WriteLine(name);
                        List<object> arguments = new List<object>();
                        foreach (string arg in argsArray)
                        {
                            if (int.TryParse(arg, out int intValue))
                            {
                                arguments.Add(intValue);
                            }
                            else
                            {
                                arguments.Add(arg.Trim());
                            }
                        }

                        parsedCommands.Add(new Command(name, arguments));
                    }
                    else if (variableMatch.Success)
                    {
                        string varName = variableMatch.Groups["name"].Value;
                        string varValueStr = variableMatch.Groups["value"].Value.Trim();

                        List<object> values = new List<object>();
                        values.Add(varName);
                        if (int.TryParse(varValueStr, out int intValue))
                        {
                            values.Add(intValue);
                        }
                        else
                        {
                            values.Add(varValueStr);
                        }

                        parsedCommands.Add(new Command("var", values));

                    }
                    else if (conditionalCommandMatch.Success)
                    {
                        if (string.Equals((conditionalCommandMatch.Groups["If"].Value), "If"))
                        {
                            skipLine = true;
                            //ParseCommands(conditionalCommandMatch.Groups["condition"].Value);
                            Console.WriteLine(conditionalCommandMatch.Groups["condition"].Value);
                            Console.WriteLine("IF STATEMENT");
                        }

                    }
                }
                

            }

            commands = parsedCommands;
            ExecuteCommands();
        }

        /// <summary>
        /// List object command with the arguments Name and arguments
        /// </summary>
        public class Command
        {
            public string Name { get; set; }
            public List<object> Arguments { get; set; }

            public Command(string name, List<object> arguments)
            {
                Name = name;
                Arguments = arguments;

            }

            public override string ToString()
            {
                string args = string.Join(", ", Arguments);
                return $"{Name}({args})";
            }
        }

        /// <summary>
        /// Gets the value from either the variables dictionary or uses the argument directly if it's an int or string.
        /// </summary>
        /// <param name="argument">The argument to get the value from.</param>
        /// <param name="variables">The dictionary of variables.</param>
        /// <returns>
        /// If the argument is already an int or string, returns the argument.
        /// If the argument is a valid variable in the variables dictionary and its value is an int or string, returns the value from the dictionary.
        /// Throws an ArgumentException if the argument is neither an int nor a string nor a valid variable.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the argument is neither an int nor a string nor a valid variable.</exception>
        private object GetValueOrDefault(object argument, Dictionary<object, object> variables)
        {

            if (variables.TryGetValue(argument, out object variableValue) && (variableValue is int || variableValue is string))
            {
                // If the argument is a key in the variables dictionary and its value is an int or string, return the value
                return variableValue;
            }
            else if (argument is int || argument is string)
            {
                // If the argument is already an int or string, return it
                return argument;
            }
            else
            {
                // If the argument is neither an int nor a string nor a valid variable, throw an ArgumentException
                throw new ArgumentException($"Invalid argument: {argument}", nameof(argument));
            }
        }
    }
}