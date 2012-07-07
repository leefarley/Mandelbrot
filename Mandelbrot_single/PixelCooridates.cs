using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot_single
{
    class PixelCooridates
    {
        public int pixelX { get; set; }
        public int pixelY { get; set; }
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
