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
            var email = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
            var user = await userManager.FindByEmailAsync(email);
            room.Owner = user;
            room.UserId = user.Id;

            repository.Add(room);

            await unitOfWork.CompleteAsync();

            room = await repository.GetRoom(room.Id);
            var result = mapper.Map<Room, RoomResource>(room);
            return Created(nameof(CreateRoom), result);
        }
    }
}