using System;

namespace DmitryNovik.Mi9.Lib.Services
{
    public class EpisodeDrmFilterService : ShowsFilterService
    {
        protected override Func<Models.ShowInRequest, bool> Predicate
        {
            get
            {
                return show => show.drm && show.episodeCount > 0;
            }
        }
    }
}
