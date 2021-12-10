using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ImageCompress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageAsyncController : ControllerBase
    {
        [HttpPost("Compress")]

        public async Task<IActionResult> Compress([FromForm] IFormFile file, int compressQuality)
        {
            return File(ImageAsyncHelper.Compress(file.OpenReadStream(), 100 - compressQuality).Result, "image/jpeg");
        }

        [HttpPost("resizeByMaxWidth")]
        public async Task<IActionResult> ResizeByMaxWidth([FromForm] IFormFile file, int maxWidth)
        {
            return File(ImageAsyncHelper.Resize(file.OpenReadStream(), maxWidth).Result, "image/jpeg");
        }

        [HttpPost("resize")]
        public Task<FileContentResult> Resize([FromForm] IFormFile file, int width, int height)
        {
            return Task.FromResult(File(ImageAsyncHelper.Resize(file.OpenReadStream(), width, height).Result, "image/jpeg"));
        }

        [HttpGet("CompressByUrl")]
        public async Task<IActionResult> CompressByUrl(string url, int compressQuality)
        {
            using var client = new WebClient();
            await using Stream stream = new MemoryStream(client.DownloadData(url));
            return File(ImageAsyncHelper.Compress(stream, 100 - compressQuality).Result, "image/jpeg");
        }

        [HttpGet("resizeByUrl")]
        public async Task<IActionResult> ResizeByUrl(string url, int width, int height)
        {
            using var client = new WebClient();
            await using Stream stream = new MemoryStream(client.DownloadData(url));
            return File(ImageAsyncHelper.Resize(stream, width, height).Result, "image/jpeg");
        }
        [HttpGet("resizeByMaxWidthByUrl")]
        public async Task<IActionResult> ResizeByUrl(string url, int maxWidth)
        {
            using var client = new WebClient();
            await using Stream stream = new MemoryStream(client.DownloadData(url));
            return File(ImageAsyncHelper.Resize(stream, maxWidth).Result, "image/jpeg");
        }
    }
}
