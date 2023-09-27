using Microsoft.AspNetCore.Mvc;
using SendoraCityApi.Services.Models;
using SendoraCityApi.Services;

namespace SendoraCityApi.Controllers;

[ApiController]
[Route("stores")]
public class StoresController : Controller
{
    private readonly IStoresService _storesService;

    public StoresController(IStoresService storesService)
        => _storesService = storesService;

    [HttpGet]
    [ProducesResponseType(typeof(List<StoreResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListStores(
        [FromQuery] int? cityid,
        [FromQuery] string? cityname,
        [FromQuery] string? type
        )
    {
        if (cityid is not null && cityname is not null)
        {
            return BadRequest("Cannot specify both cityid and cityname");
        }

        try
        {
            if (cityid is not null && type is not null)
            {
                return Ok(await _storesService.GetStoresByCityIdAndTypeAsync(cityid.Value, type));
            }
            else if (cityname is not null && type is not null)
            {
                return Ok(await _storesService.GetStoresByCityNameAndTypeAsync(cityname, type));
            }
            else if (type is not null)
            {
                return Ok(await _storesService.GetStoresByTypeAsync(type));
            }
            if (cityid is not null)
            {
                return Ok(await _storesService.GetStoresByCityIdAsync(cityid.Value));
            }
            else if (cityname is not null)
            {
                return Ok(await _storesService.GetStoresByCityNameAsync(cityname));
            }
            else
            {
                return Ok(await _storesService.GetStoresAsync());
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStoreById(int id)
    {
        try
        {
            return Ok(await _storesService.GetStoreByIdAsync(id));
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
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddStore([FromBody] StoreCreateRequest request)
    {
        try
        {
            return Created(string.Empty, await _storesService.AddStoreAsync(request));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStore(int id, [FromBody] StoreUpdateRequest request)
    {
        try
        {
            return Ok(await _storesService.UpdateStoreAsync(id, request));
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
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteStore(int id)
    {
        try
        {
            return Ok(await _storesService.DeleteStoreAsync(id));
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
