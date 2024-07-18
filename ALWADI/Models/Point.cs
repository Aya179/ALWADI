using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class Point
    {

        public int id { set; get; }
        public int ads_id { set; get; }

        public bool is_deleted { set; get; }
        [Display(Name = "عدد النقاط")]
        public int value { set; get; }

        public virtual Advertisement advertismentNavigation { get; set; }




    }
}
