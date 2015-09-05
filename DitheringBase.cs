using UnityEngine;
using System.Collections;

// This Delegate is used to find best suitable color from limited set of colors
public delegate Color32 FindColor(Color32 original);

public abstract class DitheringBase {

	protected int width;
	protected int height;

    protected FindColor colorFunction = null;

    protected string methodLongName = "";
    protected string fileNameAddition = "";

    private Color32[] pixels = null;

    public DitheringBase(FindColor colorfunc)
    {
        this.colorFunction = colorfunc;
    }

    // 
    public Texture2D DoDithering(Texture2D input)
    {
        this.width = input.width;
        this.height = input.height;

        // Copy all pixels of input Texture2D to new array, which we are going to edit
        this.pixels = input.GetPixels32();     

        Color32 originalPixel = Color.white; // Default value isn't used
        Color32 newPixel = Color.white; // Default value isn't used
        short[] quantError = null; // Default values aren't used

        for (int y = 0; y < this.height; y++)
        {
            for (int x = 0; x < this.width; x++)
            {
                originalPixel = this.pixels[GetIndexWith(x, y)];
                newPixel = this.colorFunction(originalPixel);

                this.pixels[GetIndexWith(x, y)] = newPixel;

                quantError = GetQuantError(originalPixel, newPixel);
                this.PushError(x, y, quantError);
            }
        }

        // Create the texture we are going to return from pixels array 
        Texture2D returnTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        returnTexture.SetPixels32(this.pixels);
        returnTexture.Apply();

        return returnTexture;
    }

    // Implement this for every dithering method
    protected abstract void PushError(int x, int y, short[] quantError);

    protected bool IsValidCoordinate(int x, int y)
	{
		return (0 <= x && x < this.width && 0 <= y && y < this.height);
	}

	protected int GetIndexWith(int x, int y)
	{
		return y * this.width + x;
	}

    public string GetMethodName()
    {
        return this.methodLongName;
    }

    public string GetFilenameAddition()
    {
        return this.fileNameAddition;
    }

    protected short[] GetQuantError(Color32 originalPixel, Color32 newPixel)
	{
		short[] returnValue = new short[4];

		returnValue [0] = (short)(originalPixel.r - newPixel.r);
		returnValue [1] = (short)(originalPixel.g - newPixel.g);
		returnValue [2] = (short)(originalPixel.b - newPixel.b);
		returnValue [3] = (short)(originalPixel.a - newPixel.a);

		return returnValue;
	}

    public void ModifyImageWithErrorAndMultiplier(int x, int y, short[] quantError, float multiplier)
    {
        Color32 oldColor = this.pixels[GetIndexWith(x, y)];

        // We limit the color here because we don't want the value go over 255 or under 0
        Color32 newColor = new Color32(
                            GetLimitedValue(oldColor.r, Mathf.RoundToInt(quantError[0] * multiplier)),
                            GetLimitedValue(oldColor.g, Mathf.RoundToInt(quantError[1] * multiplier)),
                            GetLimitedValue(oldColor.b, Mathf.RoundToInt(quantError[2] * multiplier)),
                            GetLimitedValue(oldColor.a, Mathf.RoundToInt(quantError[3] * multiplier)));

        this.pixels[GetIndexWith(x, y)] = newColor;
    }

    private static byte GetLimitedValue(byte original, int error)
    {
        int newValue = original + error;   
        return (byte)Mathf.Clamp(newValue, byte.MinValue, byte.MaxValue);
    }
}
