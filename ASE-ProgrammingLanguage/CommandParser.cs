using System;
using System.CodeDom.Compiler;
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
    public class CommandParser
    {
        private static CommandParser instance;

        public List<ICommand> commands;
        public List<ICommand> methods = new List<ICommand>();
        public Dictionary<object, object> variables = new Dictionary<object, object>();
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
        /// Provides a singleton instance of the <see cref="CommandParser"/> class.
        /// </summary>
        /// <remarks>
        /// This property ensures that only one instance of the <see cref="CommandParser"/> is created.
        /// </remarks>
        /// <example>
        /// Example usage to access the singleton instance:
        /// <code>
        /// var parser = CommandParser.Instance;
        /// </code>
        /// </example>
        public static CommandParser Instance
        {
            get
            {
                if (instance == null)
                {
                    // Create the instance if it doesn't exist
                    instance = new CommandParser(new Drawer(Graphics.FromImage(new Bitmap(451, 375))));
                }
                return instance;
            }
        }
        /// <summary>
        /// Uses file dialog to open files
        /// </summary>
        /// <returns>The content of an opened file or null if an error happens </returns>
        public string OpenFile()
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
                throw new OtherException($"Error opening file: {ex.Message}", ex);
            }

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
                throw new OtherException($"Error saving file: {ex.Message}", ex);
            }

        }
        /// <summary>
        /// Obtains arguments and executes command objects  
        /// </summary>
        public void ExecuteCommands()
        {
            foreach (Command command in commands)
            {
                if (command.GetArguments().Count == 2)
                {
                    int arg1;
                    int arg2; ;
                    try
                    {
                        arg1 = (int)GetValueOrDefault(command.GetArguments()[0], variables);
                        arg2 = (int)GetValueOrDefault(command.GetArguments()[1], variables);

                        switch (command.GetName().ToLower())
                        {
                            case "drawline":
                                drawer.DrawLine(arg1, arg2);
                                break;
                            case "moveto":
                                drawer.MoveTo(arg1, arg2);
                                break;
                            case "drawto":
                                drawer.DrawTo(arg1, arg2);
                                break;
                            case "drawrectangle":
                                drawer.DrawRectangle(arg1, arg2);
                                break;
                            case "drawtriangle":
                                drawer.DrawTriangle(arg1, arg2);
                                break;
                        }
                    }
                    catch (InvalidCastException ex)
                    {
                        // Handle the exception caused by an invalid cast
                        throw new OtherException($"Invalid cast: {ex.Message}", ex);
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions that might occur
                        throw new OtherException($"An error occurred: {ex.Message}", ex);
                    }                   
                }
                else if (command.GetArguments().Count == 1)
                {
                    //Checks if method of same name exists               
                    Method method = null;

                    foreach (ICommand obj in methods)
                    {
                        if (obj is ICommand && ((Method)obj).GetName() == command.GetName())
                        {
                            method = (Method)obj;
                            break; // exit the loop when a match is found
                        }
                    }

                    if (method != null)
                    {
                        Console.WriteLine($"Executing block: {method.GetBlock()}");
                        CommandBlocker commandBlocker = new CommandBlocker(method.GetBlock());
                        BlockType(commandBlocker.commandBlocks);
                    }
                    else
                    {
                        object arg1 = GetValueOrDefault(command.GetArguments()[0], variables);
                        try
                        {
                            switch (command.GetName().ToLower())
                            {
                                case "drawcircle":
                                    drawer.DrawCircle((int)arg1);
                                    break;
                                case "setpencolour":
                                    drawer.SetPenColour((string)(arg1));
                                    break;
                                case "setbrushcolour":
                                    drawer.SetBrushColour((string)(arg1));
                                    break;
                                case "clear":
                                    drawer.Clear();
                                    break;
                                case "reset":
                                    drawer.Reset();
                                    break;
                                case "enablefill":
                                    drawer.EnableFill();
                                    break;
                                case "disablefill":
                                    drawer.DisableFill();
                                    break;
                                default:
                                    new OtherException(command + " is not a valid command or method");
                                    break;
                            }
                        }
                        catch (InvalidCastException ex)
                        {
                            // Handle the exception caused by an invalid cast
                            throw new OtherException($"Invalid cast: {ex.Message}", ex);

                        }
                        catch (Exception ex)
                        {
                            // Handle other exceptions that might occur
                            throw new OtherException($"An error occurred: {ex.Message}", ex);
                        }
                    }
                    
        
                }
                else
                {
                    
                }

            }
        }
        /// <summary>
        /// Parses commands
        /// </summary>
        /// <param name="input"></param>
        public void ParseCommands(string input)
        {
            List<ICommand> parsedCommands = new List<ICommand>();

            // Split input into lines and iterate through each line
            string[] lines = input.Split('\n');

            foreach (string line in lines)
            {
                // Use regular expression to match command structure and variable structure
                Match commandMatch = Regex.Match(line.Trim(), @"^(?!(endif|endloop|endmethod)\s)(?<name>\w+)(?:\s+(?<arg1>\w+)(?:,\s*(?<arg2>\w+))?)?$");
                Match variableMatch = Regex.Match(line, @"^(?!(if|while)\s)(?<name>\w+)\s*=\s*(?<value>[^;]*)$");
                String cArg1 = commandMatch.Groups["arg1"].Value;
                String cArg2 = commandMatch.Groups["arg2"].Value;
                if (commandMatch.Success)
                {
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

                    parsedCommands.Add(Factory.CreateProduct("Command", name, arguments));
                }
                else if (variableMatch.Success)
                {

                    string varName = variableMatch.Groups["name"].Value;
                    string varValueStr = variableMatch.Groups["value"].Value.Trim();
                    List<object> values = new List<object>();
                    values.Add(varName); 
                   
                    if (variables.ContainsKey(varName)) //checks if variable already exists
                    {
                        Match variableValueMatch = Regex.Match(varValueStr, @"^\s*([a-zA-Z_]\w*)\s*([+\-])\s*([a-zA-Z_]\w*|\d+)\s*$");
                        String var1 = variableValueMatch.Groups[1].Value;
                        String comparisonOperator = variableValueMatch.Groups[2].Value;
                        String var2 = variableValueMatch.Groups[3].Value;

                        //determine variable form i.e. Count = Count + 1
                        int intValue;
                        if (int.TryParse(varValueStr, out intValue))
                        {
                            variables[varName] = intValue;
                        }
                        else if (variableValueMatch.Success)
                        {

                            string[] varValues = { var1, var2 };
                            int[] intVarValues = {0,0};
                            int intVar1;
                            int intVar2;
                            int totalVal = 0;
                            for (int i = 0; i < varValues.Length; i++)
                            {
                                if (int.TryParse(varValues[i], out int intVar))
                                {
                                    intVarValues[i] = intVar;
                                }
                                else if (variables.ContainsKey(values[i]))
                                {
                                    intVarValues[i] = (int)variables[varValues[i]];
                                }
                                
                            }
                            intVar1 = intVarValues[0];
                            intVar2 = intVarValues[1];
                            switch (comparisonOperator.Trim())
                            {
                                case "+":
                                    totalVal = intVar1 + intVar2;
                                    break;
                                case "-":
                                    totalVal = intVar1 - intVar2;
                                    break;
                            }
                          
                            variables[varName] = totalVal;
                            

                        }
                        else
                        {
                            variables[varName] = varValueStr;
                        }


                    }
                    else if(int.TryParse(varValueStr, out int intVar))
                    {

                        variables.Add(varName, intVar);

                    }
                    else
                    {
                        //creates new variable
                        variables.Add(varName, varValueStr);
                    }

                }
                new OtherException("Invalid variable or command");
               
         

            }

            commands = parsedCommands;
           
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
                       throw new OtherException($"The variable you are trying to process does not exist: {variable}");
                    }
                }
            }
            else if (AssertIteration(ConvertListToString(blocks[0])) == "NonVar" || AssertIteration(ConvertListToString(blocks[0])) == "NonMatch")
            {
                blocks = new List<List<string>>();
            }
            else if (AssertMethod(ConvertListToString(blocks[0])) == "true")
            {
                //format
                String formattedBlock = FormatBlock(ConvertListToString(blocks[0]));
                string[] lines = ConvertListToString(blocks[0]).Split('\n');
                lines[0] = lines[0];
                Match methodMatch = Regex.Match(lines[0], @"^method\s+(\w+)\s*\((.*?)\)$");
                if (methodMatch.Success)
                {
                    Console.WriteLine(formattedBlock);
                    methods.Add(Factory.CreateProduct("Method", methodMatch.Groups[1].Value, new List<object> { methodMatch.Groups[2].Value, formattedBlock }));
                }

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
        /// Asserts whether is a line is a method
        /// </summary>
        /// <param name="block">is string block encapsulated within method</param>
        /// <returns>string</returns>
        /// <exception cref="OtherException"></exception>
        public String AssertMethod(string block)
        {
            string[] lines = block.Split('\n');
            lines[0] = lines[0].ToLower();

            if (lines.Length > 0 && lines[0].Trim().StartsWith("method"))
            {
                Match methodMatch = Regex.Match(lines[0], @"^method\s+(\w+)\s*\((.*?)\)$");

                if (methodMatch.Success)
                {
                    return "true";
                }
                else
                {
                    throw new OtherException("Must declare method name");
                }
            }
            else
            {
                return "False";
            }
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

                    string val1 = selectionMatch.Groups[1].Value.Trim();
                    string comparisonOperator = selectionMatch.Groups[2].Value;
                    string val2 = selectionMatch.Groups[3].Value;

                    string[] varValues = { val1, val2 };
                    int[] intVarValues = { 0, 0 };
                    int intVar1;
                    int intVar2;
                    for (int i = 0; i < varValues.Length; i++)
                    {
                        if (int.TryParse(varValues[i], out int intVar))
                        {
                            intVarValues[i] = intVar;
                        }
                        else if (variables.ContainsKey(varValues[i]))
                        {
                            intVarValues[i] = (int)variables[varValues[i]];
                        }

                    }
                    intVar1 = intVarValues[0];
                    intVar2 = intVarValues[1];

                        // Process the condition based on the comparison operator
                        switch (comparisonOperator.Trim())
                        {
                            case "==":
                                // Handle equality comparison
                                if (intVar1 == intVar2)
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "NonMatch";
                                }

                            case "<":
                                // Handle less than comparison
                                if (intVar1 < intVar2)
                                {
                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }

                            case ">":
                                // Handle equality comparison
                                if (intVar1 > intVar2)
                                {

                                    return "true";
                                }
                                else
                                {

                                    return "NonMatch";
                                }

                            case "!=":
                                // Handle equality comparison
                                if (intVar1 != intVar2)
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
                    throw new OtherException($"Invalid selection statement: {lines[0]}");
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
                    throw new OtherException($"Invalid iteration statement: {lines[0]}");
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

            if (newLines.Any(newLine => newLine.TakeWhile(char.IsWhiteSpace).ToArray().Length % 3 != 0))
            {
                throw new OtherException("Incorrect identation");
            }
            // Join the modified lines back into a string
            string result = string.Join("\n", newLines);

            return result;
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
                throw new OtherException($"Invalid argument: {argument}");
            }
        }
        

    }
}