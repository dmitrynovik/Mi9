using DmitryNovik.Mi9.Lib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DmitryNovik.Mi9.Lib.Services
{
    public abstract class ShowsFilterService
    {
        private static readonly JsonSerializer _serializer;

        protected abstract Func<ShowInRequest, bool> Predicate { get; }

        static ShowsFilterService()
        {
            _serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,                
            });
        }

        public ShowResponse Filter(ShowRequest request)
        {
            if (request != null && request.payload != null)
            {
                return new ShowResponse() 
                { 
                    response = request.payload.Where(Predicate).Select(s => new ShowInResponse() { 
                            image = s.image.showImage, 
                            slug = s.slug, 
                            title = s.title 
                        })
                };
            }
            else
            {
                return ShowResponse.Invalid();
            }
        }

        public ShowResponse Filter(Stream requestStream)
        {
            // Might be invalid JSON (wrap in try / catch)
            try
            {
                return Filter(Deserialize(requestStream));
            }
            catch (Exception)
            {
                // TODO: Log or otherwise process the Error
                return ShowResponse.Invalid();
            }            
        }

        public static ShowRequest Deserialize(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var sRequest = reader.ReadToEnd();
                using (var stringReader = new StringReader(sRequest))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<ShowRequest>(jsonReader);
                }
            }
        }

        public static string Serialize(ShowResponse response)
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