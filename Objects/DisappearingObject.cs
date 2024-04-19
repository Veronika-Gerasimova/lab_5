using lab_5.Objects;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace lab_5.Objects
{
    class DisappearingObject : BaseObject
    {
        private const int InitialSize = 30; // Начальный размер круга
        private int size = InitialSize; // Текущий размер круга
        private int countdown = 65; // Начальное значение счетчика
        private System.Windows.Forms.Timer timer;
        private PictureBox pbMain;

        public DisappearingObject(float x, float y, float angle, PictureBox pbMain) : base(x, y, angle)
        {
            this.pbMain = pbMain;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 65;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            countdown--;
            if (countdown <= 0)
            {
                countdown = 65; // Сброс счетчика на начальное значение
                DisappearAndReappear(200, 200); // Перемещаем объект
            }
            // Перерисовываем объект, чтобы обновить отображение счетчика
            pbMain.Invalidate();
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -size / 2, -size / 2, size, size);
            g.DrawString(
                countdown.ToString(),
                new Font("Verdana", 8),
                new SolidBrush(Color.Green),
                -size / 2 + 15,
                size / 2 + 5
            );
        }

        public void DisappearAndReappear(int width, int height)
        {
            countdown = 65; // Сброс счетчика на начальное значение
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


