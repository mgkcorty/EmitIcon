using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    internal interface IWindowsIcoService
    {
        byte[] FromData(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, List<SKSizeI> resolutions = null);

        byte[] Preview(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, SKSizeI? size = null);
    }
}
