using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

#nullable disable

namespace ALWADI.Models
{
    public partial class Doctor
    {

        public Doctor()
        {

            Appointments = new HashSet<Appointment>();
        }

        public int DoctorId { get; set; }
       // [Required(ErrorMessage = "هذا الحقل اجباري")]

        [Display(Name="اسم الطبيب")]
        
        public string DoctorName { get; set; }
       // [Required(ErrorMessage = "هذا الحقل اجباري")]
        
        [Display(Name = "رقم الطبيب")]

        public string DoctorPhone { get; set; }
      //  [Required(ErrorMessage = "هذا الحقل اجباري")]

        [Display(Name = "ايميل الطبيب")]

        public string DoctorEmail { get; set; }
       // [Required(ErrorMessage = "هذا الحقل اجباري")]

        [Display(Name = "عنوان الطبيب")]

        public string DoctorAddrress { get; set; }
        [Display(Name = "عنوان العيادة")]
        public int clincLocation { get; set; }
       // [Required(ErrorMessage = "هذا الحقل اجباري")]

        [Display(Name = "جنس الطبيب")]

        public string DoctorGender { get; set; }

        [Display(Name = "اختصاص الطبيب")]

        public string DoctorSpecialization { get; set; }

        [Display(Name = "الكشفية")]
       

        public int? CostPerPatient { get; set; }

        [Display(Name = "الشهادة")]

        public string DoctorCertificate { get; set; }

        [Display(Name = "سنوات الخبرة")]

        public int? WorkExperience { get; set; }

        [Display(Name = "صورة الطبيب")]
        //[Required (ErrorMessage ="هذا الحقل اجباري")]
        public byte[] DoctorImg { get; set; }

        [Display(Name = "القسم")]

        public int? DepartmentNum { get; set; }
        [Display(Name = "كلفة حجز الموعد")]

        public double? reversedCost { set; get; }
        [Display(Name = "الوصف")]
        public string? Descreption { get; set; }


        public ICollection<DoctorSpecialization> doctorSpecializations { get; set; }
        

        public virtual Department DepartmentNumNavigation { get; set; }
        public virtual Location clincLocationNavigation { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
