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
    
    class Danger_lvl2 : Danger
    {
        public Danger_lvl2(PointGrath pos, Speed dir, PointGrath size, Screen screen) : base(pos, dir, size, screen)
        {
        }

        /// <summary>
        /// Построить объект
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Draw(Random r)
        {
            GL.Color3(Color.Yellow);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.LineLoop);
            int n = 5;                            // число вершин
            double R = size.X / 2, R2 = size.X;   // радиусы
            double alpha = r.Next(0, 100);        // поворот

            PointF[] points = new PointF[2 * n + 1];
            double a = alpha, da = Math.PI / n, l;
            for (int k = 0; k < 2 * n + 1; k++)
            {
                l = k % 2 == 0 ? R2 : R;
                GL.Vertex2((float)(pos.X + l * Math.Cos(a)), (float)(pos.Y + l * Math.Sin(a)));
                a += da;
            }
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
            pos.X += (int)(Math.Round(dir.X, 3) + r.NextDouble() * 1);
            pos.Y += (int)(Math.Round(dir.Y, 3) + r.NextDouble() * 1);
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
