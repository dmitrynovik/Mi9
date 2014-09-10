using DmitryNovik.Mi9.Lib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DmitryNovik.Mi9.Lib.Services
{
    public class ShowsFilterService
    {
        private static readonly JsonSerializer _serializer;

        static ShowsFilterService()
        {
            _serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,                
            });
        }

        public ShowResponse Filter(ShowRequest request, Func<ShowInRequest, bool> predicate)
        {
            try
            {
                return new ShowResponse() 
                { 
                    response = request.payload.Where(predicate)
                        .Select(s => new ShowInResponse() { 
                            image = s.image.showImage, 
                            slug = s.slug, 
                            title = s.title 
                        })
                };
            }
            catch (Exception)
            {
                // TODO: Log or otherwise process the Error
                return ShowResponse.Invalid();
            }
        }

        public ShowResponse Filter(Stream requestStream, Func<ShowInRequest, bool> predicate)
        {
            return Filter(Deserialize(requestStream), predicate);
        }

        private static ShowRequest Deserialize(Stream stream)
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