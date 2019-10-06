using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Тест_OpenTK
{
    class Player : BaseObject, ICollision 
    {
        protected Bitmap spaceship;
        protected Bitmap spaceship2;
        protected byte[] spaceshipByte;
        protected byte[] spaceshipByte2;

        private int _energy = 100;

        private int bullet = 10;
        public int Energy => _energy;
        public int Bullet => bullet;

        public event Message MessageDie;
        public void EnergyLow(int n)
        {
            _energy -= n;
        }

        public Player(PointGrath pos, Speed dir, PointGrath size, Screen screen) : base(pos, dir, size, screen)
        {
            if (pos.X < 0 || pos.X > screen.Width || pos.Y < 0 || pos.Y > screen.Height ||
                size.X < 0 || size.X > screen.Width / 2 || size.Y < 0 || size.Y > screen.Height / 2)
                throw new GameObjectException();

            this.screen = screen;
            this.Pos = pos;
            this.dir = dir;
            this.size = size;
            spaceship = (Bitmap)Image.FromFile("rocket2.png");
            spaceship2 = (Bitmap)Image.FromFile("rocket.png");
        }
        
        /// <summary>
        /// Построить объект
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Draw(Random r, PointGrath p)
        {
            Pos = p;
            GL.Color3(Color.Black);
            PrintSpaceShip2D((float)p.X - spaceship2.Width / 2, (float)p.Y + spaceship2.Height / 2,
                spaceshipByte2, spaceship2);

            GL.Color3(Color.Honeydew);
            PrintSpaceShip2D((float)p.X - spaceship.Width / 2 + 1, (float)p.Y + spaceship.Height / 2 + 4,
                spaceshipByte, spaceship);

            var l = r.Next(50, 250);
            var color = r.Next(160, 255);

            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            var a = 10;
            GL.Color3((byte)color, (byte)(color/1.5), (byte)0);
            GL.Vertex2(p.X - 150, (float)p.Y + a);
            GL.Vertex2(p.X - 150 - l, (float)p.Y + a);
            GL.Color3((byte)color/ 1.2, (byte)0, (byte)0);
            GL.Vertex2(p.X - 155, (float)p.Y + 10 + a);
            GL.Vertex2(p.X - 155 - l / 1.3, (float)p.Y + 10 + a);
            GL.Vertex2(p.X - 155, (float)p.Y - 10 + a);
            GL.Vertex2(p.X - 155 - l / 1.3, (float)p.Y - 10 + a);
            GL.Vertex2(p.X - 150 - l/1.2, (float)p.Y + a);
            GL.Vertex2(p.X - 150 - l, (float)p.Y + a);

            GL.Color3((byte)color, (byte)(color / 1.5), (byte)0);
            GL.Vertex2(p.X - 140, (float)p.Y + a - 45);
            GL.Vertex2(p.X - 140 - l/1.5, (float)p.Y + a - 45);
            GL.Color3((byte)color / 1.2, (byte)0, (byte)0);
            GL.Vertex2(p.X - 145, (float)p.Y + 10 + a - 45);
            GL.Vertex2(p.X - 145 - l / 1.8, (float)p.Y + 10 + a - 45);
            GL.Vertex2(p.X - 145, (float)p.Y - 10 + a - 45);
            GL.Vertex2(p.X - 145 - l / 1.8, (float)p.Y - 10 + a - 45);
            GL.Vertex2(p.X - 140 - l / 1.8, (float)p.Y + a - 45);
            GL.Vertex2(p.X - 140 - l / 1.5, (float)p.Y + a - 45);

            GL.Color3((byte)color, (byte)(color / 1.5), (byte)0);
            GL.Vertex2(p.X - 140, (float)p.Y + a + 50);
            GL.Vertex2(p.X - 140 - l / 1.5, (float)p.Y + a + 50);
            GL.Color3((byte)color / 1.2, (byte)0, (byte)0);
            GL.Vertex2(p.X - 145, (float)p.Y + 10 + a + 50);
            GL.Vertex2(p.X - 145 - l / 1.8, (float)p.Y + 10 + a + 50);
            GL.Vertex2(p.X - 145, (float)p.Y - 10 + a + 50);
            GL.Vertex2(p.X - 145 - l / 1.8, (float)p.Y - 10 + a + 50);
            GL.Vertex2(p.X - 140 - l / 1.8, (float)p.Y + a + 50);
            GL.Vertex2(p.X - 140 - l / 1.5, (float)p.Y + a + 50);
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(p.X + 250, p.Y - 240);
            GL.Vertex2(p.X + 250, p.Y - 300);
            GL.Vertex2(p.X - 250, p.Y - 300);
            GL.Vertex2(p.X - 250, p.Y - 240);
            GL.End();


            if (_energy > 0)
            {
                GL.Begin(PrimitiveType.TriangleFan);
                var c = Color.Green;
                GL.Color3((byte)(255 - (255 * (_energy / 100.0))),
                    (byte)(255 * (_energy / 100.0)), c.B);
                GL.Vertex2(p.X - 240 + (485 * (_energy / 100.0)), p.Y - 245);
                GL.Vertex2(p.X - 240 + (485 * (_energy / 100.0)), p.Y - 295);
                GL.Color3(Color.Red);
                GL.Vertex2(p.X - 240, p.Y - 295);
                GL.Vertex2(p.X - 240, p.Y - 245);
                GL.End();
            }

            GL.Color3(Color.White);
            //TextGame.Print2DText((float)(p.X - 250), (float)(p.Y - 300), $"100/{_energy}");

            return true;
        }

        /// <summary>
        /// Обновить параметры объекта
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Update(Random r)
        {
            Pos.X += (int)(Math.Round(dir.X, 3) + r.NextDouble() * 1);
            Pos.Y += (int)(Math.Round(dir.Y, 3) + r.NextDouble() * 1);
            if (Pos.X < 0 + (size.X / 2)) dir.X = -dir.X;
            if (Pos.X > screen.Width - size.X) dir.X = -dir.X;
            if (Pos.Y < 0 + (size.Y / 2)) dir.Y = -dir.Y;
            if (Pos.Y > screen.Height - size.Y) dir.Y = -dir.Y;

            if (Pos.Y > (screen.Height - size.Y / 2) + 2 || (0 - 1) > Pos.Y ||
                Pos.X > (screen.Width - size.X / 2) + 2 || (0 - 1) > Pos.X)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Построение растрового изображения в одном цвете
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void PrintSpaceShip2D(float x, float y, byte[] _spaceshipByte, Bitmap _spaceship)
        {
           
            GL.RasterPos2(x, y);
            if (_spaceshipByte == null || _spaceshipByte.Length == 0)
                _spaceshipByte = GenerateBytesArray(_spaceship);
            //var text = spaceshipByte.Select(n => n.ToString() + ",").Aggregate((n, b) => n + b);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            GL.Bitmap((int)(_spaceship.Width),
                (int)(_spaceship.Height),
                _spaceship.Width / 2, _spaceship.Height / 2, 0, 0, _spaceshipByte);
        }
        
        /// <summary>
        /// Преобразовать растровое изображение в битовый массив
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private byte[] GenerateBytesArray(Bitmap bmp)
        {
            var binary = new char[bmp.Width];
            var list = new List<byte>();
            for (int y = bmp.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < bmp.Width; ++x)
                {
                    if (bmp.GetPixel(x, y).R == Color.White.R &&
                        bmp.GetPixel(x, y).G == Color.White.G &&
                        bmp.GetPixel(x, y).B == Color.White.B &&
                        bmp.GetPixel(x, y).A == Color.White.A) binary[x] = '0';
                    else binary[x] = '1';
                }
                var str = new string(binary);
                var number = (int)(str.Length / 8.0);
                if (str.Length % 8.0 != 0) number++;

                byte[] bytes = new byte[number];
                int c = 8;
                for (int i = 0; i < number; ++i)
                {
                    if (str.Length - 8 * i < 8) c = str.Length - 8 * i;
                    list.Add(Convert.ToByte(str.Substring(8 * i, c), 2));
                }
            }
            return list.ToArray();
        }

        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < screen.Height) Pos.Y = Pos.Y + dir.Y;
        }
        public void Die()
        {
            MessageDie?.Invoke();
        }

        /// <summary>
        /// Получить расположение и габариты объекта
        /// </summary>
        public Rectangle Rect => new Rectangle((int)(
            Pos.X - spaceship.Width / 2 + 1) * 1000, 
            (int)(Pos.Y + spaceship.Height / 2 + 4) * 1000, (int)size.X * 1000, (int)size.Y * 1000);
    }
}
