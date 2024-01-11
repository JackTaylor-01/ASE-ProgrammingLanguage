﻿using System;
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
    internal class Drawer
    {
        Graphics g;
        Pen pen;
        int xPos, yPos;
        Brush brush;

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

        public void Reset()
        {
            xPos = yPos = 0;
        }

        public void DrawRectangle(int width, int height)
        {
            g.DrawRectangle(pen, xPos, yPos, width, height);
        }

        public void DrawCircle(int radius)
        {
            g.DrawEllipse(pen, xPos - radius, yPos - radius, radius * 2, radius * 2);
        }

        public void DrawTriangle(int sideLength)
        {
            Point[] trianglePoints = {
                new Point(xPos, yPos - sideLength / 2),
                new Point(xPos - sideLength / 2, yPos + sideLength / 2),
                new Point(xPos + sideLength / 2, yPos + sideLength / 2)
            };

            g.DrawPolygon(pen, trianglePoints);
        }

        public void SetPenColour(Color colour)
        {
            pen.Color = colour;
        }

        public void SetBrushColour(Color colour)
        {
            brush = new SolidBrush(colour);
        }

        public void EnableFill()
        {
            pen.Brush = brush;
        }

        public void DisableFill()
        {
            pen.Brush = null;
        }


    }
}
