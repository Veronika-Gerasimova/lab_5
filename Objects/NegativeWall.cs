using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5.Objects
{
    public class NegativeWall : BaseObject
    {
        private float fieldHeight;
        private float fieldWidth;
        private float width = 1;
        private float maxWidth = 250;

        public NegativeWall(float x, float y, float angle, float height, float width) : base(x, y, angle)
        {
            this.fieldHeight = height;
            this.fieldWidth = width;
        }
        public Action<BaseObject> onObjectOverlap;

        public override void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Black), -width / 2, -y, width, fieldHeight);
            g.DrawRectangle(new Pen(Color.Black), -width / 2, -y, width, fieldHeight);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-width / 2, -y, width, fieldHeight);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj != null)
            {
                onObjectOverlap(obj);
            }
        }

        public void Move()
        {
            if (width < maxWidth && x < maxWidth / 2)
            {
                ++width;
                x = width / 2;
            }
            else if (x == fieldWidth)
            {
                x = 0;
                width = 1;
            }
            else if (width == maxWidth && x <= fieldWidth - maxWidth / 2)
            {
                ++x;
            }
            else if (x > fieldWidth - maxWidth / 2)
            {
                --width;
                ++x;
            }
        }
    }

}
