using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Models
{
    public class AppointmentGet
    {
       
        public int Id { get; set; }


        [Display(Name = "اسم المريض")]
        public string PatientName { get; set; }
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }
        [Display(Name = "الاختصاص")]
        public string specilizationName { get; set; }
        [Display(Name = "اسم الطبيب")]
        public string DoctorName { get; set; }
        [Display(Name = "التاريخ")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }

        [Display(Name = "الموقع")]
        public string location { get; set; }
        [Display(Name = "الرقم")]
        public string phone { get; set; }

        [Display(Name = "حصة المركز")]
        public float centerCost { set; get; }
        [Display(Name = "حصة الموقع")]
        public float webCost { set; get; }
        [Display(Name = "حصة الطبيب")]
        public float doctorCost { set; get; }
        [Display(Name = "مدفوعة")]

        public bool ispaid { set; get; }
         public int appId { get; set; }
        [Display(Name = "رقم الدفعة")]
        public int payId { get; set; }
        [Display(Name = "مدفوعة لأجل")]
        public string paidedFor { set; get; }



        [Display(Name = "تاريخ الموعد")]
        public DateTime PaymentDate { set; get; }


        public bool? IsDone { get; set; }
    }
}
