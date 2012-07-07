using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Drawing;
using Mandelbrot_threaded;

namespace Mandelbrot_threaded.Test
{
    class Test_Pixel
    {
        const int pixelX = 5;
        const int pixelY = 6;
        Color pixelColor = Color.Black;

        Pixel testPixel;
        [SetUp]
        public void test_setup()
        {
            testPixel = new Pixel(pixelX, pixelY, pixelColor);
        }

        [Test]
        public void test_initilizePixel()
        {
            Assert.IsNotNull(testPixel);
        }

        [Test]
        public void test_AssignsCorrectValues()
        {
            Assert.AreEqual(pixelX, testPixel.X);
            Assert.AreEqual(pixelY, testPixel.Y);
            Assert.AreEqual(pixelColor, testPixel.colour);
        }

    }
}
