using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5.Objects
{
    public class DisappearingObject : BaseObject
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
            timer.Tick += Timer_Tick; // Подписываемся на событие таймера
            timer.Start(); // Запускаем таймер
        }
        // Метод, вызываемый при срабатывании таймера
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
        // Метод для перемещения объекта и сброса счетчика
        public void DisappearAndReappear(int width, int height)
        {
            countdown = 65; // Сброс счетчика на начальное значение
            x = new Random().Next(width);
            y = new Random().Next(height);
        }
        // Метод для отрисовки объекта
        public override void Render(Graphics g)
        {
            // Если цвет - истина, заполняем круг белым цветом, иначе - зеленым
            if (color)
            {
                g.FillEllipse(new SolidBrush(Color.White), -size / 2, -size / 2, size, size);
                g.DrawString(
                countdown.ToString(),
                new Font("Verdana", 8),
                new SolidBrush(Color.White),
                -size / 2 + 15,
                size / 2 + 5
            );
            }
            else
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
        }
        // Метод для получения графического пути объекта
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-size/2, -size / 2, size, size);
            return path;
        }
        // Переопределенный метод для обработки пересечения с другим объектом
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj); // Вызываем базовую реализацию метода пересечения
        }

    }
}
