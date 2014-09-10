using DmitryNovik.Mi9.Lib.Models;
using System;
using System.Collections.Generic;

namespace DmitryNovik.Mi9.Lib.Services
{
    public class ShowRequest
    {
        public IEnumerable<ShowInRequest> payload { get; set; }
    }
}
