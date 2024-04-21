using lab_5.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace lab_5.Objects
{
    public class Player : BaseObject
    {
        public Player(float x, float y, float angle) : base(x, y, angle) { }
        public Action<Marker> onMarkerOverlap;
        public Action<DisappearingObject> onDisappearingObjectOverlap;
        public float vX, vY;

        public override void Render(Graphics g)
        {
            if (color)
            {
                g.FillEllipse(new SolidBrush(Color.White), -15, -15, 30, 30);
                g.DrawEllipse(new Pen(Color.White), -15, -15, 30, 30);
                g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
            }
            else
            {
                g.FillEllipse(new SolidBrush(Color.DeepSkyBlue), -15, -15, 30, 30);
                g.DrawEllipse(new Pen(Color.Black, 2), -15, -15, 30, 30);
                g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is Marker)
            {
                onMarkerOverlap(obj as Marker);
            }

            else if (obj is DisappearingObject)
            {
                onDisappearingObjectOverlap(obj as DisappearingObject);
            }
        }
    }
}
