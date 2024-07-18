using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class Payment
    {


         public int Id { set; get; }
        [Display(Name = "تاريخ الموعد")]
        public DateTime PaymentDate { set; get; }
         public int appoitmentId { set; get; }
        [Display(Name = "حصة المركز")]
        public float centerCost { set; get; }
        [Display(Name = "حصة الموقع")]
        public float webCost { set; get; }
        [Display(Name = "حصة الطبيب")]
        public float doctorCost { set; get; }

        [Display(Name = "مدفوعة")]
        public bool  ispaid { set; get; }
        [Display(Name = "   لأجل مدفوعة")]
        public string paidedFor { set; get; }
        // public virtual Appointment appoitmentIdNavigation { get; set; }



    }
}
