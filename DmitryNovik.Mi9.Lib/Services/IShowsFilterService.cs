using Newtonsoft.Json;
using System;

namespace DmitryNovik.Mi9.Lib.Services
{
    public interface IShowsFilterService
    {
        string Filter(ShowRequest request, out string error);
        JsonSerializer Serializer { get; }
    }
}
