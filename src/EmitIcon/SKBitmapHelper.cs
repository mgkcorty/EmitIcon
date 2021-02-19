using System.IO;
using SkiaSharp;

namespace EmitIcon
{
    internal static class SKBitmapHelper
    {
        public static SKBitmap FromColor(SKImageInfo imageInfo, SKColor color)
        {
            var bitmap = new SKBitmap(imageInfo, SKBitmapAllocFlags.ZeroPixels);
            bitmap.Erase(color);
            return bitmap;
        }

        public static SKBitmap SvgToSKBitmap(byte[] svgData, SKSizeI? size = null)
        {
            if(size != null && (size.Value.Width == 0 || size.Value.Height == 0))
                return new SKBitmap(0, 0);
            using (var svgStream = new MemoryStream(svgData))
            {
                var skSvg = size == null ?
                    new SkiaSharp.Extended.Svg.SKSvg() :
                    new SkiaSharp.Extended.Svg.SKSvg(new SKSize(size.Value.Width, size.Value.Height));
                skSvg.Load(svgStream);
                var skBitmap = new SKBitmap((int)skSvg.CanvasSize.Width, (int)skSvg.CanvasSize.Height);
                var skCanvas = new SKCanvas(skBitmap);
                skCanvas.Clear(SKColors.Transparent);
                skCanvas.DrawPicture(skSvg.Picture);
                skCanvas.Flush();
                skCanvas.Save();
                return skBitmap;
            }
        }
    }
}
