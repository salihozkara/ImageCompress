using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace ImageCompress
{
    public static class ImageAsyncHelper
    {
        private static async Task<byte[]> HelpFunction(Image image, ResizeOptions resizeOptions, int quality = 100)
        {


            image.Mutate(x => x
                .Resize(resizeOptions));

            var encoder = new JpegEncoder()
            {
                Quality = quality //Use variable to set between 5-30 based on your requirements
            };

            //This saves to the memoryStream with encoder
            await using var memoryStream = new MemoryStream();
            await image.SaveAsync(memoryStream, encoder).ConfigureAwait(false);
            memoryStream.Position = 0; // The position needs to be reset.
            image.Dispose();
            // prepare result to byte[]
            var result = memoryStream.ToArray();
            await memoryStream.DisposeAsync();
            return result;
        }
        public static Task<byte[]> Compress(Stream stream, int quality)
        {
            using var image = Image.Load(stream);
            stream.DisposeAsync();
            var resizeOptions = new ResizeOptions
            {
                Size = image.Size(),
                Sampler = KnownResamplers.Lanczos3,
                Compand = true,
                Mode = ResizeMode.Stretch
            };

            return HelpFunction(image, resizeOptions, quality);
        }

        public static Task<byte[]> Resize(Stream stream, int width, int height)
        {
            using var image = Image.Load(stream);
            stream.DisposeAsync();
            var resizeOptions = new ResizeOptions
            {
                Size = new Size(width, height),
                Sampler = KnownResamplers.Lanczos3,
                Compand = true,
                Mode = ResizeMode.Stretch
            };

            return HelpFunction(image, resizeOptions);
        }
        public static Task<byte[]> Resize(Stream stream, int maxWidth)
        {
            using var image = Image.Load(stream);
            stream.DisposeAsync();
            var resizeOptions = new ResizeOptions
            {
                Size = new Size(maxWidth, image.Height * (maxWidth / image.Width)),
                Sampler = KnownResamplers.Lanczos3,
                Compand = true,
                Mode = ResizeMode.Stretch
            };

            return HelpFunction(image, resizeOptions);
        }
    }
}
