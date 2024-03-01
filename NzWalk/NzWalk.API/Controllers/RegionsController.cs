using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalk.API.CustomActionFilter;
using NzWalk.API.Data;
using NzWalk.API.Model.Domain;
using NzWalk.API.Model.DTO;
using NzWalk.API.Repositories;
using System.Collections.Generic;

namespace NzWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,IMapper mapper)
        {
            this._context = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // Get data for all regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from Database - Domain Model
            var regionsDomain = await regionRepository.GetAllAsync();

            //Return Dto
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }


        // Get data for all regions by id
        [HttpGet] 
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get data from database based on id.
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Return Dto to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        // Create region
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           // Convert DTO to DomainModel
           var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

           regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

           // Convert DomainModel to DTO
           var regionDto = mapper.Map<RegionDto>(regionDomainModel);

           return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);

        }

        //Update region by id
        [HttpPut("{id:Guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> Update([FromRoute] Guid id,UpdateRegionRequestDto updateRegionRequestDto)
        {         
                //var regionDomainModel = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                // Convert DomainModel to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
        }

        //Delete region by id
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // Convert DomainModel to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            
            return Ok(regionDto);
        }
    }
}
