using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5.Objects
{
  public class BaseObject
    {
        public float x;
        public float y;
        public float angle;
        public bool color;

        public Action<BaseObject, BaseObject> onOverlap;
        public Action<BaseObject> dontObjectOverlap;

        public BaseObject(float x, float y, float angle)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
        }

        public virtual void Render(Graphics g)
        {
        }

        public Matrix GetTransform()
        {
            var matrix = new Matrix();
            matrix.Translate(this.x, this.y);
            matrix.Rotate(this.angle);

            return matrix;
        }

        public virtual GraphicsPath GetGraphicsPath()
        {
            return new GraphicsPath();
        }

        public virtual bool overlaps(BaseObject obj, Graphics g)
        {
            var path1 = this.GetGraphicsPath();
            var path2 = obj.GetGraphicsPath();

            path1.Transform(this.GetTransform());
            path2.Transform(obj.GetTransform());

            var region = new Region(path1);
            region.Intersect(path2);
            return !region.IsEmpty(g);
        }

        public virtual void Overlap(BaseObject obj) 
        {
            if (this.onOverlap != null)
            {
                this.onOverlap(this, obj);
            }
        }

        public void DontOverlap(BaseObject obj)
        {
            if (obj != null)
            {
                dontObjectOverlap(obj);
            }
        }
    }
    }
