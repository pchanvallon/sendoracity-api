using System.ComponentModel.DataAnnotations;

namespace SendoraCityApi.Services.Models;

public class CityUpdateRequest
{
    [RegularExpression(@"^[a-zA-Z- ]*$",
        ErrorMessage = "City name must only contain letters, dashes and spaces.")]
    public string? Name { get; init; }
    public bool? Touristic { get; init; }
}
