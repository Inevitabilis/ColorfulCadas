using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorfulCadas;
using BepInEx;
using UnityEngine;

namespace CadaColors1
{
    [BepInDependency("Inevitabilis.ColorfulCadas")]
    [BepInPlugin("Inevitabilis.ColorfulCadasList1", "CC: Guilded Cadas", "0.0.1")]
    public class ColorsList : BaseUnityPlugin
    {
        public void OnEnable()
        {
            Framework.addcolor(Framework.CicadaColorType.secondary, Framework.SetForGender.all, new SimpleColorData(ColorOverride.ColorByRGB(255, 219, 78), 0.15f));
        }
    }
}
