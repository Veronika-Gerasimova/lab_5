using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace lab_5.Objects
{
    class MovingBlackArea : BaseObject
    {
        private const float Speed = 2f;
        private const float Width = 200f;
        private const float Height = 400f;
        private readonly Dictionary<BaseObject, Color> originalColors = new();
        private List<BaseObject> objects;

        public event Action<BaseObject> OnEnter;
        public event Action<BaseObject> OnExit;


        public MovingBlackArea(float x, float y, List<BaseObject> objects) : base(x, y, 0)
        {
            this.objects = objects;
        }

        public override void Update(Graphics g)
        {
            base.Update(g);

            // Move the black area
            X += Speed;
            if (X > Width)
            {
                X = -Width;
            }

            // Log which objects are in or out of the black area
            foreach (var obj in objects)
            {
                if (obj != this && Overlaps(obj, g))
                {
                    if (!objectsInBlackArea.Contains(obj))
                    {
                        Console.WriteLine($"{obj} entered the black area.");
                    }
                    objectsInBlackArea.Add(obj);
                }
                else
                {
                    if (objectsInBlackArea.Contains(obj))
                    {
                        Console.WriteLine($"{obj} exited the black area.");
                    }
                    objectsInBlackArea.Remove(obj);
                }
            }

            // NeedsUpdate(); // This line may not be necessary anymore
        }


        public override void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Black), X, 0, Width, Height);
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if (!(obj is Player || obj is Marker || obj is DisappearingObject))
            {
                Enter(obj);
            }
        }

        private readonly HashSet<BaseObject> objectsInBlackArea = new HashSet<BaseObject>();

        public void Enter(BaseObject obj)
        {
            if (!originalColors.ContainsKey(obj))
            {
                originalColors[obj] = obj.Color;
            }

            if (!(obj is Player || obj is Marker) && !objectsInBlackArea.Contains(obj))
            {
                obj.Color = Color.White; // Change object color to white
                objectsInBlackArea.Add(obj);
                OnEnter?.Invoke(obj); // Raise the OnEnter event
            }
            else if (objectsInBlackArea.Contains(obj) && obj.Color != Color.White)
            {
                obj.Color = Color.White; // Ensure object stays white if already in black area
            }
        }

        public void Exit(BaseObject obj)
        {
            base.Exit(obj);

            if (originalColors.ContainsKey(obj))
            {
                obj.Color = originalColors[obj];
                originalColors.Remove(obj);
            }

            if (objectsInBlackArea.Contains(obj))
            {
                objectsInBlackArea.Remove(obj);
                OnExit?.Invoke(obj); // Raise the OnExit event
            }
        }

        internal bool NeedsUpdate()
        {
            foreach (var obj in objectsInBlackArea.ToList())
            {
                if (!Overlaps(obj, null))
                {
                    Exit(obj);
                }
                else if (obj.Color != Color.White)
                {
                    Enter(obj);
                }
            }

            return false; // Метод необходим только для обновления объектов в черной области
        }

    }
}
