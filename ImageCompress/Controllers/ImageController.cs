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
    public class ImageController : ControllerBase
    {
        [HttpPost("Compress")]

        public  IActionResult Compress([FromForm] IFormFile file,int compressQuality)
        {
            return File(ImageHelper.Compress(file.OpenReadStream(), 100 - compressQuality), "image/jpeg");
        }

        [HttpPost("resizeByMaxWidth")]
        public IActionResult ResizeByMaxWidth([FromForm] IFormFile file,int maxWidth)
        {

            return File(ImageHelper.Resize(file.OpenReadStream(), maxWidth), "image/jpeg");
        }
        [HttpPost("resize")]
        public IActionResult Resize([FromForm] IFormFile file,int width,int height) => File(ImageHelper.Resize(file.OpenReadStream(), width, height), "image/jpeg");
        [HttpGet("CompressByUrl")]
        public IActionResult CompressByUrl(string url, int compressQuality)
        {
            using var client = new WebClient();
            using Stream stream = new MemoryStream(client.DownloadData(url));
            return File(ImageHelper.Compress(stream, 100 - compressQuality), "image/jpeg");
        }

        [HttpGet("resizeByUrl")]
        public IActionResult ResizeByUrl(string url, int width,int height)
        {
            using var client = new WebClient();
            using Stream stream = new MemoryStream(client.DownloadData(url));
            return File(ImageHelper.Resize(stream,width,height), "image/jpeg");
        }
        [HttpGet("resizeByMaxWidthByUrl")]
        public IActionResult ResizeByUrl(string url, int maxWidth)
        {
            using var client = new WebClient();
            using Stream stream = new MemoryStream(client.DownloadData(url));
            return File(ImageHelper.Resize(stream,maxWidth), "image/jpeg");
        }
    }
}
