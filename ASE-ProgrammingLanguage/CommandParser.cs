using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //forms as i decided to use windows file manager to select text files (wasn't specified in spec)
using System.Text.RegularExpressions; //using regular expressions to parse commands

namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Provides functionality for parsing commands and includes opening and saving files.
    /// </summary>
    internal class CommandParser 
    {
        Drawer drawer;
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;  
        /// <summary>
        /// Initialises new instance of <see cref="CommandParser"/> class
        /// </summary>
        /// <param name="drawer"> drawer used by command parser to allow executing drawer commands within class</param>
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
                    Console.WriteLine($"Error reading the file: {ex.Message}");
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
                Console.WriteLine($"Error saving the file: {ex.Message}");
            }
           
        }
        /// <summary>
        /// Parses commands
        /// </summary>
        /// <param name="cmdString"> String of command(s) to parse</param>
        public void ParseCommands(String cmdString)
        {
            // Define a regular expression pattern to match commands and their optional arguments
            string pattern = @"(\w+)(?:\(([^)]*)\))?";

            // Use Regex.Matches to find all matches in the input string
            MatchCollection matches = Regex.Matches(cmdString, pattern);

            foreach (Match match in matches)
            {
                // Extract command and arguments
                string command = match.Groups[1].Value;

                // Check if arguments are present
                string[] arguments = match.Groups[2].Success ? match.Groups[2].Value.Split(',') : new string[0];

                // Trim whitespaces from each argument
                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = arguments[i].Trim();
                }

                for (int i = 0; i < arguments.Length; i++)
                {
                    Console.WriteLine($"Argument {i + 1}: {arguments[i]}");
                    //ExecuteCommand(ArgumentException[i]);
                }

            }

        }


    }
}
