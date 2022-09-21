using System.Security.AccessControl;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoommateFinderAPI.Core;
using RoommateFinderAPI.Entities.Models;
using RoommateFinderAPI.Entities.Resources;

namespace RoommateFinderAPI.Controllers
{
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITagRepository repository;
        private readonly IMapper mapper;

        public TagController(IUnitOfWork unitOfWork, ITagRepository repository, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(Guid id)
        {
            var tag = await repository.GetTag(id);
            if (tag == null)
                return NotFound();

            var result = mapper.Map<Tag, TagResource>(tag);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await repository.GetTags();
            var result = mapper.Map<IEnumerable<Tag>, IEnumerable<TagResource>>(tags);
            return Ok(result);
        }
        [Authorize(Policy = Policies.Moderator)]
        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] TagSaveResource tagResource)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var tag = mapper.Map<TagSaveResource, Tag>(tagResource);
            repository.Add(tag);

            await unitOfWork.CompleteAsync();

            tag = await repository.GetTag(tag.Id);
            var result = mapper.Map<Tag, TagResource>(tag);
            return Created(nameof(GetTag), result);
        }
        [Authorize(Policy = Policies.Moderator)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] TagSaveResource tagResource)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var tag = await repository.GetTag(id);
            if (tag == null)
                return NotFound();

            mapper.Map<TagSaveResource, Tag>(tagResource, tag);

            await unitOfWork.CompleteAsync();

            tag = await repository.GetTag(tag.Id);
            var result = mapper.Map<Tag, TagResource>(tag);
            return Accepted(result);
        }
        [Authorize(Policy = Policies.Moderator)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await repository.GetTag(id);

            if (tag == null)
                return NotFound();

            repository.Remove(tag);
            await unitOfWork.CompleteAsync();

            return Accepted();
        }
    }
}