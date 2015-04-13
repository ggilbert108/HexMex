using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Logic
{
    static class Util
    {
        public static Random random = new Random();

        public static T getRandomElement<T>(List<T> list)
        {
            int index = random.Next(list.Count);
            return list[index];
        }
    }
}
