using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DmitryNovik.Mi9.Lib.Models
{
    public class ShowImage
    {
        public string showImage { get; set; }
    }

    public abstract class Show
    {
        public string slug { get; set; }
        public string title { get; set; }
    }

    public class ShowInResponse : Show 
    {
        public string image;
    }

    public class ShowInRequest : Show
    {
        public bool drm { get; set; }
        public int episodeCount { get; set; }
        public ShowImage image { get; set; }
    }

    public class ShowRequest
    {
        public IEnumerable<ShowInRequest> payload { get; set; }
    }

    public class ShowResponse
    {
        public IEnumerable<ShowInResponse> response { get; set; }
        public string error { get; set; }

        public static ShowResponse Invalid()
        {
            return new ShowResponse() { error = "Could not decode request: JSON parsing failed" };
        }
    }
}
