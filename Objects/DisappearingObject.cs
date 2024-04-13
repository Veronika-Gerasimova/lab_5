using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace lab_5.Objects
{
    class DisappearingObject : BaseObject
    {
        private Random random = new Random();

        public DisappearingObject(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -15, -15, 30, 30);
            //g.DrawEllipse(new Pen(Color.Black, 2), -10, -10, 20, 20);
        }

        public void DisappearAndReappear(int width, int height)
        {
            X = random.Next(width);
            Y = random.Next(height);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = new GraphicsPath();
            path.AddEllipse(-10, -10, 20, 20);
            return path;
        }
    }

}

