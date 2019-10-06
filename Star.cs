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
    class Star : Background
    {
        int color = 0;
        public Star(PointGrath pos, Speed dir, PointGrath size, Screen screen) : base(pos, dir, size, screen)
        {
        }

        public override bool Draw(Random r)
        {
            color = 0;
            if (r.Next(0, 30) > 10)
                color = r.Next(160, 255);

            GL.Color3((byte)color, (byte)color, (byte)color);
            GL.LineWidth(0.3f);

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(pos.X + (size.X / 2)*2, screen.Height - pos.Y);
            GL.Vertex2(pos.X - size.X / 2, screen.Height - pos.Y);

            GL.Vertex2(pos.X, screen.Height - pos.Y + (size.X / 2));
            GL.Vertex2(pos.X, screen.Height - pos.Y - size.X / 2);
            GL.End();
            
            return Update(r);
        }

        public override bool Update(Random r)
        {
            pos.X = pos.X + Math.Round(dir.X, 0);
            if (pos.X < 0)
            {
                pos.X = screen.Width + size.X;
                pos.Y = (screen.Height - size.Y) - (r.NextDouble() * (screen.Height - (5 + size.Y)));
                dir.X = -20 + r.NextDouble() * 15;
                dir.Y = -4 + r.NextDouble() * 8;
            }
            return true;
        }

    }
}
