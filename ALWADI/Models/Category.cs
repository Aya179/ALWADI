using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class Category
    {
        [Display(Name = "المعرف")]
        public int id { set; get; }
        [Display(Name = "كلفة العرض( باليوم) ")]
        public int categoy_cost { set;get;}

        [Display(Name = "كلفة الضغطة")]
        public int pruch_cost { set; get; }
        [Display(Name = "اسم الفئة")]
        public string category_name { set; get; }
        [Display(Name = "مدة العرض(باليوم)")]
        public int duration { set; get; }
        public bool is_deleted { set; get; }


        public virtual ICollection<Advertisement> Advertisements { get; set; }

    }
}
