using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using lab_5.Objects;

namespace lab_5
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        int score = 0; // Переменная для хранения счета
        private Random random = new Random();
        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                string logMessage = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n";

                // Доступ к элементу управления из другого потока
                txtLog.Invoke(new Action(() =>
                {
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

            // добавляем новый объект DisappearingAndReappearingObject
            objects.Add(new DisappearingObject(200, 200, 0));
            objects.Add(new DisappearingObject(300, 300, 0));
            objects.Add(new DisappearingObject(400, 400, 0));
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
                    if (obj is DisappearingObject)
                    {
                        score++; // Увеличиваем счетчик очков при пересечении с объектом DisappearingAndReappearingObject
                        txtScore.Text = $"Счет: {score}";
                    }
                }
            }

            // рендерим DisappearingAndReappearingObject первым
            foreach (var obj in objects)
            {
                if (obj is DisappearingObject)
                {
                    g.Transform = obj.GetTransform();
                    obj.Render(g);
                }
            }

            // рендерим все объекты, кроме DisappearingObject
            foreach (var obj in objects)
            {
                if (obj != marker && !(obj is DisappearingObject))
                {
                    g.Transform = obj.GetTransform();
                    obj.Render(g);
                }
            }

            // рендерим маркер отдельно, чтобы он был "сверху"
            if (marker != null)
            {
                g.Transform = marker.GetTransform();
                marker.Render(g);
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

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;

                // Проверка пересечения с объектами
                foreach (var obj in objects)
                {
                    if (obj != player && player.Overlaps(obj, pbMain.CreateGraphics()))
                    {
                        player.Overlap(obj);
                        obj.Overlap(player);
                    }

                    // Проверка пересечения с зеленым объектом
                    if (obj is DisappearingObject && player.Overlaps(obj, pbMain.CreateGraphics()))
                    {
                        (obj as DisappearingObject).DisappearAndReappear(pbMain.Width, pbMain.Height);
                    }
                }
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
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
