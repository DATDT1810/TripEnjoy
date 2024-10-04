using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Domain.Models
{
    public class HotelQuestions
    {
        [Key]
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionAnswer { get; set; }
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
    }
}
