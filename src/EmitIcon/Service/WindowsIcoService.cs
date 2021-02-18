using System;
using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    internal class WindowsIcoService : IWindowsIcoService
    {
        private IDrawService DrawService { get; } = new DrawService();

        private static readonly List<SKSizeI> DefaultResolutions = new List<SKSizeI>
        {
            new SKSizeI(16, 16),
            new SKSizeI(32, 32),
            new SKSizeI(48, 48),
            new SKSizeI(256, 256)
        };
        
        public byte[] FromData(byte[] imageData, float? imageScale = null, SKColor? imageMaskColor = null,
            SKColor? backgroundColor = null, float? cornerRadius = null, List<SKSizeI> resolutions = null)
        {
            var sizes = resolutions ?? DefaultResolutions;
            var bitmaps = new List<SKBitmap>();
            foreach(var size in sizes)
            {
                var bitmap = DrawService.CreateBitmapIcon(
                    imageData,
                    imageScale,
                    imageMaskColor,
                    backgroundColor,
                    cornerRadius,
                    size);
                bitmaps.Add(bitmap);
            }
            var result = IcoPng.Create(bitmaps);
            bitmaps.ForEach(x => x.Dispose());
            return result;
        }

        public byte[] Preview(byte[] imageData, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, SKSizeI? size = null)
        {
            var bitmap = DrawService.CreateBitmapIcon(
                imageData,                
                imageScale,
                imageMaskColor,
                backgroundColor,
                cornerRadius,
                size ?? new SKSizeI(256, 256));
            var data = bitmap.ToByteArray();
            bitmap.Dispose();
            return data;
        }
    }
}
