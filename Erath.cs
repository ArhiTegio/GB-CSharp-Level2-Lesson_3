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
    class Erath : Background
    {
        public Erath(PointGrath pos, Speed dir, PointGrath size, Screen screen) : base(pos, dir, size, screen)
        {
        }

        public override bool Draw(Random r)
        {
            GL.Color3(Color.Gray);
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.TriangleFan);
            for (var i = 0; i <= 50; i++)
            {
                var a = (float)i / 50.0f * 3.1415f * 2.00f;
                GL.Vertex2((pos.X + Math.Cos(a) * size.X), screen.Height - (pos.Y + Math.Sin(a) * size.X));
            }
            GL.End();
            return true;
        }
    }
}
