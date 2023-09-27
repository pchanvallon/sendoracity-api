using System.ComponentModel.DataAnnotations;

namespace SendoraCityApi.Services.Models;

public partial class HouseCreateRequest : BuildingCreateRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int? Inhabitants { get; init; }
}
