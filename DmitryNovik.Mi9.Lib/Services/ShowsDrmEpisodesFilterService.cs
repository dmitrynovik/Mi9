using DmitryNovik.Mi9.Lib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DmitryNovik.Mi9.Lib.Services
{
    public class ShowsDrmEpisodesFilterService : IShowsFilterService
    {
        private readonly JsonSerializer _serializer;

        public ShowsDrmEpisodesFilterService()
        {
            // Ignore nulls:
            _serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,                
            });
        }

        public JsonSerializer Serializer
        {
            get
            {
                return _serializer;
            }
        }

        public string Filter(ShowRequest request, out string error)
        {
            try
            {
                var response = new ShowResponse() 
                { 
                    response = request.payload.Where(s => s.drm && s.episodeCount > 0)
                        .Select(s => new ShowInResponse() { image = s.image.showImage, slug = s.slug, title = s.title })
                };
                error = null;
                return Serialize(response);
            }
            catch (Exception)
            {
                // TODO: Log or otherwise process the Error
                error = "Could not decode request: JSON parsing failed";
                return Serialize(new ShowResponse() { error = error });
            }
        }

        private string Serialize(ShowResponse response)
        {
            using (var writer = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                _serializer.Serialize(jsonWriter, response);
                return writer.ToString();
            }
        }
    }
}