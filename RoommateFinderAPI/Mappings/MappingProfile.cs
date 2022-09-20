using AutoMapper;
using RoommateFinderAPI.Entities.Models;
using RoommateFinderAPI.Entities.Resources;

namespace RoommateFinderAPI.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tag, TagResource>();



        CreateMap<TagSaveResource, Tag>()
               .ForMember(t => t.Id, opt => opt.Ignore())
               .ForMember(t => t.RoomTags, opt => opt.Ignore());

    }
}