using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    internal interface IMobileiOSIconService
    {
        Dictionary<string, byte[]> FromData(byte[] imageData,
           float? imageScale = null, SKColor? imageMaskColor = null,
           SKColor? backgroundColor = null, float? cornerRadius = null);

        byte[] Preview(byte[] imageData, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, SKSizeI? size = null);
    }
}
