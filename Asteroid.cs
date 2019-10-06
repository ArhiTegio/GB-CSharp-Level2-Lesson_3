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
    class Asteroid : BaseObject
    {
        public int Power { get; set; }

        public Asteroid(PointGrath pos, Speed dir, PointGrath size, Screen screen, int power) : base(pos, dir, size, screen)
        {
            Power = power;
        }

        /// <summary>
        /// Построить объект
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Draw(Random r, PointGrath p)
        {
            GL.Color3(Color.Red);
            Print2D((float)Pos.X - bit.Width / 2, (float)Pos.Y + bit.Height / 2);
            //return true;
            return Update(r);
        }

        /// <summary>
        /// Построение растрового изображения в одном цвете
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void Print2D(float x, float y)
        {
            //Построение круга
            GL.Color3(Color.Red);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i <= 25; i++)
            {
                var a = (float)i / 25.0f * 3.1415f * 2.00f;
                GL.Vertex2(Pos.X + Math.Cos(a) * size.X, Pos.Y + Math.Sin(a) * size.Y);
            }
            GL.End();
        }

        /// <summary>
        /// Обновить параметры объекта
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Update(Random r)
        {
            Pos.X += (int)dir.X;
            Pos.Y += (int)dir.Y;
            if (Pos.X < 0 + (size.X / 2)) dir.X = -dir.X;
            if (Pos.X > screen.Width - size.X) dir.X = -dir.X;
            if (Pos.Y < 0 + (size.Y / 2)) dir.Y = -dir.Y;
            if (Pos.Y > screen.Height - size.Y) dir.Y = -dir.Y;
            return false;
        }

    }
}
