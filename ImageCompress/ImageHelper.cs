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
    public class ImageHelper
    {
        private static byte[] helpFunction(Image image,ResizeOptions resizeOptions,int quality=100)
        {
            

            image.Mutate(x => x
                .Resize(resizeOptions));

            var encoder = new JpegEncoder()
            {
                Quality = quality //Use variable to set between 5-30 based on your requirements
            };

            //This saves to the memoryStream with encoder
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, encoder);
            memoryStream.Position = 0; // The position needs to be reset.

            // prepare result to byte[]
            var result = memoryStream.ToArray();
            

            return result;
        }
        public static byte[] Compress(Stream stream, int quality)
        {
            using var image = Image.Load(stream);
            var resizeOptions = new ResizeOptions
            {
                Size = image.Size(),
                Sampler = KnownResamplers.Lanczos3,
                Compand = true,
                Mode = ResizeMode.Stretch
            };
            
            return helpFunction(image, resizeOptions, quality);
        }

        public static byte[] Resize(Stream stream, int width, int height)
        {
            using var image = Image.Load(stream);
            var resizeOptions = new ResizeOptions
            {
                Size = new Size(width,height),
                Sampler = KnownResamplers.Lanczos3,
                Compand = true,
                Mode = ResizeMode.Stretch
            };

            return helpFunction(image, resizeOptions);
        }
        public static byte[] Resize(Stream stream, int maxWidth)
        {
            using var image = Image.Load(stream);
            var resizeOptions = new ResizeOptions
            {
                Size = new Size(maxWidth, image.Height * (maxWidth / image.Width)),
                Sampler = KnownResamplers.Lanczos3,
                Compand = true,
                Mode = ResizeMode.Stretch
            };

            return helpFunction(image, resizeOptions);
        }
    }
}
