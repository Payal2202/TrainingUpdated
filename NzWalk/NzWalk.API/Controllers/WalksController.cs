using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NzWalk.API.CustomActionFilter;
using NzWalk.API.Model.Domain;
using NzWalk.API.Model.DTO;
using NzWalk.API.Repositories;

namespace NzWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        //Create Walk
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddWalkRegionRequestDto addWalkRegionRequestDto)
        {      
                //Convert DTO to Domain
                var walkDomainModel = mapper.Map<Walk>(addWalkRegionRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                //Convert Domain to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
               
        }

        //Get Walk
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //Get data from Database - Domain Model
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery,sortBy,isAscending ?? true,pageNumber,pageSize);

            //Return Dto
            return Ok(mapper.Map<List<WalkDto>>(walkDomainModel));
        }

        //Get Walk ByID
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            //Return DomainModel  to Dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> Update([FromRoute] Guid id,UpdateWalkDto updateWalkDto)
        {
             // Convert Dto ro Domain
                var walkDomainModel = mapper.Map<Walk>(updateWalkDto);

                walkDomainModel = await walkRepository.GetUpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                //Return Dto to Client
                var walkDto = mapper.Map<UpdateWalkDto>(walkDomainModel);

                return Ok(walkDto);
  
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);   
            if (walkDomainModel == null)
            {
                return NotFound();  
            }
            // Convert DomainModel to DTO
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }
    }
}
