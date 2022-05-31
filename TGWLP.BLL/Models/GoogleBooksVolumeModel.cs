using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGWLP.BLL.Models
{
    public class GoogleBooksVolumeModel
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string eTag { get; set; }
        public string SelfLink { get; set; }
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string PreviewLink { get; set; }
        public ImageLinks ImageLinks { get; set; }
    }

    public class ImageLinks
    {
        public Uri SmallThumbnail { get; set; }
        public Uri Thumbnail { get; set; }
    }
}
