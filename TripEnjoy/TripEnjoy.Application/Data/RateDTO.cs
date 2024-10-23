using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Data
{
    public class RateDTO
    {
        public int RateId { get; set; }
        public int RateValue { get; set; }
        public DateTime RateDate { get; set; }
        public int RoomId { get; set; }
        public int AccountId { get; set; }
        public string FullName { get; set; }

    }
}
