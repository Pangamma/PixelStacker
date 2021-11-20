using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PixelStacker.Web.Net.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AcceptableIntValuesAttribute : ValidationAttribute
    {
        public int[] AllowableValues { get; set; }

        public AcceptableIntValuesAttribute(params int[] values)
        {
            AllowableValues = values;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int i = Convert.ToInt32(value);
            if (AllowableValues?.Contains(i) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter one of the allowable values: {string.Join(", ", AllowableValues)}.";
            return new ValidationResult(msg);
        }
    }
}
