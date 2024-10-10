using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class UserProfile
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        [NotAnonymousCustomer]
        public string AccountFullname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+\d{11,15}$", ErrorMessage = "Phone number must be in the format +<country code><number>, e.g. +84869251053.")]
        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
        public string AccountPhone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string AccountAddress { get; set; }

        
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string AccountGender { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AccountDateOfBirth { get; set; }


        public string AccountImage { get; set; }
    }

    public class NotAnonymousCustomerAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string fullname)
            {
                if (fullname.Equals("Anonymous Customer", StringComparison.OrdinalIgnoreCase))
                {
                    return new ValidationResult("Full name cannot be 'Anonymous Customer'. Please enter a different name.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
