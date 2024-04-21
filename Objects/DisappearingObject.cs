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
        public DisappearingObject(float x, float y, float angle) : base(x, y, angle) { }
        // Метод для отрисовки объекта
        public override void Render(Graphics g)
        {
            // Если цвет - истина, заполняем круг белым цветом, иначе - зеленым
            if (color)
            {
                g.FillEllipse(new SolidBrush(Color.White), -size / 2, -size / 2, size, size); 
            }
            else
            {
                g.FillEllipse(new SolidBrush(Color.Green), -size / 2, -size / 2, size, size);
            }
        }
        // Метод для получения графического пути объекта
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-size/2, -size / 2, size, size);
            return path;
        }
    
    }
}
