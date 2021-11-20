using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PixelStacker.Web.Net.Models.Attributes
{
    /// <summary>
    /// Only ever use on string types. Use this to automatically set a phone number field to be in
    /// stripped down yet padded format.
    /// 14251231234
    /// </summary>
    public class PhoneNumberFormatAttribute : ValidationAttribute
    {
        public PhoneNumberFormatAttribute()
        {
        }

        public static string ToDatabaseStyle(string unformattedPhoneNumber)
        {
            // strip off non numeric characters
            string nVal = new string(unformattedPhoneNumber.ToCharArray().Where(c => char.IsDigit(c)).ToArray());

            // Missing the country code most likely.
            if (nVal.Length == 10)
            {
                nVal = "1" + nVal;
            }

            return nVal;
        }

        public static string ToReadableStyle(string unformattedPhoneNumber)
        {
            if (unformattedPhoneNumber.Length != 11)
            {
                unformattedPhoneNumber = ToDatabaseStyle(unformattedPhoneNumber);
                if (unformattedPhoneNumber.Length != 11)
                {
                    return null; // Give up.
                }
            }

            string nbr = unformattedPhoneNumber.Substring(0, 1)
                + "-" + unformattedPhoneNumber.Substring(1, 3)
                + "-" + unformattedPhoneNumber.Substring(4, 3)
                + "-" + unformattedPhoneNumber.Substring(7, 4);

            return nbr;
        }

        protected override ValidationResult IsValid(object valueObj, ValidationContext validationContext)
        {
            if (!(valueObj is string))
                return new ValidationResult("Property must be a string type.");

            string oVal = (string)valueObj;
            if (string.IsNullOrEmpty(oVal))
                return ValidationResult.Success;

            string nVal = ToDatabaseStyle(oVal);

            // Missing the country code most likely.
            if (nVal != oVal)
            {
                validationContext
                    .ObjectType
                    .GetProperty(validationContext.MemberName)
                    .SetValue(validationContext.ObjectInstance, nVal, null);
            }

            return ValidationResult.Success;
        }
    }
}
