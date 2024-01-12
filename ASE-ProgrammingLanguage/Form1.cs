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
    public partial class Form1 : Form
    {
        Bitmap DrawingBitmap = new Bitmap(451, 375);
        Drawer drawer;
        CommandParser cmdParser;
        String cmdWindowTxt, cmdLineTxt;
        public Form1()
        {
            InitializeComponent();
            drawer = new Drawer(Graphics.FromImage(DrawingBitmap));
            cmdParser = new CommandParser(drawer);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cmdWindow_TextChanged(object sender, EventArgs e)
        {
            cmdWindowTxt = cmdWindow.Text;

        }

        private void cmdLine_TextChanged(object sender, EventArgs e)
        {
            cmdLineTxt = cmdLine.Text;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {

            drawOutput.Invalidate(); //invalidate needed as form needs repainting
            if (cmdLineTxt != "run")
            {
                cmdParser.ParseCommands(cmdLineTxt);
            }
            else if(cmdLineTxt != null)
            {
                cmdParser.ParseCommands(cmdWindowTxt);
            }
           
        }

        private void buttonSyntax_Click(object sender, EventArgs e)
        {

        }

        private void drawOutput_Click(object sender, EventArgs e)
        {
            

        }

        private void drawOutput_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(DrawingBitmap, 0, 0);

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            cmdWindow.Text = cmdParser.OpenFile();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            cmdParser.SaveFile(cmdWindowTxt);
        }


    }
}
