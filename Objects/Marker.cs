using lab_5.Objects;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5.Objects
{
    public class Marker : BaseObject
    {
        public Marker(float x, float y, float angle) : base(x, y, angle) { }

        public override void Render(Graphics g)
        {
            if (color)
            {
                g.FillEllipse(new SolidBrush(Color.White), -3, -3, 6, 6);
                g.DrawEllipse(new Pen(Color.White), -6, -6, 12, 12);
                g.DrawEllipse(new Pen(Color.White, 2), -10, -10, 20, 20);
            }
            else
            {
                g.FillEllipse(new SolidBrush(Color.Red), -3, -3, 6, 6);
                g.DrawEllipse(new Pen(Color.Red, 2), -6, -6, 12, 12);
                g.DrawEllipse(new Pen(Color.Red, 2), -10, -10, 20, 20);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-3, -3, 6, 6);
            return path;
        }
    }
}
