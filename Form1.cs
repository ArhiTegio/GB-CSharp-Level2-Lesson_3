using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform.Windows;

namespace Тест_OpenTK
{
    public partial class Form1 : Form
    {
        event Action<string, double, double> LogEvent;

        int score = 0;
        bool finish = false;
        // размеры окна 
        Screen screen = new Screen();
        // отношения сторон окна визуализации
        // для корректного перевода координат мыши в координаты, 
        // принятые в программе 
        private float devX;
        private float devY;
        // массив, который будет хранить значения x,y точек графика 
        private float[,] GrapValuesArray;
        // количество элементов в массиве 
        private int elements_count = 0;
        // флаг, означающий, что массив с значениями координат графика пока еще не заполнен 
        private bool not_calculate = true;
        // номер ячейки массива, из которой будут взяты координаты для красной точки, 
        // для визуализации текущего кадра 
        private int pointPosition = 0;
        // вспомогательные переменные для построения линий от курсора мыши к координатным осям 
        float lineX, lineY;
        // текущие координаты курсора мыши 
        float Mcoord_X = 0, Mcoord_Y = 0;

        private static Dictionary<string, Tuple<byte[], SizeF>> LookUp = new Dictionary<string, Tuple<byte[], SizeF>>();

        List<Background> backgrounds = new List<Background>();
        List<Danger> dangerous = new List<Danger>();
        List<Bullet> bullet = new List<Bullet>();
        List<Medicine> medicine = new List<Medicine>();

        Player player;

        GLControl glControl;

        Random r = new Random();

        public Form1()
        {
            InitializeComponent();
            OpenTK.Toolkit.Init();

            this.SuspendLayout();
            glControl.CreateControl();
            glControl.Width = Width - 16;
            glControl.Height = Height - 63;
            glControl.Location = new Point(0, 24);
            this.Controls.Add(glControl);
            glControl_Load();
            glControl.MouseMove += glControl_MouseMove;
            glControl.KeyDown += Form1_KeyDown;
            devX = (float)screen.Width / (float)glControl.Width;
            devY = (float)screen.Height / (float)glControl.Height;

            glControl.Load += control_Load;
            glControl.Paint += control_Paint;
            glControl.MouseDown += new MouseEventHandler(Form1_MouseDown);
            FPS.Start();
        }

        private void control_Paint(object sender, PaintEventArgs e) => glControl.SwapBuffers();

        /// <summary>
        /// Загрузить параметры заднего вида
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>
        /// Загрузить матрицу и представление видового экрана OpenGL
        /// </summary>
        private void glControl_Load()
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            if ((float)glControl.Width <= (float)glControl.Height) screen = new Screen() { Width = Math.Min(Height, Width) * 10, Height = Math.Min(Height, Width) * 10 * (float)glControl.Height / (float)glControl.Width };
            else screen = new Screen() { Width = Math.Min(Height, Width) * 10 * (float)glControl.Width / (float)glControl.Height, Height = Math.Min(Height, Width) * 10 };
            GL.Ortho(0.0, Math.Min(Height, Width) * 10 * (float)glControl.Width / (float)glControl.Height, 0.0, Math.Min(Height, Width) * 10, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        /// <summary>
        /// Получить управляющие параметры мышки относительно позиции на форме в пересчете на систему координат видового экрана OpenGL 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Mcoord_X = e.X;
            Mcoord_Y = e.Y;
            lineX = devX * e.X;
            lineY = (float)(screen.Height - devY * e.Y);
        }

        /// <summary>
        /// Загрузка всех объектов для видового экрана
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (50 > glControl.Width || glControl.Width > 1000 ||
                50 > glControl.Height || glControl.Height > 1000)
                throw new GlWindowException();

            var sizeStar = 50;
            var countStar = 500;
            for (int i = 0; i < countStar; ++i)
            {
                var rv = r.Next(sizeStar / 2, sizeStar);
                backgrounds.Add(new Star(
                    new PointGrath(r.Next(0 + sizeStar, (int)screen.Width - sizeStar),
                    r.Next(0 + sizeStar, (int)screen.Height - sizeStar)),
                    new Speed(-20 + r.NextDouble() * 15, -4 + r.NextDouble() * 8),
                    new PointGrath(rv, rv), screen));
            }

            backgrounds.Add(new Earth(new PointGrath(1000, 1000), new Speed(0, 0), new PointGrath(650, 650), screen));

