using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    public interface IEmitIconService
    {
        byte[] GetWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, List<SKSizeI> resolutions = null);

        byte[] PreviewWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, SKSizeI? size = null);

        Dictionary<string, byte[]> GetiOSIcon(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null);

        byte[] PreviewiOSIcon(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, SKSizeI? size = null);
    }
}
