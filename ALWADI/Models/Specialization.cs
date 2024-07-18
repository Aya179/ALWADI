using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ALWADI.Models
{
    public partial class Specialization
    {
        public int SpecializationId { get; set; }

        [Display(Name = "اسم التخصص")]
        public string SpecializationName { get; set; }

        [Display(Name = "الوصف")]
        public string SpecializationDescription { get; set; }

        [Display(Name = "الصورة")]
        public byte[] Specializationmg { get; set; }

        [Display(Name = "القسم")]
        public int? DepNum { get; set; }

        [Display(Name = "الكلفة")]
        public int? Cost { get; set; }

        [Display(Name = "المدة")]
        public int? SessionDuration { get; set; }
       

        public virtual Department DepNumNavigation { get; set; }
        public ICollection<DoctorSpecialization> doctorSpecializations { get; set; }

    }
}
