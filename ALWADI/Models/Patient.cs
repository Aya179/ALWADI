using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ALWADI.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int PatientId { get; set; }

        [Display(Name = "اسم المريض")]
        public string PatientName { get; set; }

        [Display(Name = "رقم المريض")]
        public string PatientPhone { get; set; }

        [Display(Name = "عنوان المريض")]
        public string PatientAddress { get; set; }

        [Display(Name = "صورة المريض")]
        public byte[] PatientImg { get; set; }

        [Display(Name = "كلمة السر")]
        public string PatientPassword { get; set; }
        public bool? IsActive { get; set; }

        [Display(Name = "موقع المريض")]
        public string PatientLocation { get; set; }

        [Display(Name = "كود التفعيل")]
        public string ActivationCode { get; set; }

        [Display(Name = "تاريخ التفعيل")]
        public DateTime? ActivationDate { get; set; }
        public bool? IsDeleted { get; set; }

        [Display(Name = "عمر المريض")]
        public int? PatientAge { get; set; }

        [Display(Name = "جنس المريض")]
        public string PatientGender { get; set; }

        public DateTime? RegistrationDate { get; set; }
        public string Token { get; set; }


        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
