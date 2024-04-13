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
            g.DrawEllipse(new Pen(Color.Green, 2), -15, -15, 30, 30);
        }

        public override bool Overlaps(BaseObject obj, Graphics g)
        {
            if (obj is Player)
            {
                // Пересечение с игроком
                // Перемещаем объект на новое место
                X = random.Next(0, 300);
                Y = random.Next(0, 300);
                return false; // Возвращаем false, чтобы объект не считался пересекающимся
            }
            return base.Overlaps(obj, g);
        }

    }
}
