using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ColorfulCadas
{
	public abstract class ColorData
	{
		public float weight;
		public ColorData(float weight)
		{
			this.weight = weight;
		}
		public abstract Color GetColor(System.Random random, RoomPalette palette);
	}
        
        
	public class SimpleColorData : ColorData
	{


		public Color color;
		public SimpleColorData(Color a, float weight = 1) : base(weight)
		{
			color = a;
		}
        public override Color GetColor(System.Random random, RoomPalette palette)
        {
			return color;
        }
    }
	public class VanillaColorData : ColorData
    {
		Framework.CicadaColorType colorType;
		bool gender;
		public VanillaColorData(Framework.CicadaColorType colorType, bool gender, float weight = 1) : base(weight)
		{
			this.colorType = colorType;
			this.gender = gender;
		}
        public override Color GetColor(System.Random random, RoomPalette palette)
        {
			return VanillaExecutor.GetVanillaColor(gender, colorType, palette, random);
        }

    }
}
