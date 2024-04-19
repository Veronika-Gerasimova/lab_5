using lab_5.Objects;


namespace lab_5
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        int score = 0; // ���������� ��� �������� �����
        private MovingBlackArea blackArea;
        private readonly Dictionary<BaseObject, Color> originalColors = new();

        public Form1()
        {
            InitializeComponent();
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                string logMessage = $"[{DateTime.Now:HH:mm:ss:ff}] ����� ��������� � {obj}\n";

                // ������ � �������� ���������� �� ������� ������
                txtLog.Invoke(new Action(() =>
                {
                    txtLog.Text = logMessage + txtLog.Text;
                }));
            };

            // ������� ������� �� ����������� � ��������
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            objects.Add(marker);

            objects.Add(player);

            // ��������� ����� ������� DisappearingObject � ��������� pbMain
            objects.Add(new DisappearingObject(200, 200, 0, pbMain));
            objects.Add(new DisappearingObject(300, 300, 0, pbMain));
            blackArea = new MovingBlackArea(0, 0, objects);
            blackArea.OnEnter += (obj) =>
            {
                if (obj is BaseObject baseObject)
                {
                    baseObject.Color = Color.White;
                }
            };

            blackArea.OnExit += (obj) =>
            {
                if (obj is BaseObject baseObject)
                {
                    if (originalColors.ContainsKey(baseObject))
                    {
                        baseObject.Color = originalColors[baseObject];
                    }
                }
            };

            objects.Add(blackArea);

        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
                      var g = e.Graphics;

            g.Clear(Color.White);
            updatePlayer();
            // ������������� �����������
            foreach (var obj in objects.ToArray())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                    if (obj is DisappearingObject)
                    {
                        score++; // ����������� ������� ����� ��� ����������� � �������� DisappearingAndReappearingObject
                        txtScore.Text = $"����: {score}";
                    }
                }
            }
            foreach (var obj in objects)
            {
                if (obj is MovingBlackArea)
                {
                    g.Transform = obj.GetTransform();
                    obj.Render(g);
                }
            }
            foreach (var obj in objects)
            {
                if (obj is Player)
                {
                    g.Transform = obj.GetTransform();
                    obj.Render(g);
                }
            }
            foreach (var obj in objects)
            {
                if (obj is DisappearingObject)
                {
                    g.Transform = obj.GetTransform();
                    obj.Render(g);
                }
            }
            // �������� ������ ��������, ����� �� ��� "������"
            if (marker != null)
            {
                g.Transform = marker.GetTransform();
                marker.Render(g);
            }
            blackArea.Update(g);
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

                // �������� ����������� � ���������
                foreach (var obj in objects)
                {
                    if (obj != player && player.Overlaps(obj, pbMain.CreateGraphics()))
                    {
                        player.Overlap(obj);
                        obj.Overlap(player);
                    }

                    // �������� ����������� � ������� ��������
                    if (obj is DisappearingObject && player.Overlaps(obj, pbMain.CreateGraphics()))
                    {
                        (obj as DisappearingObject).DisappearAndReappear(pbMain.Width, pbMain.Height);
                    }

                    // �������� ���������� ������ � ������ �������
                    if (obj is MovingBlackArea && player.Overlaps(obj, pbMain.CreateGraphics()))
                    {
                        if (player.IsInBlackArea == false)
                        {
                            player.Enter(obj as MovingBlackArea, pbMain.CreateGraphics());
                        }
                    }

                    else
                    {
                        if (player.IsInBlackArea == true)
                        {
                            player.Exit(obj as MovingBlackArea);
                        }
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
            pbMain.Invalidate();
        }
        
        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            marker.X = e.X;
            marker.Y = e.Y;
        }

    }
}
