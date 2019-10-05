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
using Tao.FreeGlut;

namespace Тест_OpenTK
{
    public partial class Form1 : Form
    {
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
            glControl.Cursor = new Cursor(new IntPtr(9));
            glControl.Location = new Point(0,24);
            this.Controls.Add(glControl);
            glControl_Load();
            glControl.MouseMove += glControl_MouseMove;
            devX = (float)screen.Width / (float)glControl.Width;
            devY = (float)screen.Height / (float)glControl.Height;

            glControl.Load += control_Load;
            glControl.Paint += control_Paint;
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
            if ((float)glControl.Width <= (float)glControl.Height) screen = new Screen() { Width = Math.Min(Height, Width)*10, Height = Math.Min(Height, Width) * 10 * (float)glControl.Height / (float)glControl.Width };
            else screen = new Screen() { Width = Math.Min(Height, Width) * 10 * (float)glControl.Width / (float)glControl.Height, Height = Math.Min(Height, Width) * 10 };
            GL.Ortho(0.0, Math.Min(Height, Width) * 10 * (float)glControl.Width / (float)glControl.Height, 0.0, Math.Min(Height, Width) * 10, -1,1);
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

            backgrounds.Add(new Erath(new PointGrath(1000, 1000), new Speed(0, 0), new PointGrath(800, 800), screen));
            
            var size = 100;
            var count = 50;
            for (int i = 0; i < count; i++)
                dangerous.Add(new Danger(new PointGrath((int)screen.Height - size, (i + 1) * size), 
                    new Speed(6 + r.NextDouble() * 16, 6 + r.NextDouble() * 24), 
                    new PointGrath(size, size), screen));

            player = new Player(new PointGrath(0, 0), new Speed(0, 0), new PointGrath(250, 250), screen);
        }

        private void Form1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        /// <summary>
        /// Тоже самое, что в примере только стандартными средствами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FPS_Tick(object sender, EventArgs e) => DrawConsole();
        
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
            
            for (int i = 0; i < dangerous.Count; ++i)            
                if (!dangerous[i].Draw(r))
                {
                    var size = 100;
                    dangerous[i] = new Danger_lvl2(new PointGrath(r.Next((int)(screen.Width / 1.5), (int)(screen.Width - size)), r.Next((int)(0 - (size * 2)), (int)(screen.Height - (size * 2)))),
                        new Speed(6 + r.NextDouble() * 16, 6 + r.NextDouble() * 24),
                        new PointGrath(size, size), screen);
                }

            player.Draw(r, new PointGrath(lineX, lineY));

            GL.PopMatrix();
            // дожидаемся завершения визуализации кадра 
            GL.Flush();
            // сигнал для обновление элемента реализующего визуализацию. 
            glControl.Invalidate();
        }
        
        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }
    }
}
