using System.Collections.Generic;
using SkiaSharp;

namespace EmitIcon.Service
{
    public class EmitIconService : IEmitIconService
    {
        private IWindowsIcoService WindowsIcoService { get; }
            = new WindowsIcoService();
        private IMobileiOSIconService MobileiOSIconService { get; }
            = new MobileiOSIconService();

        public byte[] GetWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, List<SKSizeI> resolutions = null)
        {
            var result = WindowsIcoService.FromData(image, imageScale,
                imageMaskColor, backgroundColor, cornerRadius, resolutions);
            return result;
        }

        public byte[] PreviewWindowsIco(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, SKSizeI? size = null)
        {
            var result = WindowsIcoService.Preview(image, imageScale,
                imageMaskColor, backgroundColor, cornerRadius, size);
            return result;
        }

        public Dictionary<string, byte[]> GetiOSIcon(byte[] image,
            float? imageScale = null, SKColor? imageMaskColor = null,
            SKColor? backgroundColor = null, float? cornerRadius = null)
        {
            backgroundColor = backgroundColor ?? SKColors.White;
            var result = MobileiOSIconService.FromData(image, imageScale,
                imageMaskColor, backgroundColor, cornerRadius);
            return result;
        }

        public byte[] PreviewiOSIcon(byte[] image, float? imageScale = null,
            SKColor? imageMaskColor = null, SKColor? backgroundColor = null,
            float? cornerRadius = null, SKSizeI? size = null)
        {
            var result = MobileiOSIconService.Preview(image, imageScale,
                imageMaskColor, backgroundColor, cornerRadius, size);
            return result;
        }
    }
}
