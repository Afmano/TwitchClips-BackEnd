using AutoMapper;
using TwitchLib.Api.Helix.Models.Clips.GetClips;

namespace TwitchClips.Models.MapProfile
{
    public class ClipProfile : Profile
    {
        public ClipProfile()
        {
            CreateMap<Clip, SavedClip>();
        }
    }
}