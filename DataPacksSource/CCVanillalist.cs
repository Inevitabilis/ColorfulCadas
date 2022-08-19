using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorfulCadas;
using BepInEx;

namespace CadaColorsVanilla
{
    [BepInDependency("Inevitabilis.ColorfulCadas")]
    [BepInPlugin("Inevitabilis.VanillaCadasList", "CC: Vanilla Cadas", "0.0.1")]
    public class CCVanillalist : BaseUnityPlugin
    {
        public void OnEnable()
        {
            Framework.addcolor(Framework.CicadaColorType.main, Framework.SetForGender.male, new VanillaColorData(Framework.CicadaColorType.main, true));
            Framework.addcolor(Framework.CicadaColorType.main, Framework.SetForGender.female, new VanillaColorData(Framework.CicadaColorType.main, false));
            Framework.addcolor(Framework.CicadaColorType.secondary, Framework.SetForGender.male, new VanillaColorData(Framework.CicadaColorType.secondary, true));
            Framework.addcolor(Framework.CicadaColorType.secondary, Framework.SetForGender.female, new VanillaColorData(Framework.CicadaColorType.secondary, false));
            Framework.addcolor(Framework.CicadaColorType.eyes, Framework.SetForGender.male, new VanillaColorData(Framework.CicadaColorType.eyes, true));
            Framework.addcolor(Framework.CicadaColorType.eyes, Framework.SetForGender.female, new VanillaColorData(Framework.CicadaColorType.eyes, false));
            Framework.addcolor(Framework.CicadaColorType.pupils, Framework.SetForGender.male, new VanillaColorData(Framework.CicadaColorType.pupils, true));
            Framework.addcolor(Framework.CicadaColorType.pupils, Framework.SetForGender.female, new VanillaColorData(Framework.CicadaColorType.pupils, false));
        }
    }
}
