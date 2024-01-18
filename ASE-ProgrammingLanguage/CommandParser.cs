using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Class for parsing commands
    /// </summary>
    internal class CommandParser
    {
        private List<Command> commands;
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
            foreach (Command command in commands)
            {
                switch (command.Name.ToLower()) //to lower so commands arent cast sensative
                {
                    case "drawline":
                        if (command.Arguments.Count == 2 && command.Arguments[0] is int && command.Arguments[1] is int)
                        {
                            drawer.DrawLine((int)command.Arguments[0], (int)command.Arguments[1]);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        
                        break;

                    case "moveto":
                        if (command.Arguments.Count == 2 && command.Arguments[0] is int && command.Arguments[1] is int)
                        {
                            drawer.MoveTo((int)command.Arguments[0], (int)command.Arguments[1]);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "drawto":
                        if (command.Arguments.Count == 2 && command.Arguments[0] is int && command.Arguments[1] is int)
                        {
                            drawer.DrawTo((int)command.Arguments[0], (int)command.Arguments[1]);
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
                        if (command.Arguments.Count == 2 && command.Arguments[0] is int && command.Arguments[1] is int)
                        {
                            drawer.DrawRectangle((int)command.Arguments[0], (int)command.Arguments[1]);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "drawcircle":
                        if (command.Arguments.Count == 1 && command.Arguments[0] is int)
                        {
                            drawer.DrawCircle((int)command.Arguments[0]);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "drawtriangle":
                        if (command.Arguments.Count == 1 && command.Arguments[0] is int)
                        {
                            drawer.DrawTriangle((int)command.Arguments[0]);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "setpencolour":
                        if (command.Arguments.Count == 1 && command.Arguments[0] is String)
                        {
                            drawer.SetPenColour((String)command.Arguments[0]);
                        }
                        else
                        {
                            new OtherException("invalid argument(s)");
                        }
                        break;

                    case "setbrushcolour":
                        if (command.Arguments.Count == 1 && command.Arguments[0] is String)
                        {
                            drawer.SetBrushColour((String)command.Arguments[0]);
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
            Dictionary<string, object> variables = new Dictionary<string, object>();

            // Split input into lines and iterate through each line
            string[] lines = input.Split('\n');
            foreach (string line in lines)
            {
                // Use regular expression to match command structure and variable structure
                Match commandMatch = Regex.Match(line, @"(?<name>\w+)\((?<args>[^)]*)\)");
                Match variableMatch = Regex.Match(line, @"Var\((?<name>\w+),\s*(?<value>[^)]*)\)");
                if (commandMatch.Success)
                {
                    string name = commandMatch.Groups["name"].Value;
                    string[] argsArray = commandMatch.Groups["args"].Value.Split(',');

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

                    object varValue; //object declaration allows varValue to store values of any type 
                    if (int.TryParse(varValueStr, out int intValue))
                    {
                        varValue = intValue;
                    }
                    else
                    {
                        varValue = varValueStr;
                    }

                    variables[varName] = varValue;
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
    }
}