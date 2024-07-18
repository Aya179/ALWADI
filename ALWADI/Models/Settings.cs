using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class Settings
    {
        public int Id { set; get; }
        //public int payment { set; get; }
        [Display(Name = "حصة المركز")]
        public float centerCost { set; get; }
        [Display(Name = "حصة الموقع")]
        public float webCost { set; get; }
        [Display(Name = "حصة الطبيب")]
        public float doctorCost { set; get; }
        //public virtual Payment paymentNavigation { get; set; }

    }
}
