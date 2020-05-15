using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlantDataAPI.Models;
using PlantDataAPI.Models.Entities;

namespace PlantDataAPI.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/plant")]
    public class PlantController : Controller
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        [HttpGet("{plantId}")]
        public async Task<ActionResult<Plant>> GetPlantByID([FromRoute] int plantId)
        {
            Plant plant = await PlantModel.GetPlantByID(plantId);

            if (plant == null)
                return NotFound();
            return plant;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        [HttpGet("attributes/{plantId}")]
        public async Task<ActionResult<List<Attribute>>> GetAttributesForPlant(int plantId)
        {
            List<Attribute> attributes = await PlantModel.GetAttributesForPlant(plantId);
            if (attributes.Count > 0)
                return attributes;
            return new NotFoundResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        [HttpGet("region-shape-files/{plantId}")]
        public async Task<ActionResult<List<RegionShapeFile>>> GetRegionShapeFilesForPlant(int plantId)
        {
            List<RegionShapeFile> regionShapeFiles = await PlantModel.GetRegionShapeFilesForPlant(plantId);
            if (regionShapeFiles.Count > 0)
                return regionShapeFiles;
            return new NotFoundResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        [HttpGet("images/{plantId}")]
        public async Task<ActionResult<List<Image>>> GetImagesForPlant([FromRoute] int plantId)
        {
            List<Image> images = await PlantModel.GetImagesForPlant(plantId);
            if (images.Count > 0)
                return images;
            return new NotFoundResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        [HttpPost("add-plant")]
        public async Task<ActionResult> AddPlant([Required, FromBody] Plant plant)
        {
            if (await PlantModel.AddPlant(plant))
                return Ok();


            return new BadRequestResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        [HttpPost("add-attribute")]
        public async Task<ActionResult> AddAttribute([Required, FromBody] Attribute attribute)
        {
            if (await PlantModel.AddAttribute(attribute))
                return Ok();
            return new BadRequestResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="regionShapefile"></param>
        /// <returns></returns>
        [HttpPost("add-region-shape-file")]
        public async Task<ActionResult> AddRegionShapeFile(RegionShapeFile regionShapefile)
        {
            if (await PlantModel.AddRegionShapeFile(regionShapefile))
                return Ok();
            return new BadRequestResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("add-image")]
        public async Task<ActionResult> AddImage([Required, FromBody] Image image)
        {
            if (await PlantModel.AddImage(image))
                return Ok();
            return new BadRequestResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public string SearchPlants([Required, FromQuery] string searchQuery)
        {
            return string.Empty;
        }
    }
}