            var size = 100;
            var count = 30;
            for (int i = 0; i < count; i++)
                dangerous.Add(new Danger(new PointGrath((int)screen.Height - size, (i + 1) * size),
                    new Speed(6 + r.NextDouble() * 16, 6 + r.NextDouble() * 24),
                    new PointGrath(size, size), screen));

            player = new Player(new PointGrath(0, 0), new Speed(0, 0), new PointGrath(250, 250), screen);

            medicine.Add(new Medicine(new PointGrath((screen.Width / 2) + r.Next(-2000, 2000), (screen.Height / 2) + r.Next(-2000, 2000)), new Speed(0, 0), new PointGrath(60, 60), screen));

            player.MessageDie += Finish;
            LogEvent += Log.LogConsole;
            LogEvent += Log.LogFile;
        }

        /// <summary>
        /// Плохое завершение игры
        /// </summary>
        public void Finish()
        {
            finish = true;
            LogEvent?.Invoke("Игрок проиграл", player.Pos.X, player.Pos.Y);
            //Сообщение
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            GL.PushMatrix();
            GL.Translate(15, 15, 0);

            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(0, 0);
            GL.Vertex2(0, screen.Height);
            GL.Vertex2(screen.Width, screen.Height);
            GL.Vertex2(screen.Width, 0);
            GL.End();

            GL.Color3(Color.Wheat);
            TextGame.Print2DText(50, (float)(screen.Height - 200), "Вы проиграли! Мир захватил электронный разум, вышедший из под контроля своих создателей.");
            TextGame.Print2DText(50, (float)(screen.Height - 350), "Неужели это конец?");
            TextGame.Print2DText(50, (float)(screen.Height - 500), "Мастер, готовы ли вы к перерождению во имя победы?");
            GL.PopMatrix();
            // дожидаемся завершения визуализации кадра 
            GL.Flush();
            // сигнал для обновление элемента реализующего визуализацию. 
            glControl.Invalidate();


        }
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void Form1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        /// <summary>
        /// Тоже самое, что в примере только стандартными средствами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FPS_Tick(object sender, EventArgs e)
        {
            if (finish) Finish();
            else DrawConsole();
        }

        /// <summary>
        /// Обновление матрицы видового экрана OpenGL
        /// </summary>
        private void DrawConsole()
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            GL.PushMatrix();
            GL.Translate(15, 15, 0);

            foreach (var b in backgrounds)
                b.Draw(r);

            for (int i = 0; i < bullet.Count; ++i)
            {
                if (!bullet[i].Draw(r, new PointGrath(0, 0)))
                    bullet.RemoveAt(i);
            }

