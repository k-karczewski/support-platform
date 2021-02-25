using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class FileToUploadDto 
    {
        public string Filename { get; set; }
        public byte[] FileInBytes { get; set; }
    }
}
