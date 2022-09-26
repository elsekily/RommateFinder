using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoommateFinderAPI.Core;
using RoommateFinderAPI.Entities.Models;
using RoommateFinderAPI.Entities.Resources;

namespace RoommateFinderAPI.Controllers
{
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRoomRepository repository;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public RoomController(IUnitOfWork unitOfWork, IRoomRepository repository, UserManager<User> userManager, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetRooms(double latitude, double longitude, int maxDistance)
        {
            var rooms = await repository.GetRooms(latitude, longitude, maxDistance);
            var result = mapper.Map<IEnumerable<Room>, IEnumerable<RoomResource>>(rooms);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            var room = await repository.GetRoom(id);
            if (room == null)
                return NotFound();

            var result = mapper.Map<Room, RoomResource>(room);
            return Ok(result);
        }
        [Authorize(Policy = Policies.Owner)]
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomSaveResource roomResource)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var room = mapper.Map<RoomSaveResource, Room>(roomResource);

            room.Owner = await GetUser();
            room.UserId = room.Owner.Id;

            repository.Add(room);

            await unitOfWork.CompleteAsync();

            room = await repository.GetRoom(room.Id);
            var result = mapper.Map<Room, RoomResource>(room);
            return Created(nameof(CreateRoom), result);
        }
        [Authorize(Policy = Policies.Owner)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] RoomSaveResource roomResource)
        {
            var room = await repository.GetRoom(id);

            if (!ModelState.IsValid)
                return BadRequest();

            if (room == null)
                return NotFound();

            var user = await GetUser();

            if (user.Id != room.Owner.Id)
                return Unauthorized();

            var maxLongitude = 180;
            var maxLatitude = 90;
            roomResource.Latitude = roomResource.Latitude > maxLatitude ? room.Location.Coordinate.X : roomResource.Latitude;
            roomResource.Longitude = roomResource.Longitude > maxLongitude ? room.Location.Coordinate.Y : roomResource.Longitude;
            mapper.Map<RoomSaveResource, Room>(roomResource, room);

            await unitOfWork.CompleteAsync();

            room = await repository.GetRoom(room.Id);
            var result = mapper.Map<Room, RoomResource>(room);
            return Accepted(result);
        }
        [Authorize(Policy = Policies.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            var room = await repository.GetRoom(id);

            if (room == null)
                return NotFound();

            var user = await GetUser();

            if (user.Id != room.Owner.Id)
                return Unauthorized();

            repository.Remove(room);
            await unitOfWork.CompleteAsync();

            return Accepted();
        }
        private async Task<User> GetUser()
        {
            var email = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
            return await userManager.FindByEmailAsync(email);
        }
    }
}