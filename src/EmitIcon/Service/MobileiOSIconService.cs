using System;
using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    internal class MobileiOSIconService : IMobileiOSIconService
    {
        private IDrawService DrawService { get; } = new DrawService();

        private readonly Dictionary<string, SKSizeI> _iconSizes =
            new Dictionary<string, SKSizeI>
            {
                ["Icon-60@2x"] = new SKSizeI(120, 120),
                ["Icon-60@3x"] = new SKSizeI(180, 180),
                ["Icon-76"] = new SKSizeI(76, 76),
                ["Icon-76@2x"] = new SKSizeI(152, 152),
                ["Icon-83.5@2x"] = new SKSizeI(167, 167),
                ["Icon-Small-40"] = new SKSizeI(40, 40),
                ["Icon-Small-40@2x"] = new SKSizeI(80, 80),
                ["Icon-Small-40@3x"] = new SKSizeI(120, 120),
                ["Icon-Small"] = new SKSizeI(29, 29),
                ["Icon-Small@2x"] = new SKSizeI(58, 58),
                ["Icon-Small@3x"] = new SKSizeI(87, 87),
                ["iTunesArtwork"] = new SKSizeI(512, 512),
                ["iTunesArtwork@2x"] = new SKSizeI(1024, 1024)
            };

        public Dictionary<string, byte[]> FromData(byte[] imageData,
            float? imageScale = null, SKColor? imageMaskColor = null,
            SKColor? backgroundColor = null, float? cornerRadius = null)
        {
            var result = new Dictionary<string, byte[]>();
            foreach (var size in _iconSizes)
            {
                var bitmap = DrawService.CreateBitmapIcon(
                    imageData,
                    imageScale,
                    imageMaskColor,
                    backgroundColor,
                    cornerRadius,
                    size.Value);
                result[size.Key] = bitmap.ToByteArray();
                bitmap.Dispose();
            }
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
