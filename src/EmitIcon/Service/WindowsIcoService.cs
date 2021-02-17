using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;

namespace EmitIcon.Service
{
    internal class WindowsIcoService : IWindowsIcoService
    {
        private static readonly List<SKSizeI> DefaultResolutions = new List<SKSizeI>
        {
            new SKSizeI(16, 16),
            new SKSizeI(32, 32),
            new SKSizeI(48, 48),
            new SKSizeI(256, 256)
        };
        
        public byte[] FromData(byte[] imageData, float? imageScale = null, SKColor? imageMaskColor = null,
            SKColor? backgroundColor = null, int? cornerRadius = null, List<SKSizeI> resolutions = null)
        {
            var isSvg = Encoding.UTF8.GetString(imageData)[0] == '<';
            return CreateWindowsRoundedIcon(imageData, isSvg, imageScale, imageMaskColor,
                backgroundColor, cornerRadius, resolutions);
        }

        public byte[] Preview(byte[] imageData, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            int? cornerRadius = null, SKSizeI? size = null)
        {
            var backgroundSize = size ?? new SKSizeI(256, 256);
            var scale = imageScale ?? 0.9f;
            if (scale > 1)
                scale = 1;
            if (scale < 0)
                scale = 0;
            var pictureSize = new SKSizeI((int)(scale * backgroundSize.Width), (int)(scale * backgroundSize.Height));
            var isSvg = Encoding.UTF8.GetString(imageData)[0] == '<';
            var scaledCornerRadius = cornerRadius == null ? null : (int?)(backgroundSize.Width * ((float)cornerRadius.Value / 256.0f));
            scaledCornerRadius = scaledCornerRadius == null ? (int?)null : (scaledCornerRadius > 1 ? (int)scaledCornerRadius : 1);
            var bitmap = GetBitmapFromImageData(imageData, isSvg, pictureSize, backgroundSize, imageMaskColor, backgroundColor, (int)scaledCornerRadius);
            var skData = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);
            var data = skData.ToArray();
            return data;
        }

        private SKBitmap GetSKBitmap(byte[] imageData, bool isSvg, SKSizeI size)
        {
            if (isSvg)
            {
                var bitmap = SKBitmapHelper.SvgToSKBitmap(imageData, size);
                return bitmap;
            }
            else
            {
                var bitmap = SKBitmap.Decode(imageData);
                bitmap = bitmap.Resize(new SKSizeI(size.Width, size.Height), SKFilterQuality.High);
                return bitmap;
            }
        }

        private SKBitmap GetBitmapFromImageData(byte[] imageData, bool isSvg,
            SKSizeI pictureSize, SKSizeI backgroundSize, SKColor? imageMaskColor = null,
            SKColor? backgroundColor = null, int? cornerRadius = null)
        {
            var bitmap = GetSKBitmap(imageData, isSvg, pictureSize);
            if (imageMaskColor != null)
            {
                bitmap = bitmap.ApplyMask(imageMaskColor.Value);
            }
            var result = new SKBitmap(backgroundSize.Width, backgroundSize.Height);
            var skCanvas = new SKCanvas(result);
            var rect = new SKRoundRect(new SKRect(0, 0, backgroundSize.Width, backgroundSize.Height), cornerRadius ?? 0);
            skCanvas.ClipRoundRect(rect, SKClipOperation.Intersect, true);
            skCanvas.Clear(backgroundColor ?? SKColors.Transparent);
            skCanvas.DrawBitmap(bitmap,
                (backgroundSize.Width - pictureSize.Width) / 2.0f,
                (backgroundSize.Height - pictureSize.Height) / 2.0f);
            skCanvas.Flush();
            skCanvas.Save();
            var skData = SKImage.FromBitmap(result).Encode(SKEncodedImageFormat.Png, 100);
            var data = skData.ToArray();
            return SKBitmap.Decode(data);
        }

        private byte[] CreateWindowsRoundedIcon(byte[] imageData, bool isSvg, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null, int? cornerRadius = null, List<SKSizeI> resolutions = null)
        {
            var arr = (resolutions ?? DefaultResolutions).ToList();
            arr.Reverse();
            var cornerRadiusValues = arr.Select(x =>
            {
                if (cornerRadius == null)
                    return 0;
                var scaledCornerRadius = x.Width * ((float)cornerRadius.Value / 256.0f);
                return scaledCornerRadius > 1 ? (int)scaledCornerRadius : 1;
            }).ToList();
            var bitmaps = new List<SKBitmap>();
            for (int i = 0; i < arr.Count; i++)
            {
                var backgroundSize = arr[i];
                var scale = imageScale ?? 0.9f;
                if (scale > 1)
                    scale = 1;
                if (scale < 0)
                    scale = 0;
                var pictureSize = new SKSizeI((int) (scale * backgroundSize.Width), (int) (scale * backgroundSize.Height));
                var scaledCornerRadius = cornerRadiusValues[i];
                var bitmap = GetBitmapFromImageData(imageData, isSvg, pictureSize, backgroundSize, imageMaskColor, backgroundColor, scaledCornerRadius);
                bitmaps.Add(bitmap);
            }
            var result = IcoPng.Create(bitmaps.ToList());
            bitmaps.ForEach(x => x.Dispose());
            bitmaps.Clear();
            return result;
        }
    }
}
