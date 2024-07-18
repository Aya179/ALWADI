using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ALWADI.Models;

#nullable disable

namespace ALWADI.Models
{
    public partial class AL_WADIContext : DbContext
    {
        public AL_WADIContext()
        {
        }

        public AL_WADIContext(DbContextOptions<AL_WADIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Point>  Points { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<client_pament> Client_Paments { get; set; }
        public virtual DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=PC\\SQLEXPRESS;Initial Catalog=AL_WADI;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");


            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.NotificationAddress)
                .HasMaxLength(250)

                .HasColumnName("NotificationAddress");

                entity.Property(e => e.NotificationText)
                .HasMaxLength(250)
                .HasColumnName("NotificationText");
                entity.Property(e => e.NotificationDate).HasColumnType("datetime");



            });


            modelBuilder.Entity<Settings>(entity =>
            {
                entity.ToTable("Settings");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.centerCost).HasColumnType("float");

                entity.Property(e => e.doctorCost).HasColumnType("float");
                entity.Property(e => e.webCost).HasColumnType("float");

                //entity.HasOne(d => d.paymentNavigation)
                //   .WithMany(p => p.Settings)
                //   .HasForeignKey(d => d.payment)
                //   .HasConstraintName("FK_Settings_Payment");

            });


            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.ispaid).HasColumnName("ispaid");

                entity.Property(e => e.paidedFor).HasColumnName("paidedFor");
                entity.Property(e => e.PaymentDate).HasColumnName("PaymentDate");
                entity.Property(e => e.centerCost).HasColumnType("float");

                entity.Property(e => e.doctorCost).HasColumnType("float");
                entity.Property(e => e.webCost).HasColumnType("float");
                //entity.HasOne(d => d.appoitmentIdNavigation)
                //    .WithMany(p => p.Payments)
                //    .HasForeignKey(d => d.appoitmentId)
                //    .HasConstraintName("FK_Payment_Appointment");


               

            });




            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Note)
                    .HasMaxLength(50)
                    .HasColumnName("note");
                entity.Property(e => e.Rating).HasColumnName("Rating");


                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.DoctorNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.Doctor)
                    .HasConstraintName("FK_Appointment_Doctor");

                entity.HasOne(d => d.PatientNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.Patient)
                    .HasConstraintName("FK_Appointment_Patient");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentDescription).HasMaxLength(2000);

                entity.Property(e => e.DepartmentImg).HasColumnType("image");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.DeptType).HasMaxLength(50);
                entity.Property(e => e.arrangement).HasColumnName("arrangement");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctor");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.DepartmentNum).HasColumnName("Department_Num");

                entity.Property(e => e.DoctorAddrress).HasMaxLength(100);
                entity.Property(e => e.clincLocation).HasColumnName("clincLocation");

                entity.Property(e => e.DoctorCertificate).HasMaxLength(100);

                entity.Property(e => e.DoctorEmail).HasMaxLength(50);

                entity.Property(e => e.DoctorGender)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.DoctorImg).HasColumnType("image");

                entity.Property(e => e.DoctorName).HasMaxLength(100);

                entity.Property(e => e.DoctorPhone).HasMaxLength(50);

                //entity.Property(e => e.DoctorSpecialization).HasMaxLength(50);
                entity.Property(e => e.DoctorSpecialization).HasMaxLength(50);
                entity.Property(e => e.Descreption).HasMaxLength(250);
                entity.Property(e => e.reversedCost).HasColumnName("reversedCost"); 

                entity.HasOne(d => d.DepartmentNumNavigation)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.DepartmentNum)
                    .HasConstraintName("FK_Doctor_Department");

                entity.HasOne(d => d.clincLocationNavigation)
                   .WithMany(p => p.Doctors)
                   .HasForeignKey(d => d.clincLocation)
                   .HasConstraintName("FK_Doctor_Location");


            });


       //     modelBuilder.Entity<DoctorSpecialization>()
       //.HasKey(bc => new { bc.Did, bc.Cid });
       //     modelBuilder.Entity<DoctorSpecialization>()
       //         .HasOne(bc => bc.Doctor)
       //         .WithMany(b => b.doctorSpecializations)
       //         .HasForeignKey(bc => bc.Did)
       //         .HasConstraintName("FK_DoctorSpecialization_Doctor"); ;
       //     modelBuilder.Entity<DoctorSpecialization>()
       //         .HasOne(bc => bc.Specialization)
       //         .WithMany(c => c.doctorSpecializations)
       //         .HasForeignKey(bc => bc.Cid)
       //         .HasConstraintName("FK_DoctorSpecialization_Specialization"); ;

            modelBuilder.Entity<DoctorSpecialization>(entity =>
            {
                entity.ToTable("DoctorSpecialization");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Cid).HasColumnName("Cid");

                entity.Property(e => e.Did).HasColumnName("Did");
                entity.HasOne(bc => bc.Specialization)
                .WithMany(c => c.doctorSpecializations)
                .HasForeignKey(bc => bc.Cid)
                .HasConstraintName("FK_DoctorSpecialization_Specialization");


                entity.HasOne(bc => bc.Doctor)
                .WithMany(b => b.doctorSpecializations)
                .HasForeignKey(bc => bc.Did)
                .HasConstraintName("FK_DoctorSpecialization_Doctor");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.ActivationCode).HasMaxLength(50);

                entity.Property(e => e.ActivationDate).HasColumnType("datetime");

                entity.Property(e => e.PatientAddress).HasMaxLength(100);

                entity.Property(e => e.PatientGender)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.PatientImg).HasColumnType("image");

                entity.Property(e => e.PatientLocation).HasMaxLength(50);

                entity.Property(e => e.PatientName).HasMaxLength(100);

                entity.Property(e => e.PatientPassword).HasMaxLength(50);

                entity.Property(e => e.PatientPhone).HasMaxLength(50);
                entity.Property(e => e.Token).HasMaxLength(300);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
        });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.ToTable("Specialization");

                entity.Property(e => e.SpecializationId).HasColumnName("SpecializationID");

                entity.Property(e => e.DepNum).HasColumnName("Dep_NUM");
               

                entity.Property(e => e.SpecializationDescription).HasMaxLength(200);

                entity.Property(e => e.SpecializationName).HasMaxLength(50);

                entity.Property(e => e.Specializationmg).HasColumnType("image");

                entity.HasOne(d => d.DepNumNavigation)
                    .WithMany(p => p.Specializations)
                    .HasForeignKey(d => d.DepNum)
                    .HasConstraintName("FK_Specialization_Department");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ActivationCode).HasMaxLength(50);

                entity.Property(e => e.ActivationDate).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserPassword).HasMaxLength(50);
            });



            modelBuilder.Entity<Advertisement>(entity =>
            {
                entity.ToTable("Advertisement");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.URL).HasMaxLength(100);

                entity.Property(e => e.image).HasColumnType("image");

                entity.Property(e => e.Ads_cost).HasColumnName("Ads_cost");
                entity.Property(e => e.Ads_EndDate).HasColumnName("Ads_EndDate");

                entity.Property(e => e.Ads_place).HasColumnName("Ads_place");
                entity.Property(e => e.Ads_profit).HasColumnName("Ads_profit");
                entity.Property(e => e.Ads_StartDate).HasColumnType("datetime");
                entity.Property(e => e.is_deleted).HasColumnName("is_deleted");

                entity.HasOne(d => d.clientNavigation)
                   .WithMany(p => p.Advertisements)
                   .HasForeignKey(d => d.client_id)
                   .HasConstraintName("FK_Advertisement_Client");

                entity.HasOne(d => d.categoryNavigation)
                  .WithMany(p => p.Advertisements)
                  .HasForeignKey(d => d.Ads_category_id)
                  .HasConstraintName("FK_Advertisement_Category");



            });
            modelBuilder.Entity <Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.location).HasMaxLength(50);
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("Point");

                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.is_deleted).HasColumnName("is_deleted");
                entity.Property(e => e.value).HasColumnName("value");

                entity.HasOne(d => d.advertismentNavigation)
                  .WithMany(p => p.Points)
                  .HasForeignKey(d => d.ads_id)
                  .HasConstraintName("FK_Point_Advertisement");


            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.is_deleted).HasColumnName("is_deleted");
                entity.Property(e => e.category_name).HasColumnName("category_name");
                entity.Property(e => e.categoy_cost).HasColumnName("categoy_cost");
                entity.Property(e => e.duration).HasColumnName("duration");
                entity.Property(e => e.pruch_cost).HasColumnName("pruch_cost");







            });


            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.clientId).HasColumnName("clientId");
                entity.Property(e => e.address).HasColumnName("address");
                entity.Property(e => e.clientname).HasColumnName("clientname");
                entity.Property(e => e.client_profit).HasColumnName("client_profit");
                entity.Property(e => e.company).HasColumnName("company");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.Is_deleted).HasColumnName("Is_deleted");
                entity.Property(e => e.phone).HasColumnName("phone");










            });


            modelBuilder.Entity<client_pament>(entity =>
            {
                entity.ToTable("client_pament");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.payment_date).HasColumnType("datetime");
                entity.Property(e => e.payment_details).HasColumnName("payment_details");
                entity.Property(e => e.Payment_Num).HasColumnName("Payment_Num");
                entity.Property(e => e.Payment_value).HasColumnName("Payment_value");

                entity.HasOne(d => d.clientNavigation)
                   .WithMany(p => p.Client_Paments)
                   .HasForeignKey(d => d.Client_Id)
                   .HasConstraintName("FK_client_pament_Client");








            });



            OnModelCreatingPartial(modelBuilder);
          
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<ALWADI.Models.AppointmentGet> test { get; set; }
    }
}
