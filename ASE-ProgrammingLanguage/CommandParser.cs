using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
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
        /// <summary>
        /// Constructor constructs drawer from given graphics and instansiates Open file / Save file
        /// </summary>
        /// <param name="drawer">The drawer object used to draw onto bitmap</param>
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
        /// Obtains arguments and executes command objects  
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
                            new OtherException("invalid number of argument(s) for : DrawLine");
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
                            new OtherException("invalid argument(s) for : MoveTo");
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
                            new OtherException("invalid argument(s) for : DrawTo");
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
                            new OtherException($"invalid argument(s) for : DrawRectangle");
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
                            new OtherException($"invalid argument(s) for : DrawCircle");
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
                            new OtherException($"invalid argument(s) for : DrawTriangle");
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
                            new OtherException($"invalid argument(s) for : SetPenColour");
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
                            new OtherException("invalid argument(s) for : SetBrushColour");
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
                            int currentValue = Convert.ToInt32(variables[command.Arguments[0]]);
                            Match variableValueMatch = Regex.Match(Convert.ToString(command.Arguments[1]), @"^\s*([a-zA-Z_]\w*)\s*\+\s*([a-zA-Z_]\w*|\d+)\s*$");
                            String var1 = variableValueMatch.Groups[1].Value;
                            String var2 = variableValueMatch.Groups[2].Value;
              
                            //determine variable form i.e. Count = Count + 1
                            if (command.Arguments[1] is int)
                            {

                                variables[command.Arguments[0]] = command.Arguments[1];
                            }
                            else if (variableValueMatch.Success)
                            {

                                String[] varValues = { var1, var2 };
                                int totalVal = 0;
                                foreach(String var in varValues)
                                {
                                    if (variables.ContainsKey(var))
                                    {
                                        totalVal += (int)variables[var];
                                    }
                                    else if (int.TryParse(var, out int intValue))
                                    {
                                        totalVal += intValue;
                                    }
                                    
                                }
                                variables[command.Arguments[0]] = totalVal;

                            }
                            else
                            {
                                variables[command.Arguments[0]] = command.Arguments[1];
                            }

    
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
                Match commandMatch = Regex.Match(line.Trim(), @"^(?!(endif|endloop)\s)(?<name>\w+)(?:\s+(?<arg1>\w+)(?:,\s*(?<arg2>\w+))?)?$");
                Match variableMatch = Regex.Match(line, @"^(?!(if|while)\s)(?<name>\w+)\s*=\s*(?<value>[^;]*)$");
                String cArg1 = commandMatch.Groups["arg1"].Value;
                String cArg2 = commandMatch.Groups["arg2"].Value;
                if (commandMatch.Success)
                {
                    //Console.WriteLine("Command match: " + line);
                    string name = commandMatch.Groups["name"].Value;
                    string[] argsArray;
                    if (!string.IsNullOrWhiteSpace(cArg2))
                    {
                        argsArray = new string[] { cArg1, cArg2 };
                    }
                    else
                    {
                        argsArray = new string[] { cArg1 };
                    }
                    
                    //Console.WriteLine($"Command: {name} {cArg1} {cArg2}");
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

                    //Console.WriteLine($"Variable: {varName} {varValueStr}");

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
        /// <summary>
        /// Determines block type given a Command block this function will also iterate for nested blocks 
        /// </summary>
        /// <param name="blocks">A list of lists containing blocks created by CommandBlocker class</param>
        public void BlockType(List<List<String>> blocks)
        {

            if (blocks.Count > 1)
            {
                foreach (List<String> block in blocks)
                {

                    BlockType(new List<List<string>> { block });
                }
            }
            else if (AssertSelection(ConvertListToString(blocks[0])) == "true")
            {
                //format
                String formattedBlock = FormatBlock(ConvertListToString(blocks[0]));
                CommandBlocker commandBlocker = new CommandBlocker(formattedBlock);

                foreach (List<string> block in commandBlocker.commandBlocks)
                {
                    BlockType(new List<List<string>> { block });
                }
            }
            else if (AssertSelection(ConvertListToString(blocks[0])) == "NonVar" || AssertSelection(ConvertListToString(blocks[0])) == "NonMatch")
            {
                blocks = new List<List<string>>();
            }
            else if (AssertIteration(ConvertListToString(blocks[0])) == "true")
            {
                //format
                string[] lines = ConvertListToString(blocks[0]).Split('\n');
                lines[0] = lines[0].ToLower();
                Match selectionMatch = Regex.Match(lines[0], @"^while\s+(\w+)\s+(>|<|==|!=)\s+(\w+)$");
                if (selectionMatch.Success)
                {
                    string variable = selectionMatch.Groups[1].Value.Trim();
                    string comparisonOperator = selectionMatch.Groups[2].Value;
                    string value = selectionMatch.Groups[3].Value;
                    if (variables.ContainsKey(variable))
                    {

                        // Process the condition based on the comparison operator
                        // poor code practice code is reused
                        switch (comparisonOperator.Trim())
                        {
                            case "==":
                                // Handle equality comparison
                                while (Convert.ToInt32(variables[variable]) == int.Parse(value))
                                {
                                    String formattedBlock = FormatBlock(ConvertListToString(blocks[0]));
                                    CommandBlocker commandBlocker = new CommandBlocker(formattedBlock);

                                    foreach (List<string> block in commandBlocker.commandBlocks)
                                    {
                                        BlockType(new List<List<string>> { block });
                                    }
                                }
                                break;


                            case "<":
                                Console.WriteLine(FormatBlock(ConvertListToString(blocks[0])));
                                // Handle less than comparison
                                while (Convert.ToInt32(variables[variable]) < int.Parse(value))
                                {
                                    String formattedBlock = FormatBlock(ConvertListToString(blocks[0]));
                                    CommandBlocker commandBlocker = new CommandBlocker(formattedBlock);

                                    foreach (List<string> block in commandBlocker.commandBlocks)
                                    {
                                        BlockType(new List<List<string>> { block });
                                    }

                                }
                                break;

                            case ">":
                                // Handle equality comparison
                                while (Convert.ToInt32(variables[variable]) > int.Parse(value))
                                {
                                    String formattedBlock = FormatBlock(ConvertListToString(blocks[0]));
                                    CommandBlocker commandBlocker = new CommandBlocker(formattedBlock);

                                    foreach (List<string> block in commandBlocker.commandBlocks)
                                    {
                                        BlockType(new List<List<string>> { block });
                                    }

                                }
                                break;



                            case "!=":
                                // Handle equality comparison
                                while (Convert.ToInt32(variables[variable]) != int.Parse(value))
                                {
                                    String formattedBlock = FormatBlock(ConvertListToString(blocks[0]));
                                    CommandBlocker commandBlocker = new CommandBlocker(formattedBlock);

                                    foreach (List<string> block in commandBlocker.commandBlocks)
                                    {
                                        BlockType(new List<List<string>> { block });
                                    }

                                }
                                break;

                            default:

                                break;

                        }
                    }

                    else
                    {
                        new OtherException($"The variable you are trying to process does not exist {variable}");
                    }
                }
            }

            else if (AssertIteration(ConvertListToString(blocks[0])) == "NonVar" || AssertIteration(ConvertListToString(blocks[0])) == "NonMatch")
            {
                blocks = new List<List<string>>();
            }

            else
            {

                ParseCommands(ConvertListToString(blocks[0]));
            }
        
        }
        /// <summary>
        /// Converts a list of strings into a single string
        /// </summary>
        /// <param name="list"> list of strings</param>
        /// <returns>String</returns>
        public String ConvertListToString(List<String> list)
        {
            string returnString = "";
            int lastIndex = list.Count - 1;

            for (int i = 0; i < list.Count; i++)
            {
                returnString += list[i];

                if (i < lastIndex)
                {
                    returnString += "\n";
                }
            }
            return returnString;
        }
        /// <summary>
        /// Asserts whether a selection statement has been entered
        /// </summary>
        /// <param name="block">block containing potential statement</param>
        /// <returns>String</returns>
        public string AssertSelection(String block)
        {
            string[] lines = block.Split('\n');
            lines[0] = lines[0].ToLower();

            if (lines.Length > 0 && lines[0].Trim().StartsWith("if"))
            {
                

                Match selectionMatch = Regex.Match(lines[0], @"^if\s+(\w+)\s+(>|<|==|!=)\s+(\w+)$");
               
                if (selectionMatch.Success)
                {

                    string variable = selectionMatch.Groups[1].Value.Trim();
                    string comparisonOperator = selectionMatch.Groups[2].Value;
                    string value = selectionMatch.Groups[3].Value;

                    if (variables.ContainsKey(variable))
                    {

                        // Process the condition based on the comparison operator
                        switch (comparisonOperator.Trim())
                        {
                            case "==":
                                // Handle equality comparison
                                if (Convert.ToInt32(variables[variable]) == int.Parse(value))
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "NonMatch";
                                }

                            case "<":
                                // Handle less than comparison
                                if (Convert.ToInt32(variables[variable]) < int.Parse(value))
                                {
                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }

                            case ">":
                                // Handle equality comparison
                                if (Convert.ToInt32(variables[variable]) > int.Parse(value))
                                {

                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }

                            case "!=":
                                // Handle equality comparison
                                if (Convert.ToInt32(variables[variable]) != int.Parse(value))
                                {
                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }


                            default:
                                // Invalid comparison operator
                                Console.WriteLine("Invalid comparison operator");
                                return "NonMatch";
                        }
                    }
                    else
                    {
                        return "NonVar";
                    }
                }
                else
                {
                    Console.WriteLine("Throw exception");
                }
            }

            return "false";



        }
        /// <summary>
        /// Asserts whether an iteration statement has been entered
        /// </summary>
        /// <param name="block">block containing potential statement</param>
        /// <returns></returns>
        public string AssertIteration(String block)
        {
            string[] lines = block.Split('\n');
            lines[0] = lines[0].ToLower();

            if (lines.Length > 0 && lines[0].Trim().StartsWith("while"))
            {


                Match selectionMatch = Regex.Match(lines[0], @"^while\s+(\w+)\s+(>|<|==|!=)\s+(\w+)$");

                if (selectionMatch.Success)
                {

                    string variable = selectionMatch.Groups[1].Value.Trim();
                    string comparisonOperator = selectionMatch.Groups[2].Value;
                    string value = selectionMatch.Groups[3].Value;

                    if (variables.ContainsKey(variable))
                    {

                        // Process the condition based on the comparison operator
                        switch (comparisonOperator.Trim())
                        {
                            case "==":
                                // Handle equality comparison
                                if (Convert.ToInt32(variables[variable]) == int.Parse(value))
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "NonMatch";
                                }

                            case "<":
                                // Handle less than comparison
                                if (Convert.ToInt32(variables[variable]) < int.Parse(value))
                                {
                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }

                            case ">":
                                // Handle equality comparison
                                if (Convert.ToInt32(variables[variable]) > int.Parse(value))
                                {

                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }

                            case "!=":
                                // Handle equality comparison
                                if (Convert.ToInt32(variables[variable]) != int.Parse(value))
                                {
                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }


                            default:
                                // Invalid comparison operator
                                Console.WriteLine("Invalid comparison operator");
                                return "NonMatch";
                        }
                    }
                    else
                    {
                        return "NonVar";
                    }
                }
                else
                {
                    Console.WriteLine("Throw exception");
                }
            }
            return "false";



        }
        /// <summary>
        /// Formats a block to so it can be easily processed
        /// </summary>
        /// <param name="block">block to format</param>
        /// <returns>string</returns>
        public string FormatBlock(String block)
        {
            string[] lines = block.Split('\n');
            
            string[] newLines = new string[lines.Length - 2];
            int newIndex = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if(i != 0 && i != lines.Length - 1)
                {

                    newLines[newIndex] = lines[i];
                    newIndex++;
                }
            }    
            
            // Trim 3 whitespaces from each line
            newLines = newLines.Select(newLine => newLine.Substring(3)).ToArray();

            // Join the modified lines back into a string
            string result = string.Join("\n", newLines);

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

        /// <summary>
        /// Creates variable objects consisting of name and value
        /// </summary>
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