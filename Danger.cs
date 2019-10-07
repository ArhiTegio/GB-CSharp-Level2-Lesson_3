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
    class Danger : ICollision
    {
        protected Bitmap bit;
        protected byte[] bitByte;
        protected PointGrath pos;
        protected Speed dir;
        protected PointGrath size;
        protected Screen screen;

        public Danger(PointGrath pos, Speed dir, PointGrath size, Screen screen)
        {
            this.screen = screen;
            this.pos = pos;
            this.dir = dir;
            this.size = size;

            bit = (Bitmap)Image.FromFile("network.png");
            //bitByte = Elements.;
        }
        /// <summary>
        /// Построить объект
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public virtual bool Draw(Random r)
        {
            //Построение круга
            GL.Color3(Color.Black);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.TriangleFan);
            var b = 16f;
            for (var i = 0; i <= b; i++)
            {
                var a = (float)i / b * 3.1415f * 2.00f;
                GL.Vertex2(pos.X + Math.Cos(a) * size.X, pos.Y + Math.Sin(a) * size.Y);
            }
            GL.End();

            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i <= b; i++)
            {
                var a = (float)i / b * 3.1415f * 2.00f;
                GL.Vertex2(pos.X + Math.Cos(a) * size.X, pos.Y + Math.Sin(a) * size.Y);
                GL.Vertex2(pos.X + Math.Cos(a) * size.X - dir.X, pos.Y + Math.Sin(a) * size.Y - dir.Y);
            }
            GL.End();


            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(pos.X - dir.X * 8, pos.Y - dir.Y * 8);
            GL.Vertex2(pos.X - dir.X * 20, pos.Y - dir.Y * 20);
            GL.End();

            return Update(r);
        }


        /// <summary>
        /// Обновить параметры объекта
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public virtual bool Update(Random r)
        {
            pos.X += (int)(Math.Round(dir.X, 3));
            pos.Y += (int)(Math.Round(dir.Y, 3));
            if (pos.X < 0 + (size.X / 2)) dir.X = -dir.X;
            if (pos.X > screen.Width - size.X) dir.X = -dir.X;
            if (pos.Y < 0 + (size.Y / 2)) dir.Y = -dir.Y;
            if (pos.Y > screen.Height - size.Y) dir.Y = -dir.Y;

            if (pos.Y > (screen.Height - size.Y / 2) + 2 || (0 - 1) > pos.Y ||
                pos.X > (screen.Width - size.X / 2) + 2 || (0 - 1) > pos.X)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Построение растрового изображения в одном цвете
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void Print2D(float x, float y)
        {
            GL.RasterPos2(x, y);
            if (bitByte == null || bitByte.Length == 0)
                bitByte = GenerateBytesArray(bit);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            GL.Bitmap((int)(bit.Width),
                (int)(bit.Height),
                bit.Width / 2, bit.Height / 2, 0, 0, bitByte);
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

        /// <summary>
        /// Проверка на пересечение двух объектова
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        /// <summary>
        /// Получить расположение и габариты объекта
        /// </summary>
        public Rectangle Rect => new Rectangle((int)pos.X * 1000, (int)pos.Y * 1000, (int)size.X * 1000, (int)size.Y * 1000);
    }
}
