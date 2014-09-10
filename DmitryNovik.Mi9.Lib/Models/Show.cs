using System;

namespace DmitryNovik.Mi9.Lib.Models
{
    public abstract class Show
    {
        public string image { get; set; }
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
