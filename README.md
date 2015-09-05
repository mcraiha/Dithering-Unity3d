# Dithering-Unity3d
Dithering algorithms for Unity3d

This project contains source code and sample images of dithering algorithms that can be used in Unity3d. There are total of 8 different dithering algorithms. You can find more info about the algorithms from [blog post](http://www.tannerhelland.com/4660/dithering-eleven-algorithms-source-code/) made by **Tanner Helland**.

##Definition
*"Dither is an intentionally applied form of noise used to randomize quantization error, preventing large-scale patterns such as color banding in images."* [Wikipedia](https://en.wikipedia.org/wiki/Dither)

##What can I do with it
With Unity you can use dithering e.g. to convert 32 bit textures to 16/8 bit textures in such way that new image doesn't look so bad to the human eye than it would if just color reduction was done to it.

You can do this on Editor or on Device.

##How do I use it
It is very simple. Just follow following example
```cs
private static Color32 TrueColorToWebSafeColor(Color32 inputColor)
{
  Color32 returnColor = new Color32( (byte)(Mathf.RoundToInt(inputColor.r / 51.0f) * 51),
                                      (byte)(Mathf.RoundToInt(inputColor.g / 51.0f) * 51),
                                      (byte)(Mathf.RoundToInt(inputColor.b / 51.0f) * 51),
                                      byte.MaxValue);
  return returnColor;
}

DitheringBase method = new FloydSteinbergDithering(TrueColorToWebSafeColor);
Texture2D dithered = method.DoDithering(input);
```
First you have to have a color reduction function (in this case **TrueColorToWebSafeColor**). Then you create new instance of chosen algorithm (in this case **FloydSteinbergDithering**) and finally call DoDithering of dithering instance with chosen input Texture2D. It then returns the dithered Texture2D.

## License
Text in this document and source code files are released into the public domain. See [PUBLICDOMAIN](https://github.com/mcraiha/Dithering-Unity3d/blob/master/PUBLICDOMAIN) file.

Parrot image (half.png) is made from image that comes from [Kodak Lossless True Color Image Suite](http://r0k.us/graphics/kodak/) and it doesn't have any specific license.

##Sample image
I took the famous [parrot image](http://r0k.us/graphics/kodak/kodim23.html) and reduced its size to 384x256. Then I ran the image (which has 64655 different colors) with all dithering methods and using Web safe colors as palette. I also calculated PSNR and SSIM values for dithered images.

Resized image has filesize of 244 kB as PNG. All dithered files have filesize between 63-70 kB.

![Parrots](https://github.com/mcraiha/Dithering-Unity3d/blob/master/birds.png)

##.NET version
If you need same functionality as .NET version, then head to https://github.com/mcraiha/CSharp-Dithering
