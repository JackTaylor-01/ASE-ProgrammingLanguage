using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Converts string into List of lists of strings"
    /// </summary>
    class CommandBlocker
    {
        public List<List<string>> commandBlocks;

        public CommandBlocker(string input)
        {
            commandBlocks = new List<List<string>>();
            ProcessString(input);
        }
        /// <summary>
        /// Splits string into individual command blocks to be executed in order
        /// </summary>
        /// <param name="input">input string</param>
        private void ProcessString(string input)
        {
            string[] lines = input.Split('\n');

            Stack<List<string>> blockStack = new Stack<List<string>>();
            List<string> soloBlocks = new List<string>();
            bool insideBlock = false; // New variable to track if inside a block
            bool insideIf = false;
            bool insideWhile = false;
            string statementType;
            foreach (string line in lines)
            {
                string trimmedLine = line.TrimEnd();

                // Check if the line contains "If" or "While" at the beginning
                Match openStatementMatch = Regex.Match(trimmedLine, @"^\b(If|While)\b");
                Match closeStatementMatch = Regex.Match(trimmedLine, @"^\b(Endif|Endloop)\b");

                if (openStatementMatch.Success)
                {
                    statementType = openStatementMatch.Groups[1].Value.ToLower();
                    if (!insideBlock) // Check if not already inside a block
                    {
                        switch (statementType)
                        {
                            case "if":
                                insideIf = true;
                                break;
                            case "while":
                                insideWhile = true; 
                                break;
                        }
                        List<string> block = new List<string> { trimmedLine };
                        commandBlocks.Add(soloBlocks);
                        blockStack.Push(block);
                        soloBlocks = new List<string>();
                        insideBlock = true;
                    }
                    else
                    {
                        // If already inside a block, add the line to the current block
                        List<string> currentBlock = blockStack.Peek();
                        currentBlock.Add(trimmedLine);
                    }
                }

                // Check if the line contains "Endif" or "Endloop" at the beginning
              
                else if (closeStatementMatch.Success)
                {
                    statementType = closeStatementMatch.Groups[1].Value.ToLower();
                    switch (statementType)
                    {
                        case "endif":
                            insideIf = false;
                            break;
                        case "endloop":
                            insideWhile = false;
                            break;
                    }
                    if (blockStack.Count > 0 && insideIf == false && insideWhile == false)
                    {
                        List<string> currentBlock = blockStack.Pop();
                        currentBlock.Add(trimmedLine);
                        commandBlocks.Add(currentBlock);
                        insideBlock = false;
                    }
                    else
                    {
                        // Handle error: unexpected "Endif" or "Endloop"
                        throw new OtherException($"Invalid {trimmedLine} incorrect ending statement");
                    }
                }
                // Check if the line is inside a block
                else if (blockStack.Count > 0)
                {
                    List<string> currentBlock = blockStack.Peek();
                    currentBlock.Add(trimmedLine);
                }
                // If the line is not inside a block
                else if (!insideBlock)
                {
                    soloBlocks.Add(trimmedLine);
                }
            }

            // Add any remaining solo blocks to the commandBlocks list
            if (soloBlocks.Count > 0)
            {
                commandBlocks.Add(soloBlocks);
            }
        }
    }



}
   
