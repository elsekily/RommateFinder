using System.Security.AccessControl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoommateFinderAPI.Core;
using RoommateFinderAPI.Entities.Resources;

namespace RoommateFinderAPI.Controllers
{
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITagRepository repository;

        public TagController(IUnitOfWork unitOfWork,
            ITagRepository repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            var tag = await repository.GetTag(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await repository.GetTags();
            return Ok(tags);
        }
        [Authorize(Policy = Policies.Moderator)]
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientSaveResource clientResource)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = mapper.Map<ClientSaveResource, Client>(clientResource);
            repository.Add(client);

            await unitOfWork.CompleteAsync();

            client = await repository.GetClient(client.Id);
            var result = mapper.Map<Client, ClientResource>(client);
            return Created(nameof(GetClient), result);
        }


    }
}