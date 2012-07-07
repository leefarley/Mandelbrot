using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mandelbrot_threaded;

namespace Mandelbrot_threaded.Test
{
    class Test_PixelCooridates
    {
        const int pixelX = 5;
        const int pixelY = 6;
        const double fractalX = 0.2756;
        const double fractalY = 0.89443;
        PixelCooridates newPixelLocation;
        [SetUp]
        public void test_setup()
        {
            newPixelLocation = new PixelCooridates(pixelX, pixelY, fractalX, fractalY);
        }

        [Test]
        public void test_initPixelCoordinates()
        {
            Assert.IsNotNull(newPixelLocation);
        }

        [Test]
        public void test_AssignsCorrectValues()
        {
            Assert.AreEqual(pixelX, newPixelLocation.pixelX);
            Assert.AreEqual(pixelY, newPixelLocation.pixelY);
            Assert.AreEqual(fractalX, newPixelLocation.fractalX);
            Assert.AreEqual(fractalY, newPixelLocation.fractalY);
        }
    }
}
