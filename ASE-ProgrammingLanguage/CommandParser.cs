﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //forms as i decided to use windows file manager to select text files (wasn't specified in spec)

namespace ASE_ProgrammingLanguage
{
    internal class CommandParser 
    {
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


    }
}
