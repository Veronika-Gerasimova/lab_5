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
        private int size = InitialSize;
        public DisappearingObject(float x, float y, float angle) : base(x, y, angle) { }
        public Action<DisappearingObject> onDisappearingObjectOverlap;

        public override void Render(Graphics g)
        {
            if (color)
            {
                g.FillEllipse(new SolidBrush(Color.White), -size / 2, -size / 2, size, size); 
            }
            else
            {
                g.FillEllipse(new SolidBrush(Color.Green), -size / 2, -size / 2, size, size);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-size/2, -size / 2, size, size);
            return path;
        }
    
    }
}
