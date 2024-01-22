﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ASE_ProgrammingLanguage.CommandParser;
using static System.Windows.Forms.LinkLabel;

namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Class for parsing commands
    /// </summary>
    internal class CommandParser
    {
        private List<Command> commands;
        Dictionary<object, object> variables = new Dictionary<object, object>();
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
            //Dictionary<object, object> variables = new Dictionary<object, object>();
            foreach (Command command in commands)
            {
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
            List<Variable> parsedVariables = new List<Variable>();

            // Split input into lines and iterate through each line
            string[] lines = input.Split('\n');

            foreach (string line in lines)
            {
                // Use regular expression to match command structure and variable structure
                //Console.WriteLine(line);
                Match commandMatch = Regex.Match(line, @"^Command\s+(?<arg1>\w+)(?:\s+(?<arg2>\w+))?$");

                Match variableMatch = Regex.Match(line, @"(?<name>\w+)\s*=\s*(?<value>[^;]*)");


                if (commandMatch.Success)
                {
                    //Console.WriteLine("Command match: " + line);
                    string name = commandMatch.Groups["name"].Value;
                    string[] argsArray = new string[] { commandMatch.Groups["arg1"].Value, commandMatch.Groups["arg2"].Value };
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
                    //Console.WriteLine("Variable match: " + line);
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
                    
                
                

            }

            commands = parsedCommands;
            /*foreach (Command item in commands)
            {
                Console.WriteLine(item.Name);

            }*/
            ExecuteCommands();
        }

        public void BlockType(List<String> blocks)
        {
            //Console.WriteLine(blocks);
            if (blocks.Count > 1)
            {
                foreach (String block in blocks)
                {
                    BlockType(new List<string> {block});
                }
            }
            else if (AssertSelection(blocks[0]))
            {
                //format
                String formattedBlock = FormatBlock(blocks[0]);
                //System.Console.WriteLine(formattedBlock);
                CommandBlocker commandBlocker = new CommandBlocker(formattedBlock);
                foreach (List<string> block in commandBlocker.commandBlocks)
                {
                    BlockType(block);
                }



            }
            else 
            {
                /*foreach (string line in blocks[0])
                {
                    Console.WriteLine(line);
                }*/
                ParseCommands(blocks[0]);
            }
        }

        public bool AssertSelection(String block)
        {
            string[] lines = block.Split('\n');
            lines[0] = lines[0].ToLower();
            //Console.WriteLine(lines[0]);
            // Check if the first line contains "If"
            if (lines.Length > 0 && lines[0].Trim().StartsWith("if"))
            {
                Console.WriteLine("-----------HERE-----------");
                Console.WriteLine(lines[0]);
                Console.WriteLine("-----------HERE-----------");

                Match selectionMatch = Regex.Match(lines[0], @"^if\s+(\w+)\s+(>|<|==|!=)\s+(\w+)$");
               
                if (selectionMatch.Success)
                {
                    Console.WriteLine("Slection match success");
                    string variable = selectionMatch.Groups[1].Value.Trim();
                    string comparisonOperator = selectionMatch.Groups[2].Value;
                    string value = selectionMatch.Groups[3].Value;

                    if (variables.ContainsKey(variable))
                    {
                        Console.WriteLine("variable exists");
                        // Process the condition based on the comparison operator
                        switch (comparisonOperator.Trim())
                        {
                            case "==":
                                // Handle equality comparison
                                if (variable == value)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            case "<":
                                // Handle less than comparison
                                if (int.Parse(variable) < int.Parse(value))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            case ">":
                                Console.WriteLine(int.Parse(variable));
                                // Handle equality comparison
                                if (int.Parse(variable) > int.Parse(value))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            case "!=":
                                // Handle equality comparison
                                if (variable != value)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }


                            default:
                                // Invalid comparison operator
                                Console.WriteLine("Invalid comparison operator");
                                return false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Throw exception");
                }
            }
            return false;

        }
        public string FormatBlock(String block)
        {
            string[] lines = block.Split('\n');

            // Remove the first and last lines
            lines = lines.Where((line, index) => index != 0 && index != lines.Length - 1).ToArray();

            // Trim 3 whitespaces from each line
            lines = lines.Select(line => line.TrimStart(' ', '\t').Substring(3)).ToArray();

            // Join the modified lines back into a string
            string result = string.Join("\n", lines);

            return result;
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

        public class Variable
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public Variable(string name, object value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return $"{Name} = {Value}";
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