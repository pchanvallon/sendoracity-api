using System.ComponentModel.DataAnnotations;
using SendoraCityApi.Services.Enums;

namespace SendoraCityApi.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class StoreTypeEnumAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
            => Enum.GetNames<StoreType>().Contains(value?.ToString());

        public override string FormatErrorMessage(string type)
            => $"Value must be one of: {string.Join(", ", Enum.GetNames<StoreType>())}.";
    }
}