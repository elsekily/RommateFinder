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


        CreateMap<User, UserResource>();
        CreateMap<Room, RoomResource>()
            .ForMember(rr => rr.Tags, opt => opt.MapFrom(r => r.RoomTags.Select(rt => new TagResource()
            { Name = rt.Tag.Name, Id = rt.TagId })))
            .ForMember(rr => rr.Owner, opt => opt.MapFrom(r => new UserResource()
            { Email = r.Owner.Email, UserId = r.Owner.Id, Name = r.Owner.UserName, PhoneNumber = r.Owner.PhoneNumber }));





        CreateMap<RoomSaveResource, Room>()
            .ForMember(r => r.Id, opt => opt.Ignore())
            .ForMember(r => r.RoomTags, opt => opt.Ignore())
            .ForMember(r => r.Location, opt => opt.MapFrom(rr => LocationUtilities.GetLocation(rr.Latitude, rr.Longitude)))
            .ForMember(r => r.Owner, opt => opt.Ignore())
            .ForMember(r => r.UserId, opt => opt.Ignore())
            .AfterMap((rr, r) =>
            {
                // Remove unselected tags
                var removedTags = r.RoomTags.Where(rt => !rr.TagIds.Contains(rt.TagId)).ToList();
                foreach (var t in removedTags)
                    r.RoomTags.Remove(t);

                // Add new tags
                var newTags = rr.TagIds.Where(ti => !r.RoomTags.Any(k => k.TagId == ti)).Select(ti => new RoomTag() { TagId = ti }).ToList();
                foreach (var t in newTags)
                {
                    r.RoomTags.Add(t);
                }
            });
    }
}