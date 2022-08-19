using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorfulCadas
{
    static class ListExtension
    {
        public static T GetRandomElement<T>(this List<T> list, Random random)
        {
            return list[random.Next(0, list.Count - 1)];
        }
    }
}
