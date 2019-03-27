using System;
using System.ComponentModel.DataAnnotations;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// classs for valiadte Date Of birth in admin
    /// </summary>
    public sealed class DateOfBirthFilterAttribute : ValidationAttribute
    {
        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        /// <summary>
        /// Validate dateOfBirth
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var val = (DateTime)value;

            if ((val.AddYears(MinAge) > DateTime.Now) || ( string.Compare(val.ToShortDateString(),DateTime.Now.ToShortDateString(),StringComparison.OrdinalIgnoreCase) == 0))
                return false;

            return (val.AddYears(MaxAge) > DateTime.Now);
        }
    }
}
