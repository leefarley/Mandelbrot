using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mandelbrot_threaded
{
    class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color colour { get; set; }


        public Pixel(int newX, int newY, Color newColour)
        {
            X = newX;
            Y = newY;
            colour = newColour;
        }
    }
}
