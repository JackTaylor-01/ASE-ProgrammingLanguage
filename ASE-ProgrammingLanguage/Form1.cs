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
            MessageBox.Show("cmdWindowTxt = " + cmdWindowTxt);
            MessageBox.Show("cmdLineTxt = " + cmdLineTxt);
            drawOutput.Invalidate(); //invalidate needed as form needs repainting
            //Following switch case used for testing Drawer class functionality

            switch (cmdLineTxt)
            {
                case "DrawLine":
                    new CommandParser(drawer);
                    //drawer.DrawLine(160, 120);
                    break;
                case "MoveTo":
                    drawer.MoveTo(50, 50);
                    break;

                case "DrawTo":
                    drawer.DrawTo(200, 200);
                    break;

                case "Clear":
                    drawer.Clear();
                    break;

                case "Reset":
                    drawer.Reset();
                    break;

                case "DrawRectangle":
                    drawer.DrawRectangle(30, 40);
                    break;

                case "DrawCircle":
                    drawer.DrawCircle(25);
                    break;

                case "DrawTriangle":
                    drawer.DrawTriangle(50);
                    break;

                case "SetPenColour":
                    drawer.SetPenColour(Color.Red);
                    break;

                case "SetBrushColour":
                    drawer.SetBrushColour(Color.Blue);
                    break;

                case "EnableFill":
                    drawer.EnableFill();
                    break;

                case "DisableFill":
                    drawer.DisableFill();
                    break;

                default:
                    Console.WriteLine($"Unknown command: {cmdLineTxt}");
                    break;
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
            Console.WriteLine(cmdParser.OpenFile());
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }


    }
}
