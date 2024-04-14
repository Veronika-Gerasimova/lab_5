using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace lab_5.Objects
{
    class MovingBlackArea : BaseObject
    {
        private const float Speed = 3f;
        private const float Width = 200f;
        private const float Height = 400f;
        private bool isVisible = true;
     

        public event Action<MovingBlackArea, BaseObject> OnOverlap;
        public event Action<MovingBlackArea, BaseObject> OnExit;
        public MovingBlackArea(float x, float y) : base(-Width, y, 0)
        {
        }

        public override void Update()
        {
            X += Speed;
            if (X > Screen.PrimaryScreen.Bounds.Width)
            {
                X = -Width;
            }
        }

        public override void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Black), -Width, -Height, Width, Height);
            
        }
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if (!(obj is MovingBlackArea))
            {
                obj.Color = Color.White; // Изменяем цвет объекта на белый при попадании в область
            }
            OnOverlap?.Invoke(this, obj);
        }

        public override void Exit(BaseObject obj)
        {
            base.Exit(obj);
            if (!(obj is MovingBlackArea))
            {
                obj.Color = Color.Black; // Возвращаем цвет объекта на черный при выходе из области
            }
            OnExit?.Invoke(this, obj);
        }


    }
}
