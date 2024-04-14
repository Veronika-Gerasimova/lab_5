using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

namespace lab_5.Objects
{
    class DisappearingObject : BaseObject
    {
        private const int InitialSize = 30; // Начальный размер круга
        private const int MinSize = 5; // Минимальный размер круга

        private int size = InitialSize; // Текущий размер круга
       private bool shrinking = false; // Флаг для отслеживания уменьшения круга
        private bool moving = false; // Флаг для отслеживания перемещения круга
        private int countdown = 5000; // 5 секунд
        private bool counting = false;

        public DisappearingObject(float x, float y, float angle) : base(x, y, angle)
        {
        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -size / 2, -size / 2, size, size);
           
        }

        public void DisappearAndReappear(int width, int height)
        {
            X = new Random().Next(width);
            Y = new Random().Next(height);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = new GraphicsPath();
            path.AddEllipse(-size / 2, -size / 2, size, size);
            return path;
        }


        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
        }
    }
}
