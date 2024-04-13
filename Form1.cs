using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using lab_5.Objects;

namespace lab_5
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                string logMessage = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n";

                // Доступ к элементу управления из другого потока
                txtLog.Invoke(new Action(() => {
                    txtLog.Text = logMessage + txtLog.Text;
                }));
            };


            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            objects.Add(marker);

            objects.Add(player);
           // objects.Add(new MyRectangle(50, 50, 0));
            //objects.Add(new MyRectangle(100, 100, 45));

            // добавляем новый объект DisappearingObject
            objects.Add(new DisappearingObject(200, 200, 0));
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);
            updatePlayer();
            // пересчитываем пересечения
            foreach (var obj in objects.ToArray())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.X += dx * 2f; // увеличил скорость движения для наглядности
                player.Y += dy * 2f; // увеличил скорость движения для наглядности

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(dx, dy) * 180 / MathF.PI;

                // Проверка пересечения с объектами
                foreach (var obj in objects)
                {
                    if (obj != player && player.Overlaps(obj, pbMain.CreateGraphics()))
                    {
                        player.Overlap(obj);
                        obj.Overlap(player);
                    }
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            //updatePlayer();
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // а это так и остается
            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
