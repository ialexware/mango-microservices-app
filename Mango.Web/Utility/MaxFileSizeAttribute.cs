using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utility
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var filte = (IFormFile)value;
            if (filte != null)
            {
                if (filte.Length > (_maxFileSize * 1024 * 1024))
                {
                    return new ValidationResult($"File size can not be more than {_maxFileSize} MB");
                }
            }
            return ValidationResult.Success;
        }
    }
}
