namespace ALWADI.Models
{
    public partial class DoctorSpecialization
    {
        public int Id { get; set; }
        public int Did { get; set; }
        public int Cid { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Specialization Specialization { get; set; }


    }
}