            var boom = false;
            var size = 100;
            for (int i = 0; i < dangerous.Count; ++i)
            {
                for (int b = 0; b < bullet.Count; ++b)
                    if (dangerous[i].Collision(bullet[b]))
                    {
                        score++;
                        boom = true;
                        System.Media.SystemSounds.Hand.Play();
                        bullet.RemoveAt(b);
                    }

                for (int m = 0; m < medicine.Count; ++m)
                    if (dangerous[i].Collision(medicine[m]))
                    {
                        boom = true;
                        System.Media.SystemSounds.Hand.Play();
                        medicine[m] = new Medicine(new PointGrath((screen.Width / 2) + r.Next(-2000, 2000), (screen.Height / 2) + r.Next(-2000, 2000)), new Speed(0, 0), new PointGrath(60, 60), screen);
                    }

                if (dangerous[i].Collision(player))
                {
                    boom = true;
                    var damage = r.Next(1, 10);
                    LogEvent?.Invoke($"Столкновение с {dangerous[i].GetType()}. Вы получили урон в размере: {damage}", player.Pos.X, player.Pos.Y);
                    player.EnergyLow(damage);
                    score++;
                    System.Media.SystemSounds.Asterisk.Play();
                    if (player.Energy <= 0) player?.Die();
                }

                if (!boom && !dangerous[i].Draw(r))
                {
                    dangerous[i] = new Danger_lvl2(new PointGrath(r.Next((int)(screen.Width / 1.5), (int)(screen.Width - size)), r.Next((int)(0 - (size * 2)), (int)(screen.Height - (size * 2)))),
                        new Speed(6 + r.NextDouble() * 16, 6 + r.NextDouble() * 24),
                        new PointGrath(size, size), screen);
                }
                if (boom)
                {
                    LogEvent?.Invoke($"Враг {dangerous[i].GetType()} повержен", player.Pos.X, player.Pos.Y);
                    if (r.Next(0, 100) < 50)
                    {

                        dangerous[i] = new Danger_lvl2(new PointGrath(r.Next((int)(screen.Width / 1.5), (int)(screen.Width - size)), r.Next((int)(0 - (size * 2)), (int)(screen.Height - (size * 2)))),
                            new Speed(12 + r.NextDouble() * 64, 6 + r.NextDouble() * 24), new PointGrath(size, size), screen);
                        if (r.Next(0, 1000) > 900)
                            dangerous.Add(new Danger_lvl2(new PointGrath(r.Next((int)(screen.Width / 1.5), (int)(screen.Width - size)), r.Next((int)(0 - (size * 2)), (int)(screen.Height - (size * 2)))),
                            new Speed(12 + r.NextDouble() * 64, 6 + r.NextDouble() * 24), new PointGrath(size, size), screen));
                    }
                    else
                    {
                        dangerous[i] = new Danger(new PointGrath(r.Next((int)(0 + size), (int)(screen.Width / 4)), r.Next((int)(0 - (size * 2)), (int)(screen.Height - (size * 2)))),
                            new Speed(12 + r.NextDouble() * 64, 6 + r.NextDouble() * 24),
                            new PointGrath(size, size), screen);
                        if (r.Next(0, 1000) > 700)
                            dangerous.Add(new Danger(new PointGrath(r.Next((int)(0 + size), (int)(screen.Width / 4)), r.Next((int)(0 - (size * 2)), (int)(screen.Height - (size * 2)))),
                            new Speed(12 + r.NextDouble() * 64, 6 + r.NextDouble() * 24), new PointGrath(size, size), screen));
                    }

                    boom = false;
                }
            }

            player.Draw(r, new PointGrath(lineX, lineY));

            for (var i = 0; i < medicine.Count; ++i)
            {
                if (player.Collision(medicine[i]))
                {
                    player.Heal(medicine[i].Heal);
                    medicine[i] = new Medicine(new PointGrath((screen.Width / 2) + r.Next(-2000, 2000), (screen.Height / 2) + r.Next(-2000, 2000)), new Speed(0, 0), new PointGrath(60, 60), screen);
                }
                else if (!medicine[i].Draw(r, new PointGrath(lineX, lineY)))
                    medicine[i] = new Medicine(new PointGrath((screen.Width / 2) + r.Next(-2000, 2000), (screen.Height / 2) + r.Next(-2000, 2000)), new Speed(0, 0), new PointGrath(60, 60), screen);
            }

            GeneralizedDelegate<string> d = Summ<string>;
            GL.Color3(Color.Crimson);
            if (score < 5000)
                TextGame.Print2DText(50, (float)(screen.Height - 200), d("Марафон дикой аптечки! ",$"Марафон дикой аптечки! Вы разрушили {score} из 5000 астероидов."));
            else
                TextGame.Print2DText(50, (float)(screen.Height - 200), d("Марафон дикой аптечки выы выйграли!", $"Со счетом {score} астероидов."));            

            GL.PopMatrix();
            // дожидаемся завершения визуализации кадра 
            GL.Flush();
            // сигнал для обновление элемента реализующего визуализацию. 
            glControl.Invalidate();
        }

        /// <summary>
        /// Обобщенный делегат
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        delegate T GeneralizedDelegate<T>(T obj1, T obj2);

        private T Summ<T>(T obj1, T obj2) => (dynamic)obj1 + (dynamic)obj2;

        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();


        /// <summary>
        /// Нажатие клавиши мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LogEvent?.Invoke($"Выстрел пулей", player.Pos.X, player.Pos.Y);
                bullet.Add(new Bullet(new PointGrath(lineX + 39, lineY - 39/2), new Speed(100, 0), new PointGrath(250, 250), screen, 1));
            }
        }
        
        /// <summary>
        /// Нажатие клавиши клавиатуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)            
                bullet.Add(new Bullet(new PointGrath(lineX + 39, lineY - 39 / 2), new Speed(100, 0), new PointGrath(250, 250), screen, 1));            
            if (e.KeyCode == Keys.Up)
                player.Up();
            if (e.KeyCode == Keys.Down)
                player.Down();
        }

        private void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }
    }
}
