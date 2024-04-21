using lab_5.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace lab_5
{
    public partial class Form1 : Form
    {
        private static List<BaseObject> objects = new List<BaseObject>();
        private static List<BaseObject> negativeObjects = new List<BaseObject>();
        private Player player;
        private Marker marker;
        private DisappearingObject disappearingObject; 
        private BlackArea wall;
        private int scores = 0;
        private static Random rand = new Random(); //системное время

        public Form1()
        {
            InitializeComponent();
            updateScores();// Обновление счета
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0); // Создание игрока в центре формы
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            wall = new BlackArea(0, pbMain.Height / 2, 0, pbMain.Height, pbMain.Width);

            objects.Add(wall);// Добавление черной области в список объектов

            addDisappearingObject(); //добавление зеленого кружка

            // Обработчик события пересечения игрока с другим объектом
            player.onOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };
            // Обработчик события пересечения игрока с маркером
            player.onMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            // Обработчик события пересечения игрока с исчезающим объектом
            player.onDisappearingObjectOverlap += (t) =>
            {
                objects.Remove(t); //удалеение зеленого кружка из списка объектов
                t = null; //сброс ссылки на объект
                addDisappearingObject();
                ++scores;
                updateScores();
            };
            // Обработчик события пересечения черной области с другим объектом
            wall.onObjectOverlap += (o) =>
            {
                negativeObjects.Add(o);
            };

            objects.Add(marker);
            objects.Add(player);
        }
        // Метод для перерисовки главного окна
        private void pbMain_Paint(object sender, PaintEventArgs e)
        {

            var g = e.Graphics;// Получение графического контекста

            g.Clear(Color.White);// Очистка окна

            updatePlayer();// Обновление позиции игрока
            negativeObjects.Clear();
            // Пересчет пересечений между объектами
            foreach (var obj1 in objects.ToList())
            {
                foreach (var obj2 in objects.ToList())
                {
                    if (obj1 != obj2 && obj1.Overlaps(obj2, g))
                    {
                        obj1.Overlap(obj2);// Обработка пересечения
                    }
                }
            }
            // Отрисовка объектов
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.color = negativeObjects.Contains(obj);// Установка цвета объекта в зависимости от его наличия в списке "отрицательных" объектов
                obj.Render(g);
            }
        }
        // Метод для обновления позиции игрока
        public void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.x - player.x;
                float dy = marker.y - player.y;

                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.x += dx * 2;
                player.y += dy * 2;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.angle = (float)(90 - Math.Atan2(player.vX, player.vY) * 180 / Math.PI);


            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.x += player.vX;
            player.y += player.vY;
        }
        // Метод для обработки события таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();// Перерисовка окна
        }
        // Метод для обработки события щелчка мыши
        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.x = e.X;// Установка координаты X маркера
            marker.y = e.Y;
        }

        private void addDisappearingObject()
        {

            var d = 70; //размер зеленого кружка

            var x = rand.Next() % (pbMain.Width - d) + d;
            var y = rand.Next() % (pbMain.Height - d) + d;
            this.disappearingObject = new DisappearingObject(x, y, 0);

            objects.Add(disappearingObject);
        }
        // Метод для обновления счета
        private void updateScores()
        {
            txtScore.Text = "Очки: " + scores;
        }
        // Метод для обработки события таймера черной области
        private void wallTimer_Tick(object sender, EventArgs e)
        {
            wall.Move(); // Перемещение черной области
        }
    }
}
