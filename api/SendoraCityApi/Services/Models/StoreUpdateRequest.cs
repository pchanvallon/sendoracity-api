using System.ComponentModel.DataAnnotations;
using SendoraCityApi.Services.Attributes;

namespace SendoraCityApi.Services.Models;

public partial class StoreUpdateRequest : BuildingUpdateRequest
{
    [RegularExpression(@"^[0-9a-zA-Z- ]*$",
        ErrorMessage = "Name must only contain numbers, letters, dashes and spaces.")]
    public string? Name { get; init; }

    [StoreTypeEnum]
    public string? Type { get; init; }
}
