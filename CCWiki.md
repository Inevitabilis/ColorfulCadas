# Colorful Cadas
## How to use
`CadaColorsVanilla` and `CadaColorsGuildedWings` source codes pretty much explain everything you need to know about it, the latter being more useful since it is the basic unit that is required for framework to recognise your inputs.  Here is its contents:

```cs
Framework.addcolor(Framework.CicadaColorType.secondary, Framework.SetForGender.all, new SimpleColorData(ColorOverride.ColorByRGB(255, 219, 78), 0.15f));
```
`Framework.addcolor` is the only method you will be interacting with. Its arguments are as following: 
```cs
public static void Framework.addcolor(CicadaColorType colorType, SetForGender gender, ColorData colorData)
```
`Framework.CicadaColorType` is responsible for knowing which part of body will be affected by the color you specify:
```cs
public enum Framework.CicadaColorType
{
    main,
	secondary,
	eyes,
	pupils
}
```
`Framework.SetForGender` is responsible for knowing which cicada type will be affected by the color you specify:
```cs
public enum Framework.SetForGender
{
	female=1,
	male=2,
	all=3
}
```
`ColorData` is a class that is designed to hold information about colors to apply, specifically their color code and rarity. Also used to define vanilla colors.
```cs
public abstract class ColorData
{
	public float weight;
	public abstract Color GetColor(System.Random random, RoomPalette palette);
}
```
Note: this class is abstract. You have to use any of its children such as `VanillaColorData` or `SimpleColorData`.

```cs
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
```
This one exists to allow treating vanilla colors as any other colors added with this framework. Most importantly — be a part of the same random pool.
```cs
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
```
`SimpleColorData` is the simple way to make CadaColors use your custom colors. Important note: it uses UnityEngine.Color to store data.

All ColorData types share the `float weight` field responsible for telling framework of how likely the option is to be chosen from the list. By default all colors you write in have a weight of 1. A weight of a color represents how likely it is to be chosen from a whole pool of options. Say, if two of `(bool, Framework.CicadaColorType)` options have respective weights of 1 and 0.15, then a chance for the first one to be chosen is 1/1.15 and for the last one is 0.15/1.15, so be wary, 0.15 doesn't mean 15% chance to meet one.

Code file `ColorOverride` and its only function `ColorByRGB` is made for the sole purpose of converting ints to color, as default UnityEngine method only works with floats on a scale 0-1, which was inconvenient to say the least.

You can summon `SimpleColorData` without specifying the second parameter, `weight`, which would default it to 1. Same applies to `VanillaColorData`.

## Disclaimer
This mod uses `System.Random` instead of `UnityEngine.Random`, so for vanilla chosen parts their results won't be the same as for their pure vanilla analogues. I can state however that all used randoms in my code are based on cada ID and theoretically same seed cadas within same datapacks would be the same.

It is fine if you don't fill some options up, in that case `Fallback` function will be activated imitating vanilla colors. In this case, however, no random is used and within the same `(bool, CicadaColorType)` coordinates all cadas would share the same vanilla color.

Changing wings colors on the fly is a `vanilla` mechanic I haven't touched. 

`Random picking mechanism` was never intended to be used with negative or zero weights. If you happen to use these, `Joar` might get summoned.

