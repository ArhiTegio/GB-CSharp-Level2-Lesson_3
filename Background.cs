using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Тест_OpenTK
{
    class Background
    {
        protected PointGrath pos;
        protected Speed dir;
        protected PointGrath size;
        protected Screen screen;

        public Background(PointGrath pos, Speed dir, PointGrath size, Screen screen)
        {
            this.screen = screen;
            this.pos = pos;
            this.dir = dir;
            this.size = size;
        }
        public virtual bool Draw(Random r)
        {
            return Update(r);
        }

        public virtual bool Update(Random r)
        {
            return true;
        }
    }
}
