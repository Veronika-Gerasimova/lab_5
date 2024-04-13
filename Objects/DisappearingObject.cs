using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace lab_5.Objects
{
    class DisappearingObject : BaseObject
    {
        private Random random = new Random();
        private int countdown = 5000; // 5 секунд
        private bool counting = false;

        public DisappearingObject(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -15, -15, 30, 30);
            //g.DrawEllipse(new Pen(Color.Black, 2), -10, -10, 20, 20);

            // Отрисовка текста с отсчетом времени
            if (counting)
            {
                g.DrawString(
                    (countdown / 1000).ToString(),
                    new Font("Verdana", 8),
                    new SolidBrush(Color.Green),
                    -10, -10
                );
            }
        }

        public void StartCountdown()
        {
            counting = true;
            Task.Delay(5000).ContinueWith(_ =>
            {
                counting = false;
                countdown = 5000;
                DisappearAndReappear(800, 600);
            });
            Task.Run(() =>
            {
                while (counting && countdown > 0)
                {
                    Task.Delay(1000).Wait();
                    countdown -= 1000;
                }
            });
        }

        public void DisappearAndReappear(int width, int height)
        {
            X = new Random().Next(width);
            Y = new Random().Next(height);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = new GraphicsPath();
            path.AddEllipse(-10, -10, 20, 20);
            return path;
        }
    }

}

