using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Тест_OpenTK
{
    class Speed
    {
        private double x = 0;
        private double y = 0;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public Speed(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    class PointGrath
    {
        private double x = 0;
        private double y = 0;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public PointGrath(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    
    class Screen
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

}
