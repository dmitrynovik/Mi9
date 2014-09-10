using Newtonsoft.Json;
using System;

namespace DmitryNovik.Mi9.Lib.Models
{
    public class ShowImage
    {
        public string showImage { get; set; }
    }

    public abstract class Show
    {
        public ShowImage image { get; set; }
        public string slug { get; set; }
        public string title { get; set; }
    }

    public class ShowInResponse : Show {  }

    public class ShowInRequest : Show
    {
        public bool drm { get; set; }
        public int episodeCount { get; set; }
    }


}
