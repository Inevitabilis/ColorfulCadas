using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyCSharp;
using BepInEx;
using UnityEngine;

namespace ColorfulCadas
{
    [BepInPlugin("Inevitabilis.ColorfulCadas", "Colorful Cadas Framework", "0.0.1")]
    public class Framework : BaseUnityPlugin
    {
		
		

		public static Dictionary<bool, Dictionary<CicadaColorType, List<ColorData>>> CicadaColorSettings =
			new Dictionary<bool, Dictionary<CicadaColorType, List<ColorData>>> { };                             //creating new dictionary. For every bool there is a bunch of color types, for every color type there is a list of colors
		public static Color Fallback(CicadaColorType colorType, CicadaGraphics self, RoomPalette palette)			//This is a reserve case function: what happens in case there is nothing in addressed bool and colortype: it takes vanilla color
        {
			Color color = GetDefaultColor(self, palette);
			switch(colorType)
            {
				case CicadaColorType.main:	return color;
				case CicadaColorType.secondary:
					if (self.cicada.gender)
					{
						HSLColor hslcolor = new HSLColor(self.cicada.iVars.color.hue, 0.5f, 0.4f);
						return Color.Lerp(color, hslcolor.rgb, 0.8f);
					}
					else
					{
						HSLColor hslcolor2 = new HSLColor(self.cicada.iVars.color.hue, 0.5f, 0.5f);
						return Color.Lerp(color, hslcolor2.rgb, 0.4f);
					}
				case CicadaColorType.eyes: return (self.cicada.gender ? Color.Lerp(color, palette.blackColor, 0.8f) : self.iVars.color.rgb);
				case CicadaColorType.pupils: return (self.cicada.gender ? self.iVars.color.rgb : palette.blackColor);
				default: throw new ArgumentException($"No fallback defined for {colorType}");
			}
		}
		
		#region addcolor
		public static void addcolor(CicadaColorType colorType, SetForGender gender, ColorData colorData)
        {
			if ((gender & SetForGender.male) > 0)	addcolor(colorType, true, colorData);
			if ((gender & SetForGender.female) > 0)   addcolor(colorType, false, colorData);
		}
		private static void addcolor(CicadaColorType colorType, bool gender, ColorData colorData)
        {
			if (!CicadaColorSettings.ContainsKey(gender))
			{
				CicadaColorSettings[gender] = new Dictionary<CicadaColorType, List<ColorData>> { };
			}
			addcolor(CicadaColorSettings[gender], colorType, colorData);
		}
		private static void addcolor(Dictionary<CicadaColorType, List<ColorData>> dictionary, CicadaColorType colorType, ColorData colorData)
        {
			if(!dictionary.ContainsKey(colorType))
            {
				dictionary[colorType] = new List<ColorData>();
            }
			List<ColorData> List = dictionary[colorType];
			List.Add(colorData);
        }
		private static void AddWeight(Dictionary<CicadaColorType, List<double>> dictionary, CicadaColorType colorType, double weight)
		{
			if (!dictionary.ContainsKey(colorType))
			{
				dictionary[colorType] = new List<double>();
			}
			List<double> List = dictionary[colorType];
			List.Add(weight);
		}
        #endregion     //this is a constructor for color and weight dictionaries		

        public enum CicadaColorType
        {
			main,
			secondary,
			eyes,
			pupils
        }
		public enum SetForGender
        {
			female=1,
			male=2,
			all=3
        }//This is a gender enum, with values picked in a way to use bitmask in addcolor primary method

		public BepInEx.Logging.ManualLogSource pubLog;
		public void OnEnable()
        {
			pubLog = Logger;
			On.CicadaGraphics.ApplyPalette += PaletteOverride;
			On.CicadaGraphics.DrawSprites += CustomAnimationColor.CustomDrawSprites;
        }		//this is basic function where we ask BepInEx to overwrite mentioned function with PaletteOverride method
		private static Color GetRandomColorWithFallback(CicadaColorType colorType, System.Random random, CicadaGraphics self, RoomPalette palette)
        {
			if (CicadaColorSettings.TryGetValue(self.cicada.gender, out var subDictionary)) 
			{
				if(subDictionary.TryGetValue(colorType, out var colorsList))
                {
					return RandomWeight.GetColor(colorType, random, self, palette);
					UnityEngine.Debug.Log("GetColor happened");
				}
			}
			return Fallback(colorType, self, palette);
		}           //this is a function to choose a random color given specific bool and color type
		private static Color GetDefaultColor(CicadaGraphics self, RoomPalette palette)
        {
			if (self.cicada.gender)
			{
				return Color.Lerp(HSLColor.Lerp(self.iVars.color, new HSLColor(self.cicada.iVars.color.hue, 0f, 1f), 0.8f).rgb, palette.fogColor, 0.1f);
			}
			else
			{
				return Color.Lerp(HSLColor.Lerp(self.cicada.iVars.color, new HSLColor(self.cicada.iVars.color.hue, 1f, 0f), 0.8f).rgb, palette.blackColor, 0.85f);
			}
		}
		public static void PaletteOverride(On.CicadaGraphics.orig_ApplyPalette orig, CicadaGraphics self,
            RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette) // this is main code. Basically, using unique seed random is instantiated and a color is chosen of that palette based on random for every part
        {
			orig(self, sLeaser, rCam, palette);
			Debug.Log("CadaColors PaletteOverride method starts working");
			System.Random random = new System.Random(self.cicada.abstractCreature.ID.RandomSeed);

			Color color = GetRandomColorWithFallback(CicadaColorType.main, random, self, palette);
			self.shieldColor = GetRandomColorWithFallback(CicadaColorType.secondary, random, self, palette);
			
			if (self.cicada.gender)
			{
				sLeaser.sprites[self.HighlightSprite].color = Color.Lerp(color, new Color(1f, 1f, 1f), 0.7f);
			}
			else
			{
				sLeaser.sprites[self.HighlightSprite].color = Color.Lerp(color, self.cicada.iVars.color.rgb, 0.07f);
			}
			sLeaser.sprites[self.BodySprite].color = color;
			sLeaser.sprites[self.HeadSprite].color = color;
			sLeaser.sprites[self.ShieldSprite].color = self.shieldColor;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{

					Debug.Log($"CadaColors wing/sprite application {i}, {j}");
					sLeaser.sprites[self.WingSprite(i, j)].color = Color.Lerp(new Color(0f, 0f, 0f), self.shieldColor, 0.2f);
					sLeaser.sprites[self.TentacleSprite(i, j)].color = color;
				}
			}
			self.eyeColor = GetRandomColorWithFallback(CicadaColorType.eyes, random, self, palette);
			Color pupilColor = GetRandomColorWithFallback(CicadaColorType.pupils, random, self, palette);

			sLeaser.sprites[self.EyesASprite].color = self.eyeColor;
			sLeaser.sprites[self.EyesBSprite].color = pupilColor;
			//base.ApplyPalette(sLeaser, rCam, palette);   is empty anyway
		}
    }
}
