using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class Client
    {

        [Display(Name = "المعرف")]

        public int clientId { set; get; }
        [Display(Name = "اسم المعلن")]

        public string clientname { set; get; }
        [Display(Name = "رقم الهاتف")]

        public string phone { set; get; }
        [Display(Name = "الايميل")]

        public string email { set; get; }
        [Display(Name = "العنوان")]
        public  string address { set; get; }
        [Display(Name = "اسم الشركة")]

        public string company { set; get; }
        public int client_profit { set; get; }
        public bool Is_deleted { set; get; }

        public virtual ICollection<client_pament> Client_Paments { get; set; }
        public virtual ICollection<Advertisement> Advertisements { get; set; }


    }
}
