using Microsoft.AspNetCore.Mvc;
using SendoraCityApi.Services.Models;
using SendoraCityApi.Services;

namespace SendoraCityApi.Controllers;

[ApiController]
[Route("cities")]
public class CitiesController : Controller
{
    private readonly ICitiesService _citiesService;

    public CitiesController(ICitiesService citiesService)
        => _citiesService = citiesService;

    [HttpGet]
    [ProducesResponseType(typeof(List<CityResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCities()
    {
        try
        {
            return Ok(await _citiesService.GetCitiesAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCityById(int id)
    {
        try
        {
            return Ok(await _citiesService.GetCityByIdAsync(id));
        }
        catch (ArgumentOutOfRangeException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddCity([FromBody] CityCreateRequest request)
    {
        try
        {
            return Created(string.Empty, await _citiesService.AddCityAsync(request));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] CityUpdateRequest request)
    {
        try
        {
            return Ok(await _citiesService.UpdateCityAsync(id, request));
        }
        catch (ArgumentOutOfRangeException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCity(int id)
    {
        try
        {
            return Ok(await _citiesService.DeleteCityAsync(id));
        }
        catch (ArgumentOutOfRangeException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
