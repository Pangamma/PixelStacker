using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PixelStacker.Web.Net.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AcceptableStringValuesAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        public AcceptableStringValuesAttribute(params string[] values)
        {
            AllowableValues = values;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (AllowableValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter one of the allowable values: {string.Join(", ", AllowableValues ?? new string[] { "No allowable values found" })}.";
            return new ValidationResult(msg);
        }
    }
}
