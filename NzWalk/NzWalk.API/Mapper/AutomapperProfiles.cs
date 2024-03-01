using AutoMapper;
using NzWalk.API.Model.Domain;
using NzWalk.API.Model.DTO;

namespace NzWalk.API.Mapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {          
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkRegionRequestDto, Walk>().ReverseMap();
            CreateMap<UpdateWalkDto, Walk>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
