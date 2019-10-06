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
    abstract class BaseObject : ICollision
    {
        protected Bitmap bit;
        protected byte[] bitByte;
        protected PointGrath pos;
        protected Speed dir;
        protected PointGrath size;
        protected Screen screen;

        public BaseObject(PointGrath pos, Speed dir, PointGrath size, Screen screen)
        {
            this.screen = screen;
            this.Pos = pos;
            this.dir = dir;
            this.size = size;
        }

        /// <summary>
        /// Построить объект
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public abstract bool Draw(Random r, PointGrath p);

        /// <summary>
        /// Обновить параметры объекта
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public abstract bool Update(Random r);

        /// <summary>
        /// Проверка на пересечение двух объектова
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        /// <summary>
        /// Получить расположение и габариты объекта
        /// </summary>
        public Rectangle Rect => new Rectangle((int)Pos.X * 1000, (int)Pos.Y * 1000, (int)size.X * 1000, (int)size.Y * 1000);

        public PointGrath Pos { get => pos; set => pos = value; }
    }

    public delegate void Message();
}
