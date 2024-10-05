using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class EmailConfirm
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }   
    }
}
