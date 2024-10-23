using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class Account
    {
        public int AccountId { get; set; } 
        public string AccountUsername { get; set; } 
        public string AccountFullname { get; set; } 
        public string AccountPhone { get; set; } 
        public string AccountEmail { get; set; } 
        public string AccountAddress { get; set; }
        public string AccountGender { get; set; } 
    }
}
