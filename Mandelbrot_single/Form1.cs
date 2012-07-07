using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Mandelbrot_single
{
    public partial class Form1 : Form
    {

        Mandelbrot mandelbrot;
        Graphics graphics;
        Bitmap buffer;
        double xmin, xmax, ymin, ymax;
        public delegate void UpdateProgress(object sender, EventArgs a);


        public Form1()
        {
            InitializeComponent();

            mandelbrot = new Mandelbrot();
            buffer = new Bitmap(Width, Height);
            graphics = this.CreateGraphics();

            UpdateProgress update = new UpdateProgress(updateProgress);
            mandelbrot.PercentChanged += new EventHandler(update);
        }

        private void btn_draw_Click(object sender, EventArgs e)
        {

            pbMandel.Value = 0;

            xmin = -2.1;
            xmax = 1.0;
            ymin = -1.3;
            ymax = 1.3;

            buffer = mandelbrot.DrawMandel(ClientSize.Width, ClientSize.Height, xmin, ymin, xmax, ymax);
            graphics.DrawEllipse(new Pen(Color.Black), 10, 10, 10, 10);
            //graphics.DrawImage(buffer, 0, 0, Width, Height);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            pbMandel.Value = 0;

            //find the new x and y from the button click
            double intigralX = (xmax - xmin) / ClientSize.Width;
            double intigralY = (ymax - ymin) / ClientSize.Height;

            double newx = (e.X * intigralX) - Math.Abs(xmin);
            double newy = (e.Y * intigralY) - Math.Abs(ymin);

            //find how far to draw from the center /3 is too add some zoom
            double mandelHalfWidth = (xmax - xmin)/5;
            double mandelHalfHeight = (ymax - ymin)/5;

            // alter coordinates to new zoom position
            xmax = (newx + mandelHalfWidth);
            xmin = (newx - mandelHalfWidth);
            ymax = (newy + mandelHalfHeight);
            ymin = (newy - mandelHalfHeight);


            buffer = mandelbrot.DrawMandel(ClientSize.Width, ClientSize.Height, xmin, ymin, xmax, ymax);
            graphics.DrawImage(buffer, 0, 0, Width, Height);
        }
        public void updateProgress(object sender, EventArgs a)
        {
            pbMandel.Value = mandelbrot.PercentDone;
        }


    }
}
