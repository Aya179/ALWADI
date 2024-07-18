using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class advStatistics
    {
        public int id { get; set; }
        public string client { set; get; }  
        public DateTime Ads_StartDate { set; get; }
        public DateTime Ads_endDate { set; get; }
        public int duration { set; get; }
        public float profits { set; get; }
        public byte[] image { get; set; }
        [NotMapped]
        public string ImageToShow { get; set; }

    }
}
