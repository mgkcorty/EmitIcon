using System;
using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    internal class MobileiOSIconService : IMobileiOSIconService
    {
        private IDrawService DrawService { get; } = new DrawService();

        // based on https://developer.apple.com/library/archive/qa/qa1686/_index.html
        private readonly Dictionary<string, SKSizeI> _iconSizes =
            new Dictionary<string, SKSizeI>
            {
                ["Icon-60@2x.png"] = new SKSizeI(120, 120),
                ["Icon-60@3x.png"] = new SKSizeI(180, 180),
                ["Icon-76.png"] = new SKSizeI(76, 76),
                ["Icon-76@2x.png"] = new SKSizeI(152, 152),
                ["Icon-83.5@2x.png"] = new SKSizeI(167, 167),
                // iPhone
                ["Icon-Small-40@2x.png"] = new SKSizeI(80, 80),
                ["Icon-Small-40@3x.png"] = new SKSizeI(120, 120),
                // iPad
                ["Icon-Small-iPad-40.png"] = new SKSizeI(40, 40),
                ["Icon-Small-iPad-40@2x.png"] = new SKSizeI(80, 80),
                // iPhone Notifications
                ["Icon-Notifications-iPhone-20@2x.png"] = new SKSizeI(40, 40),
                ["Icon-Notifications-iPhone-20@3x.png"] = new SKSizeI(60, 60),
                // iPad Notifications
                ["Icon-Notifications-iPad-20.png"] = new SKSizeI(20, 20),
                ["Icon-Notifications-iPad-20@2x.png"] = new SKSizeI(40, 40),
                ["Icon-Small.png"] = new SKSizeI(29, 29),
                ["Icon-Small@2x.png"] = new SKSizeI(58, 58),
                ["Icon-Small@3x.png"] = new SKSizeI(87, 87),
                ["Icon-Small-iPad.png"] = new SKSizeI(29, 29),
                ["Icon-Small-iPad@2x.png"] = new SKSizeI(58, 58),

                // iTunesArtwork icon images should be in png format, but name them without the .png extension.
                ["iTunesArtwork"] = new SKSizeI(512, 512),
                ["iTunesArtwork@2x"] = new SKSizeI(1024, 1024),

                ["Icon-iOSMarketing.png"] = new SKSizeI(1024, 1024),
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
