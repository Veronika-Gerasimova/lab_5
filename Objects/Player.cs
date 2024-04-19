using lab_5.Objects;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5.Objects
{
    class Player : BaseObject
    {
        public Action<Marker> OnMarkerOverlap;
        public float vX, vY;
        private List<BaseObject> objects;

        public Player(float x, float y, float angle) : base(x, y, angle)
        {

        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.DeepSkyBlue), -15, -15, 30, 30);
            g.DrawEllipse(new Pen(Color.Black, 2), -15, -15, 30, 30);
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-3, -3, 6, 6);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if (obj is Marker)
            {
                OnMarkerOverlap(obj as Marker);
            }
        }
        public void Enter(MovingBlackArea area, Graphics g)
        {
            IsInBlackArea = true;
            foreach (var obj in objects)
            {
                if (obj != this && obj is DisappearingObject && Overlaps(obj, g))
                {
                    (obj as DisappearingObject).Color = Color.White; 
                }
            }
        }


    }
}
