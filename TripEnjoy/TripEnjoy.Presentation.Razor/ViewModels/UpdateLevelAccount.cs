using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
	public class UpdateLevelAccount
	{
		public int AccountId { get; set; }  

		public int AccountRole { get; set; } 

		public bool AccountUpLevel { get; set; } 
 		public string UserId { get; set; } 

	}
}
