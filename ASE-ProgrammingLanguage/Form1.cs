using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Main entry way in to program
    /// </summary>
    public partial class Form1 : Form
    {
        Bitmap DrawingBitmap = new Bitmap(451, 375);
        Drawer drawer;
        CommandParser cmdParser;
        String cmdWindowTxt, cmdLineTxt;
        /// <summary>
        /// Represents the main form of the application.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            drawer = new Drawer(Graphics.FromImage(DrawingBitmap));
            cmdParser = new CommandParser(drawer);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Handles the TextChanged event of the cmdWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmdWindow_TextChanged(object sender, EventArgs e)
        {
            cmdWindowTxt = cmdWindow.Text;

        }
        /// <summary>
        /// Handles the TextChanged event of the cmdLine control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmdLine_TextChanged(object sender, EventArgs e)
        {
            cmdLineTxt = cmdLine.Text;
        }
        /// <summary>
        /// Handles the Click event of the buttonRun control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonRun_Click(object sender, EventArgs e)
        {

            drawOutput.Invalidate(); //invalidate needed as form needs repainting
            if (cmdLineTxt != "run" && !string.IsNullOrWhiteSpace(cmdLineTxt))
            {
                CommandBlocker commandBlocker = new CommandBlocker(cmdLineTxt);
                cmdParser.BlockType(commandBlocker.commandBlocks);

            }
            else if(cmdLineTxt == "run")
            {
                CommandBlocker commandBlocker = new CommandBlocker(cmdWindowTxt);
                cmdParser.BlockType(commandBlocker.commandBlocks);

            }
            else
            {
                //throw exception
                new OtherException("cannot run using null command");
            }
           
        }

        private void buttonSyntax_Click(object sender, EventArgs e)
        {

        }

        private void drawOutput_Click(object sender, EventArgs e)
        {
            

        }
        /// <summary>
        /// Handles the Paint event of the drawOutput control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void drawOutput_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(DrawingBitmap, 0, 0);

        }
        /// <summary>
        /// Handles the Click event of the buttonOpen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            cmdWindow.Text = cmdParser.OpenFile();
        }
        /// <summary>
        /// Handles the Click event of the buttonSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            cmdParser.SaveFile(cmdWindowTxt);
        }


    }
}
