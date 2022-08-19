using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorfulCadas
{/// <summary>
/// This code file is responsible for adding rarity parameter to colors. Each color can optionally have a double type parameter (by default it is 1.0d),
/// the bigger percentage a color takes of whole pie, the bigger the chances are for it to be chosen
/// </summary>
    class RandomWeight
    {
        public static double GetRandomDouble(Random random)
        {
            return ((double)random.Next(1000000))/1000000d;
        }
        public static double GetTotalWeight(bool gender, Framework.CicadaColorType cicadaColorType)
        {
            double c = 0d;
            foreach (ColorData colorData in Framework.CicadaColorSettings[gender][cicadaColorType])
                c += colorData.weight;
            return c;
        }
        public static UnityEngine.Color GetColor(Framework.CicadaColorType cicadaColorType, Random random, CicadaGraphics self, RoomPalette palette)
        {
            bool gender = self.cicada.gender;
            double choice = GetTotalWeight(gender, cicadaColorType) * GetRandomDouble(random);
            double counter = 0d;
            foreach (ColorData colorData in Framework.CicadaColorSettings[gender][cicadaColorType])
            {
                counter += colorData.weight;
                UnityEngine.Debug.Log("CadaColors Counter");
                if(counter>=choice)
                {
                    UnityEngine.Debug.Log($"CadaColors random = {random}, palette = {palette}");
                    return colorData.GetColor(random, palette);
                }
            }
            return Framework.Fallback(cicadaColorType, self, palette);
        }
    }
}
