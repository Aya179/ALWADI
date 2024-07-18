using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class client_pament
    {
        [Display(Name = "المعرف")]

        public int Id { set; get; }
        [Display(Name = "رقم الدفعة")]

        public int Payment_Num { set; get; }
        [Display(Name = "قيمة الدفعة")]

        public int Payment_value { set; get; }
        [Display(Name = "تاريخ الدفعة ")]

        public DateTime payment_date { set; get; }
        [Display(Name = "التفاصيل")]

        public string payment_details { set; get; }
        [Display(Name = "اسم المعلن")]
        public int Client_Id { set; get; }
        [Display(Name = "اسم المعلن")]

        public virtual Client clientNavigation { get; set; }

    }
}
