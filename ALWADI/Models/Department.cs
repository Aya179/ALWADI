using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ALWADI.Models
{
    public partial class Department
    {
        public Department()
        {
            Doctors = new HashSet<Doctor>();
            Specializations = new HashSet<Specialization>();
        }

        public int DepartmentId { get; set; }

        [Display(Name = "الاسم")]
        public string DepartmentName { get; set; }

        [Display(Name = "الوصف")]
        public string DepartmentDescription { get; set; }

        [Display(Name = "الصورة")]
        public byte[] DepartmentImg { get; set; }


        [Display(Name ="نوع العيادة")]
        public String DeptType { get; set; }
        [Display(Name = "ترتيب")]
        //[Remote(action: "CheckValue", controller: "Departments")]
        public int? arrangement { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }
    }
}
