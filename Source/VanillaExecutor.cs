using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RWCustom;

namespace ColorfulCadas
{
    /// <summary>
    /// This piece of code is responsible for simulating vanilla color pickup. Initially I thought of just picking several colors manually
    /// for the vanilla list but got slapped, so here we are
    /// Unlike vanilla code, this one gets executed using system.random (UnityEngine.Random in vanilla case), so it won't perfectly imitate vanilla picker
    /// but the principle is pretty much the same. Probs would fix it later, if anyone decides to look into code
    /// There's a throw exception thing here, it's pretty much useless unless someone goes wild at enum. Idk it's just there, alright?
    /// If someone does decide to do that, well, I have no catch method
    /// </summary>
    class VanillaExecutor
    {
        private static float NonVanillaRandomDeviation(System.Random random, float k)
        {
            return Custom.SCurve((float)RandomWeight.GetRandomDouble(random) * 0.5f, k) * 2f * (((float)RandomWeight.GetRandomDouble(random) >= 0.5f) ? -1f : 1f);
        }
        public static float NonVanillaClampedRandomVariation(float baseValue, float maxDeviation, float k, System.Random random)
        {
            return Mathf.Clamp(baseValue + NonVanillaRandomDeviation(random, k) * maxDeviation, 0f, 1f);
        }
        public static Color GetVanillaColor(bool gender, Framework.CicadaColorType colorType, RoomPalette palette, System.Random random)
        {
            HSLColor color = new HSLColor(NonVanillaClampedRandomVariation(0.55f, 0.1f, 0.5f, random), 1f, 0.5f);
            switch (colorType)
            {
                case Framework.CicadaColorType.main:
                    {
                        if(gender)
                        {
                            return Color.Lerp(HSLColor.Lerp(color, new HSLColor(color.hue, 0f, 1f), 0.8f).rgb, palette.fogColor, 0.1f);
                        }
                        else
                        {
                            return Color.Lerp(HSLColor.Lerp(color, new HSLColor(color.hue, 1f, 0f), 0.8f).rgb, palette.blackColor, 0.85f);
                        }
                    }
                case Framework.CicadaColorType.secondary:
                    {
                        if(gender)
                        {
                            HSLColor hslcolor = new HSLColor(color.hue, 0.5f, 0.4f);
                            return Color.Lerp(color.rgb, hslcolor.rgb, 0.8f);
                        }
                        else
                        {
                            HSLColor hslcolor2 = new HSLColor(color.hue, 0.5f, 0.5f);
                            return Color.Lerp(color.rgb, hslcolor2.rgb, 0.4f);
                        }
                    }
                case Framework.CicadaColorType.eyes:
                {
                    if(gender)
                    {
                        return Color.Lerp(color.rgb, palette.blackColor, 0.8f);
                    }
                    else
                    {
                        return color.rgb;
                    }
                }
                case Framework.CicadaColorType.pupils:
                    {
                        if(gender)
                        {
                            return color.rgb;
                        }
                        else
                        {
                            return palette.blackColor;
                        }
                    }
                default: throw new ArgumentException($"CadaColors VanillaExecutor found an unexpected colortype enum");
            }
        }
    }
}
