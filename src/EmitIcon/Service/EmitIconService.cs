using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    public class EmitIconService : IEmitIconService
    {
        private IWindowsIcoService WindowsIcoService { get; }
            = new WindowsIcoService();

        public byte[] GetWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            int? cornerRadius = null, List<SKSizeI> resolutions = null)
        {
            var result = WindowsIcoService.FromData(image, imageScale,
                imageMaskColor, backgroundColor, cornerRadius, resolutions);
            return result;
        }

        public byte[] PreviewWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            int? cornerRadius = null, SKSizeI? size = null)
        {
            var result = WindowsIcoService.Preview(image, imageScale,
                imageMaskColor, backgroundColor, cornerRadius, size);
            return result;
        }
    }
}
