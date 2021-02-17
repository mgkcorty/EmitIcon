using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    public interface IEmitIconService
    {
        byte[] GetWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            int? cornerRadius = null, List<SKSizeI> resolutions = null);

        byte[] PreviewWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            int? cornerRadius = null, SKSizeI? size = null);
    }
}
