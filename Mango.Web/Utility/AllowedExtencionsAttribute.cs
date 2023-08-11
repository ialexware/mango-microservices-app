using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utility
{
    public class AllowedExtencionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtencions;

        public AllowedExtencionsAttribute(string[] allowedExtencions)
        {
            _allowedExtencions = allowedExtencions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var filte = (IFormFile)value;
            if (filte != null)
            {
                var ext = Path.GetExtension(filte.FileName);
                if (!_allowedExtencions.Contains(ext.ToLower()))
                {
                    return new ValidationResult($"Allowed extencions are {string.Join(",", _allowedExtencions)}");
                }
            }
            return ValidationResult.Success;
        }

    }
}
