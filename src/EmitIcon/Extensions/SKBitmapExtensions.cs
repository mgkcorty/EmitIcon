using SkiaSharp;

namespace System
{
    internal static class SKBitmapExtensions
    {
        public static SKBitmap ClipRoundRect(this SKBitmap bitmap, int cornerRadius)
        {
            var rect = new SKRoundRect(new SKRect(0, 0, bitmap.Width, bitmap.Height), cornerRadius);
            var skCanvas = new SKCanvas(bitmap);
            skCanvas.ClipRoundRect(rect);
            skCanvas.Flush();
            skCanvas.Save();
            return bitmap;
        }

        public static SKBitmap ApplyMask(this SKBitmap bitmap, SKColor color)
        {
            var skCanvas = new SKCanvas(bitmap);
            skCanvas.DrawColor(color, SKBlendMode.SrcIn);
            skCanvas.Flush();
            skCanvas.Save();
            return bitmap;
        }

        public static byte[] ToByteArray(this SKBitmap bitmap)
        {
            var skData = SKImage.FromBitmap(bitmap)
                .Encode(SKEncodedImageFormat.Png, 100);
            var data = skData.ToArray();
            return data;
        }
    }
}
