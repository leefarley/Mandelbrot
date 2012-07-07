using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mandelbrot_single;
using System.Drawing;

namespace Mandelbrot_single.Test
{
    class Test_Mandelbrot
    {

        const int pixelX = 5;
        const int pixelY = 6;
        const double fractalX = 0.2756;
        const double fractalY = 0.89443;
        const int bitmapX = 200;
        const int bitmapY = 200;

        Mandelbrot testMandelbrot;
        [SetUp]
        public void test_setup()
        {
            testMandelbrot = new Mandelbrot();
        }

        [Test]
        public void test_initMandelbrot()
        {
            Assert.IsNotNull(testMandelbrot);
        }

        [Test]
        public void test_DrawMandel()
        {
           Bitmap b = testMandelbrot.DrawMandel(bitmapX, bitmapY);

           Assert.AreEqual(bitmapX, b.Width);
           Assert.AreEqual(bitmapY, b.Height);
        }

        [Test]
        public void test_percentDone()
        {
            int percent = testMandelbrot.PercentDone;
            Assert.AreEqual(0, percent);
        }
    }
}
