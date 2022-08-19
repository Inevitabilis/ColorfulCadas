using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorfulCadas
{
    /// <summary>
    /// I was too lazy to search if there's color by RGB ints available, so I made one myself. Feel free to use
    /// </summary>
    public class ColorOverride
    {
        public static UnityEngine.Color ColorByRGB(int R, int G, int B)
        {
            return new UnityEngine.Color(R / 256f, G / 256f, B / 256f);
        }
    }
}
