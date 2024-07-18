using System;
using System.Collections.Generic;

#nullable disable

namespace ALWADI.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string ActivationCode { get; set; }
        public DateTime? ActivationDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
