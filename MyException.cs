using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Тест_OpenTK
{
    class GlWindowException : ArgumentException
    {
        public GlWindowException()
        {
            Console.WriteLine(base.Message);
        }
    }

    class GameObjectException : Exception
    {
        public GameObjectException()
        {
            Console.WriteLine(base.Message);
        }
    }
}
