using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Тест_OpenTK
{
    abstract class Background
    {
        protected PointGrath pos;
        protected Speed dir;
        protected PointGrath size;
        protected Screen screen;

        public Background(PointGrath pos, Speed dir, PointGrath size, Screen screen)
        {
            if (pos.X < 0 || pos.X > screen.Width || pos.Y < 0 || pos.Y > screen.Height ||
                size.X < 0 || size.X > screen.Width / 2 || size.Y < 0 || size.Y > screen.Height / 2)
                throw new GameObjectException();
            this.screen = screen;
            this.pos = pos;
            this.dir = dir;
            this.size = size;
        }
        
        /// <summary>
        /// Построить объект
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public virtual bool Draw(Random r)
        {
            return Update(r);
        }
        
        /// <summary>
        /// Обновить параметры объекта
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public virtual bool Update(Random r)
        {
            return true;
        }
    }
}
