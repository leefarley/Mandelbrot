using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot_threaded
{
    /// <summary>
    /// This class is responsible for storing the pixel cooridinates to draw
    /// and the fractal coordinates that are to be calculated to find pixel colour
    /// </summary>
    class PixelCooridates
    {
        // the x and y coorinates of the pixel to be drawn
        public int pixelX { get; set; }
        public int pixelY { get; set; }

        // the position of the fractal to be calculated 
        public double fractalX { get; set; }
        public double fractalY { get; set; }

        public PixelCooridates(int newPixelX, int newPixelY, double newFractalX, double newFractalY)
        {
            pixelX = newPixelX;
            pixelY = newPixelY;
            fractalX = newFractalX;
            fractalY = newFractalY;
        }
    }
}
