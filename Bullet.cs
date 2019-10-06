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
    class Bullet : BaseObject
    {
        Random r;
        public int Power { get; set; }

        public Bullet(PointGrath pos, Speed dir, PointGrath size, Screen screen, int power) : base(pos, dir, size, screen)
        {
            Power = power;
        }


        public override bool Draw(Random r, PointGrath p)
        {
            this.r = r;
            Print2D((float)Pos.X, (float)Pos.Y);
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
            GL.Color3(Color.Salmon);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(x , y);
            GL.Vertex2(x - 40, y - 20);
            GL.Vertex2(x - 40, y + 20);
            GL.End();


            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(x - 150, y);
            GL.Vertex2(x - 50, y - 20);
            GL.Vertex2(x - 50, y + 20);
            GL.End();
        }

        /// <summary>
        /// Обновить параметры объекта
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Update(Random r)
        {
            Pos.X += (int)(dir.X);
            Pos.Y += (int)(dir.Y);

            if (Pos.Y > (screen.Height - size.Y / 2) + 2 || (0 - 1) > Pos.Y ||
                Pos.X > (screen.Width - size.X / 2) + 2 || (0 - 1) > Pos.X)
                return false;
            else
                return true;
        }
    }
}
