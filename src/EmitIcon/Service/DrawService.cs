using System;
using System.Text;
using SkiaSharp;

namespace EmitIcon.Service
{
    internal class DrawService : IDrawService
    {
        private SKBitmap GetSKBitmap(byte[] imageData, bool isSvg,
            SKSizeI? size = null)
        {
            if (isSvg)
            {
                var bitmap = SKBitmapHelper.SvgToSKBitmap(imageData, size);
                return bitmap;
            }
            else
            {
                var bitmap = SKBitmap.Decode(imageData);
                if (size != null)
                {
                    bitmap = bitmap.Resize(
                        new SKSizeI(size.Value.Width, size.Value.Height),
                        SKFilterQuality.High);
                }
                return bitmap;
            }
        }

        private SKBitmap DrawBitmapIcon(byte[] imageData, bool isSvg,
            SKSizeI pictureSize, SKSizeI backgroundSize,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null)
        {
            var bitmap = GetSKBitmap(imageData, isSvg, pictureSize);
            if (imageMaskColor != null)
            {
                bitmap = bitmap.ApplyMask(imageMaskColor.Value);
            }
            var result = new SKBitmap(
                backgroundSize.Width,
                backgroundSize.Height);
            var skCanvas = new SKCanvas(result);
            var xRadius = cornerRadius == null ?
                0 :
                backgroundSize.Width / 2.0f * cornerRadius.Value;
            var yRadius = cornerRadius == null ?
                0 :
                backgroundSize.Height / 2.0f * cornerRadius.Value;
            var rect = new SKRoundRect(
                new SKRect(0, 0, backgroundSize.Width, backgroundSize.Height),
                xRadius, yRadius);
            skCanvas.ClipRoundRect(rect, SKClipOperation.Intersect, true);
            skCanvas.Clear(backgroundColor ?? SKColors.Transparent);
            skCanvas.DrawBitmap(bitmap,
                (backgroundSize.Width - pictureSize.Width) / 2.0f,
                (backgroundSize.Height - pictureSize.Height) / 2.0f);
            skCanvas.Flush();
            skCanvas.Save();
            var skData = SKImage.FromBitmap(result).Encode(
                SKEncodedImageFormat.Png, 100);
            var data = skData.ToArray();
            return SKBitmap.Decode(data);
        }

        public SKBitmap CreateBitmapIcon(byte[] imageData,
            float? imageScale = null, SKColor? imageMaskColor = null,
            SKColor? backgroundColor = null, float? cornerRadius = null,
            SKSizeI? resolution = null)
        {
            var isSvg = Encoding.UTF8.GetString(imageData)[0] == '<';

            SKSizeI backgroundSize;
            if(resolution == null)
            {
                var bitmap = GetSKBitmap(imageData, isSvg);
                backgroundSize = new SKSizeI(bitmap.Width, bitmap.Height);
            }
            else
            {
                backgroundSize = resolution.Value;
            }

            if (imageScale != null && imageScale > 1)
                imageScale = 1;
            if (imageScale != null && imageScale < 0)
                imageScale = 0;
            if (cornerRadius != null && cornerRadius > 1)
                cornerRadius = 1;
            if (cornerRadius != null && cornerRadius < 0)
                cornerRadius = 0;
            var pictureSize = new SKSizeI(
                (int)((imageScale ?? 1) * backgroundSize.Width),
                (int)((imageScale ?? 1) * backgroundSize.Height));
            var result = DrawBitmapIcon(imageData, isSvg,
                pictureSize, backgroundSize, imageMaskColor,
                backgroundColor, cornerRadius);
            return result;
        }
    }
}
