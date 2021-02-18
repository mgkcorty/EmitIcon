using SkiaSharp;

namespace EmitIcon.Service
{
    internal interface IDrawService
    {
        SKBitmap CreateBitmapIcon(byte[] imageData,
               float? imageScale = null, SKColor? imageMaskColor = null,
               SKColor? backgroundColor = null, float? cornerRadius = null,
               SKSizeI? resolution = null);
    }
}
