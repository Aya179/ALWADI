using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ALWADI.Models
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }

        [Display(Name = "وقت بدء الموعد")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "وقت انتهاء الموعد")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "اسم المريض")]
        public int? Patient { get; set; }

        [Display(Name = "اسم الطبيب")]
        public int? Doctor { get; set; }

        [Display(Name = "التاريخ")]
        public DateTime? Date { get; set; }

        [Display(Name = "ملاحظات")]
        public string Note { get; set; }

        [Display(Name = "تأكيد")]
        [DefaultValue(false)]
        public bool? IsApproved { get; set; }

        [Display(Name = "تم")]
        public bool? IsDone { get; set; }

        [Display(Name = "حجز")]
        public bool?IsReversed { get; set; }
        [Display(Name = "التقييم")]
        public int Rating { set; get; }

        public virtual Doctor DoctorNavigation { get; set; }
        public virtual Patient PatientNavigation { get; set; }
        //public virtual ICollection<Payment> Payments { get; set; }
        

    }
}
