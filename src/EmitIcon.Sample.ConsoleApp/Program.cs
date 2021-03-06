﻿using System.IO;
using EmitIcon.Service;
using SkiaSharp;

namespace EmitIcon.Sample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IEmitIconService emitIconService = new EmitIconService();
            var imagePath = Path.Combine("Icons", "ic_ac_unit_48px.svg");
            var image = File.ReadAllBytes(imagePath);

            var ico = emitIconService.GetWindowsIco(image,
                imageMaskColor: SKColors.White,
                backgroundColor: SKColors.BlueViolet,
                imageScale: 0.9f,
                cornerRadius: 1f);
            var preview = emitIconService.PreviewWindowsIco(image,
                imageMaskColor: SKColors.White,
                backgroundColor: SKColors.BlueViolet,
                imageScale: 0.9f,
                cornerRadius: 1f,
                size: new SKSizeI(1000, 1000));
            File.WriteAllBytes("icon.ico", ico);
            File.WriteAllBytes("preview.png", preview);

            var iosIcon = emitIconService.GetiOSIcon(image,
                imageMaskColor: SKColors.White,
                backgroundColor: SKColors.BlueViolet);
            foreach(var pair in iosIcon)
            {
                var name = pair.Key;
                var value = pair.Value;
                File.WriteAllBytes(name, value);
            }
        }
    }
}
