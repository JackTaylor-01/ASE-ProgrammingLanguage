using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Class is used for drawing lines and shapes on a picture box
    /// </summary>
    public class Drawer
    {
        Graphics g;
        public Pen pen;
        public int xPos, yPos;
        Brush brush;
        public bool fill = false;
        /// <summary>
        /// Initialises <see cref="Drawer"/> class
        /// </summary>
        /// <param name="g"> where everything is drawn </param>
        public Drawer(Graphics g)
        {
            this.g = g;
            pen = new Pen(Color.Black, 5);
            xPos = yPos = 0;
            brush = Brushes.Black;

        }
        /// <summary>
        /// Draws line on the picturebox using the pen between 2 vectors.
        /// </summary>
        /// <param name="toX"> The horizontal componenet of the vector where the line is being drawn to</param>
        /// <param name="toY"> The vertical component of the vector where the line is being drawn to</param>
        public void DrawLine(int toX, int toY)
        {
            g.DrawLine(pen, xPos, yPos, toX, toY);
        }
        /// <summary>
        /// Moves the pen to specified vector, allowing functions in drawer class to paint anywhere on the picturebox
        /// </summary>
        /// <param name="x"> Horizontal compoenent of the vector for the pens new location</param>
        /// <param name="y"> Vertical component of the vector for the pens new location</param>
        public void MoveTo(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Draws line on the picture box using the pen between 2 vectors and set the pen to vector that was drawn to
        /// </summary>
        /// <param name="toX"> Horizontal componenet of the vector for the pens new location</param>
        /// <param name="toY"> Vertical component of the vector for the pens new location</param>
        public void DrawTo(int toX, int toY)
        {
            g.DrawLine(pen, xPos, yPos, toX, toY);
            xPos = toX;
            yPos = toY;
        }
        /// <summary>
        /// Clears the picturebox of all drawings setting the background to 'WhiteSmoke'
        /// </summary>
        public void Clear()
        {
            g.Clear(Color.WhiteSmoke);
        }
        /// <summary>
        /// Resets the pens vector to 0,0
        /// </summary>
        public void Reset()
        {
            xPos = yPos = 0;
        }
        /// <summary>
        /// Draws rectangle at pen location
        /// </summary>
        /// <param name="width"> is the width of the rectangle</param>
        /// <param name="height"> is the height of the rectangle</param>
        public void DrawRectangle(int width, int height)
        {
            if (fill)
            {
                g.FillRectangle(brush, xPos, yPos, width, height);
            }
            else
            {
                g.DrawRectangle(pen, xPos, yPos, width, height);
            }

        }
        /// <summary>
        /// Draws circle around the pen. With the pen location being the centre of the circle
        /// </summary>
        /// <param name="radius"> Radius of the circle</param>
        public void DrawCircle(int radius)
        {
            if (fill)
            {
                g.FillEllipse(brush, xPos - radius, yPos - radius, radius * 2, radius * 2);
            }
            else
            {
                g.DrawEllipse(pen, xPos - radius, yPos - radius, radius * 2, radius * 2);
            }
        }
        /// <summary>
        /// Draws triangle 
        /// </summary>
        /// <param name="sideLength"> Length of the sides of the triangle</param>
        public void DrawTriangle(int sideLength)
        {
            Point[] trianglePoints = {
                new Point(xPos, yPos - sideLength / 2),
                new Point(xPos - sideLength / 2, yPos + sideLength / 2),
                new Point(xPos + sideLength / 2, yPos + sideLength / 2)
            };

            if (fill)
            {
                g.FillPolygon(brush, trianglePoints);
            }
            else
            {
                g.DrawPolygon(pen, trianglePoints);
            }
        }
        /// <summary>
        /// Sets the colour of the pen
        /// </summary>
        /// <param name="colour"> The colour of the pen</param>
        public void SetPenColour(String colour)
        {
            pen.Color = Color.FromName(colour);
        }
        /// <summary>
        /// Sets the colour of the brush
        /// </summary>
        /// <param name="colour">The colour of the brush</param>
        public void SetBrushColour(String colour)
        {
            brush = new SolidBrush(Color.FromName(colour));
        }
        /// <summary>
        /// Enables fill
        /// </summary>
        public void EnableFill()
        {
            fill = true;
            pen.Brush = brush;
        }
        /// <summary>
        /// Disables fill
        /// </summary>
        public void DisableFill()
        {
            fill = false;
            //pen.Brush = Brushes.Transparent;
        }


    }
}
