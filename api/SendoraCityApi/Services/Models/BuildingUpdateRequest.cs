using System.ComponentModel.DataAnnotations;

namespace SendoraCityApi.Services.Models;

public partial class BuildingUpdateRequest
{
    [RegularExpression(@"^[0-9a-zA-Z-, ]*$",
        ErrorMessage = "Address must only contain numbers, letters, commas, dashes and spaces.")]
    public string? Address { get; init; }

    [Range(1, int.MaxValue)]
    public int? Cityid { get; init; }
}
