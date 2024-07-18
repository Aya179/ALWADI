using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class Advertisement
    {

        [Display(Name = "المعرف")]
        public int id { get; set; }
        [Display(Name = "الصورة")]
        //[Required(ErrorMessage = "هذا الحقل اجباري")]
        public byte[] image { get; set; }
        [Display(Name = "رابط الإعلان")]

        public string URL { get; set; }
        [Display(Name = "اسم المعلن")]
        public int client_id { set; get; }

        [Display(Name = "تاريخ العرض")]
        public DateTime Ads_StartDate { set; get; }
        public int Ads_cost { set; get; }
        [Display(Name = "المكان")]


        public string Ads_place { set; get; }
        public DateTime Ads_EndDate { set; get; }
        public bool is_deleted { set; get; }
        [Display(Name = "نسبة الحسم")]
        public int Ads_profit { set; get; }
        [Display(Name = "فئة الإعلان")]

        public int Ads_category_id { set; get; }

        [Display(Name = "اسم المعلن")]

        public virtual Client clientNavigation { get; set; }
        [Display(Name = "فئة الإعلان")]

        public virtual Category categoryNavigation { get; set; }

        public virtual ICollection<Point> Points { get; set; }





    }
}
