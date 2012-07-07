using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;


namespace Mandelbrot_threaded
{
    /// <summary>
    /// A class that draws the mandelbrot set to a bitmap. Also publishes the percent completion within a 
    /// single draw call.
    /// </summary>
    class Mandelbrot
    {
        private int percentDone = 0;
        public int PercentDone { get { return percentDone; } }


        Bitmap b;

        /// <summary>
        /// Clients can subscribe to this event to get notified when the PercentDone property gets updates.
        /// </summary>
        public event EventHandler PercentChanged;

        /// <summary>
        /// max_iterations changes the maximum number of iterations we perform before bailing out. Lower
        /// values give faster results, but less accurate pictures - zooming in on a picture with a low
        /// max_iterations will fail very quickly. 5000 is a good compromise between speed and accuracy.
        /// </summary>
        const double max_iterations = 5000;

        /// <summary>
        /// This is actually the *square* of the escape radius. I store it as a square because it saves
        /// a costly Math.sqrt call. 
        /// </summary>
        const int escape_radius = 4;

        // A range of colours to assign to each fractal iteration:
        Color[] colours = new Color[256];

        Queue<PixelCooridates> pixelsToCompute;
        Queue<Pixel> computedPixel;

        public Mandelbrot()
        {
            // fill colours with nice values - we can put whatever colours we want in here...
            // make the set look nice by picking nicer colours!
            for (int i = 0; i < 256; i++)
            {
                colours[i] = Color.FromArgb(i, i, i);
            }
        }

        double total;

        /// <summary>
        /// Draws a mandelbrot set to a bitmap. Code taken from:
        /// http://www.codeproject.com/KB/graphics/mandelbrot.aspx
        /// ...and then heavily adapted to make it work better in C# and make it a bit prettier.
        /// 
        /// The width & height parameters control the size of the output Bitmap. Be aware that larger images won't give you
        /// more accurate renderings, they'll just take up more memory. It is recommended that you set width and height
        /// to the size that you are going to render the imge to.
        /// 
        /// (xmin,ymin) and (xmax,ymax) define the points we are going to render. The default values capture the entire
        /// mandelbrot set. You can "zoom in" but setting these points to be closer together, and "zoom out" by setting them
        /// further apart. Eventually you will hit a wall: *either* you will run out of precision in the double data type,
        /// *or* the floating point rounding errors will grow larger than the actual result. At that point zooming in won't 
        /// give any better results.
        /// 
        /// See also: max_iterations and escape_radius above - tweaking these affect the output significantly.
        /// </summary>
        public Bitmap DrawMandel(int width, int height, double xmin = -2.1, double ymin = -1.3, double xmax = 1, double ymax = 1.3)
        {
            // set percentDone to 0, since we're about tos tart rendering the next frame:
            percentDone = 0;

            // total is used for % calculations
            total = width * height;

            // create new bitmap with the desired width & height.
            b = new Bitmap(width, height);


            // work out what step each pixel represents in X and Y dimensions.
            // if we wanted to keep aspect ratio the same we could snap these to the same value.
            double intigralX = (xmax - xmin) / width;
            double intigralY = (ymax - ymin) / height;


            double fractalX = xmin;
            double fractalY;
            PixelCooridates pixel;
            pixelsToCompute = new Queue<PixelCooridates>();
            //loop throught all the pixel positions in the bitmap and add them to the queue for proccessing
            for (int pixelX = 1; pixelX < width; pixelX++)
            {
                fractalY = ymin;
                for (int pixelY = 1; pixelY < height; pixelY++)
                {
                    pixel = new PixelCooridates(pixelX, pixelY, fractalX, fractalY);
                    pixelsToCompute.Enqueue(pixel);
                    fractalY += intigralY;
                }
                fractalX += intigralX;
            }
            total = pixelsToCompute.Count;
            computedPixel = new Queue<Pixel>();
            Thread[] threads = new Thread[System.Environment.ProcessorCount];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(drawPixels);
                threads[i].Start();
            }

            while (pixelsToCompute.Count > 0)
            {
                double newPercent = (total - pixelsToCompute.Count) / total;
                newPercent *= 100;
                if (PercentChanged != null && newPercent != percentDone)
                {
                    percentDone = (int)newPercent;
                    PercentChanged(this, new EventArgs());

                }
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }           

            return b;
        }

        public void drawPixels()
        {
            PixelCooridates pixel;
            while (pixelsToCompute.Count > 0)
            {
                lock(pixelsToCompute)
                {
                    pixel = pixelsToCompute.Dequeue();
                }
                drawNextPixel(pixel);
            }
        }

        public void drawNextPixel(PixelCooridates pixel)
        {
            // here (x,y) == the point inside the complex plane we want to render.
            double x = pixel.fractalX;
            double y = pixel.fractalY;
            // and (x1,y1) is the starting point for all calculations
            double x1 = 0;
            double y1 = 0;

            // keep track of number of iterations so far:
            int iterations = 0;

            // keep iterating until EITHER we've exceeded our max iterations, OR we've 
            // escaped beyond the escape radius of the set.
            //
            // in complex terms, 'Z' is made up of x1,y1 - the real part of z is x1, the imaginary part of
            // 'Z' is y1.
            while (iterations < max_iterations && ((x1 * x1) + (y1 * y1)) < escape_radius)
            {
                iterations++;
                double xtemp = (x1 * x1) - (y1 * y1) + x;
                y1 = 2 * x1 * y1 + y;
                x1 = xtemp;
            }

            Color c;
            if (iterations == max_iterations)
            {
                // if we went for so long and never escaped:
                c = Color.White;
            }
            else
            {
                // otherwise, do two more iterations (helps reduce error)
                iterations++;
                double xtemp = (x1 * x1) - (y1 * y1) + x;
                y1 = 2 * x1 * y1 + y;
                x1 = xtemp;

                iterations++;
                xtemp = (x1 * x1) - (y1 * y1) + x;
                y1 = 2 * x1 * y1 + y;
                x1 = xtemp;

                // then grab a number based on the number of iterations performed before the value escaped:
                double v = ((iterations + 1) - Math.Log(Math.Log(Math.Sqrt((x1 * x1) + (y1 * y1)))) / Math.Log(2.0));
                c = colours[(int)v % colours.Length];
            }
            lock (b)
            {
                b.SetPixel(pixel.pixelX, pixel.pixelY, c);
            }
            
        }
    }
}
