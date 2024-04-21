using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5.Objects
{
    public class BlackArea : BaseObject
    {
        private float fieldHeight;
        private float fieldWidth;
        private float width = 1;
        private float maxWidth = 250;

        public BlackArea(float x, float y, float angle, float height, float width) : base(x, y, angle)
        {
            this.fieldHeight = height;
            this.fieldWidth = width;
        }
        public Action<BaseObject> onObjectOverlap; // Событие, вызываемое при перекрытии с другим объектом
        // Метод для отрисовки черной области
        public override void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Black), -width / 2, -y, width, fieldHeight);
            g.DrawRectangle(new Pen(Color.Black), -width / 2, -y, width, fieldHeight);
        }
        // Метод для получения графического пути
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-width / 2, -y, width, fieldHeight);
            return path;
        }
        // Метод для обработки перекрытия с другим объектом
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj != null)
            {
                onObjectOverlap(obj);
            }
        }
        // Метод для перемещения черной области
        public void Move()
        {
            // Если текущая ширина меньше максимальной и x меньше половины максимальной ширины
            if (width < maxWidth && x < maxWidth / 2)
            {
                // Увеличение ширины и обновление позиции x
                ++width;
                x = width / 2;
            }
            // Если x равен ширине поля
            else if (x == fieldWidth)
            {
                // Сброс позиции x и ширины до начальных значений
                x = 0;
                width = 1;
            }
            // Если текущая ширина равна максимальной и x меньше или равен разности ширины поля и половины максимальной ширины
            else if (width == maxWidth && x <= fieldWidth - maxWidth / 2)
            {
                // Увеличение позиции x
                ++x;
            }
            // Если x больше разности ширины поля и половины максимальной ширины
            else if (x > fieldWidth - maxWidth / 2)
            {
                // Уменьшение ширины и увеличение позиции x
                --width;
                ++x;
            }
        }
    }

}
