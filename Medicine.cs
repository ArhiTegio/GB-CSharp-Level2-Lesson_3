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
    /// <summary>
    /// Дикая аптечка
    /// </summary>
    class Medicine : BaseObject, ICollision
    {
        protected int heal = 1;
        protected int speed = 10;
        PointGrath p;

        public Medicine(PointGrath pos, Speed dir, PointGrath size, Screen screen) : base(pos, dir, size, screen)
        {
            var r = new Random();
            speed = r.Next(200, 300);
        }

        /// <summary>
        /// Получить уровень лечение
        /// </summary>
        public int Heal { get => heal; }

        /// <summary>
        /// Построить объект
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Draw(Random r, PointGrath p)
        {
            this.p = p;
            heal = r.Next(1, 5);
            GL.Color3(Color.White);
            GL.LineWidth(1);

            GL.Begin(PrimitiveType.TriangleFan);
            var b = 16f;
            for (var i = 0; i <= b; i++)
            {
                var a = (float)i / b * 3.1415f * 2.00f;
                GL.Vertex2(pos.X + Math.Cos(a) * 60, pos.Y + Math.Sin(a) * 60);
            }
            GL.End();

            var g = 10;
            var v = 40;
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2((float)(pos.X + g), (float)(pos.Y + g));
            GL.Vertex2((float)(pos.X + v), (float)(pos.Y + g));
            GL.Vertex2((float)(pos.X + v), (float)(pos.Y - g));
            GL.Vertex2((float)(pos.X + g), (float)(pos.Y - g));
            GL.Vertex2((float)(pos.X + g), (float)(pos.Y - v));
            GL.Vertex2((float)(pos.X - g), (float)(pos.Y - v));
            GL.Vertex2((float)(pos.X - g), (float)(pos.Y - g));
            GL.Vertex2((float)(pos.X - v), (float)(pos.Y - g));
            GL.Vertex2((float)(pos.X - v), (float)(pos.Y + g));
            GL.Vertex2((float)(pos.X - g), (float)(pos.Y + g));
            GL.Vertex2((float)(pos.X - g), (float)(pos.Y + v));
            GL.Vertex2((float)(pos.X + g), (float)(pos.Y + v));
            GL.Vertex2((float)(pos.X + g), (float)(pos.Y + g));
            GL.End();

            return Update(r);
        }

        /// <summary>
        /// Обновить параметры объекта
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Update(Random r)
        {
            pos.X += (int)(speed * ((-100 + r.Next(1, 200))/100.0));
            pos.Y += (int)(speed * ((-100 + r.Next(1, 200))/100.0));
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
    }
}
