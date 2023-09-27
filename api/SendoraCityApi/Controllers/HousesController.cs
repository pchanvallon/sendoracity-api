using Microsoft.AspNetCore.Mvc;
using SendoraCityApi.Services.Models;
using SendoraCityApi.Services;

namespace SendoraCityApi.Controllers;

[ApiController]
[Route("houses")]
public class HousesController : Controller
{
    private readonly IHousesService _housesService;

    public HousesController(IHousesService housesService)
        => _housesService = housesService;

    [HttpGet]
    [ProducesResponseType(typeof(List<HouseResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListHouses([FromQuery] int? cityid, [FromQuery] string? cityname)
    {
        if (cityid is not null && cityname is not null)
        {
            return BadRequest("Cannot specify both cityid and cityname");
        }

        try
        {
            if (cityid is not null)
            {
                return Ok(await _housesService.GetHousesByCityIdAsync(cityid.Value));
            }
            else if (cityname is not null)
            {
                return Ok(await _housesService.GetHousesByCityNameAsync(cityname));
            }
            else
            {
                return Ok(await _housesService.GetHousesAsync());
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HouseResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHouseById(int id)
    {
        try
        {
            return Ok(await _housesService.GetHouseByIdAsync(id));
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
    [ProducesResponseType(typeof(HouseResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddHouse([FromBody] HouseCreateRequest request)
    {
        try
        {
            return Created(string.Empty, await _housesService.AddHouseAsync(request));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(HouseResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateHouse(int id, [FromBody] HouseUpdateRequest request)
    {
        try
        {
            return Ok(await _housesService.UpdateHouseAsync(id, request));
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
    [ProducesResponseType(typeof(HouseResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteHouse(int id)
    {
        try
        {
            return Ok(await _housesService.DeleteHouseAsync(id));
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
