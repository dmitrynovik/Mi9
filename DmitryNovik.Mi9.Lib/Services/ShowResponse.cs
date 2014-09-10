using DmitryNovik.Mi9.Lib.Models;
using System;
using System.Collections.Generic;

namespace DmitryNovik.Mi9.Lib.Services
{
    public class ShowResponse
    {
        public IEnumerable<ShowInResponse> response { get; set; }
        public string error { get; set; }
    }
}
