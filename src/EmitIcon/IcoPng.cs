using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace EmitIcon
{
    internal class IcoPng
    {
        public const int MaxIconWidth = 256;
        public const int MaxIconHeight = 256;

        private const ushort HeaderReserved = 0;
        private const ushort HeaderIconType = 1;
        private const byte HeaderLength = 6;

        private const byte EntryReserved = 0;
        private const byte EntryLength = 16;

        private const byte PngColorsInPalette = 0;
        private const ushort PngColorPlanes = 1;

        public static byte[] Create(List<SKBitmap> images)
        {
            if (images == null)
                throw new ArgumentNullException(nameof(images));
            ThrowForInvalidPngs(images);

            var orderedImages = images
                .OrderByDescending(i => i.Width)
                .ToArray();

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(HeaderReserved);
                writer.Write(HeaderIconType);
                writer.Write((ushort)orderedImages.Length);

                var buffers = new Dictionary<uint, byte[]>();

                uint lengthSum = 0;
                uint baseOffset = (uint)(HeaderLength + EntryLength * orderedImages.Length);
                foreach (var image in orderedImages)
                {
                    var buffer = image.ToByteArray();

                    uint offset = baseOffset + lengthSum;

                    writer.Write(GetIconWidth(image));
                    writer.Write(GetIconHeight(image));
                    writer.Write(PngColorsInPalette);
                    writer.Write(EntryReserved);
                    writer.Write(PngColorPlanes);
                    writer.Write((ushort)image.Info.BitsPerPixel);
                    writer.Write((uint)buffer.Length);
                    writer.Write(offset);

                    lengthSum += (uint)buffer.Length;
                    buffers.Add(offset, buffer);
                }

                foreach (var kvp in buffers)
                {
                    writer.BaseStream.Seek(kvp.Key, SeekOrigin.Begin);
                    writer.Write(kvp.Value);
                }
                return stream.ToArray();
            }
        }

        private static void ThrowForInvalidPngs(IEnumerable<SKBitmap> images)
        {
            foreach(var image in images)
            {
                var Format32bppArgb = (image.ColorType == SKColorType.Rgba8888 || image.ColorType == SKColorType.Bgra8888) &&
                    image.AlphaType == SKAlphaType.Premul;
                if (!Format32bppArgb)
                {
                    throw new InvalidOperationException(
                        $"You should use {nameof(Format32bppArgb)} pixel format.");
                }

                if (image.Width > MaxIconWidth ||
                    image.Height > MaxIconHeight)
                {
                    throw new InvalidOperationException(
                        $"Max dimension is {MaxIconWidth}.");
                }
            }
        }

        private static byte GetIconWidth(SKBitmap image)
        {
            if (image.Width == MaxIconWidth)
                return 0;

            return (byte)image.Width;
        }

        private static byte GetIconHeight(SKBitmap image)
        {
            if (image.Height == MaxIconHeight)
                return 0;

            return (byte)image.Height;
        }
    }
}
