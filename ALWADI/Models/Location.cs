using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class Location
    {
        [Display(Name = "المعرف")]
        public int Id { get; set; }



        [Display(Name = "الموقع")]
        public string  location { get; set; }


        public virtual ICollection<Doctor> Doctors { get; set; }

    }
}
