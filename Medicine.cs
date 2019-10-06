using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Тест_OpenTK
{
    class Medicine : BaseObject, ICollision
    {
        public Medicine(PointGrath pos, Speed dir, PointGrath size, Screen screen) : base(pos, dir, size, screen)
        {
        }

        public override bool Draw(Random r, PointGrath p)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Random r)
        {
            throw new NotImplementedException();
        }
    }
}
