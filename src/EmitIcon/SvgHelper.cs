using SkiaSharp;

namespace EmitIcon
{
    public static class SvgHelper
    {
        public static byte[] SvgToPng(byte[] svg, SKSizeI? size = null)
        {
            var bitmap = SKBitmapHelper.SvgToSKBitmap(svg, size);
            var skData = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);
            var data = skData.ToArray();
            return data;
        }
    }
}
