using Abp.Collections.Extensions;
using ALWADI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ALWADI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class MobilePController : ControllerBase
    {
        AL_WADIContext _context;
        private IConfiguration _config;
        public MobilePController(AL_WADIContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;

        }


        public string generatCode()
        {
            Random random = new Random();
            int i = random.Next(100000, 999999);
            return i.ToString();

        }


        [HttpGet("logIn")]
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/logIn?name=ayaNew123&password=1234
        public IActionResult logIn(string phone, string password)
        {
            Patient[] users = _context.Patients.Where(c => c.PatientPassword == password && c.PatientPhone ==phone ).ToArray();
            if (users.Length > 0)
                return Ok(new { users = users[0] });
            else
                return BadRequest("invalid phone or password");
        }
        [HttpGet("allPat")]
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/allPat
        public IActionResult getAllPatients()
        {
            Patient[] Patients = null;
            try
            {
                Patients = _context.Patients.ToArray();

                return Ok(new { Patients = Patients });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return Ok(new { Patients = "hello" });
            }



        }

        //https://alwadi-mc.sy/api/MobileP/CallSyApi?phone=0945351836
        //https://localhost:44318/api/MobileP/CallSyApi?phone=0945351836

        [HttpGet("CallSyApi")]

        public IActionResult CallSyApi(string phone)
        {

            try
            {

                //string html = string.Empty;
                //string url = @"https://bms.syriatel.sy/API/SendTemplateSMS.aspx?user_name=EMPEROR APP API&password=***&template_code=EMPEROR APP API _T1&param_list=000000&sender=EMPEROR APP&to=0933157238";

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.AutomaticDecompression = DecompressionMethods.GZip;

                //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //using (Stream stream = response.GetResponseStream())
                //using (StreamReader reader = new StreamReader(stream))
                //{
                //    html = reader.ReadToEnd();
                //}

               // Console.WriteLine(html);


               // return Ok(html);
                return Ok("000000");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }


        }




        [HttpGet("alldoctors")]
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/alldoctors
        public IActionResult alldoctors()
        {
            Doctor[] doctors1 = null;
            try
            {
             var   doctors = _context.Doctors.Include(d=>d.DepartmentNumNavigation).Select(c => new { c.DepartmentNumNavigation.DepartmentName, c.DepartmentNum, c.DoctorId, c.DoctorName, c.DoctorPhone, c.DoctorEmail, c.DoctorAddrress, c.clincLocation, c.DoctorGender, c.CostPerPatient, c.DoctorCertificate, c.WorkExperience, c.reversedCost, c.Descreption, c.DoctorImg }).ToArray();

                return Ok(new { doctors = doctors });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return Ok(new { doctors = "hello" });
            }



        }


        [HttpGet("alldoctors1")]
        ///api/MobileP/alldoctors
        public IActionResult  alldoctors1()
        {
            var doctors = _context.Doctors.Include(d => d.DepartmentNumNavigation).Select(c => new { c.DepartmentNumNavigation.DepartmentName, c.DepartmentNum, c.DoctorId, c.DoctorName, c.DoctorPhone, c.DoctorEmail, c.DoctorAddrress, c.clincLocation, c.DoctorGender, c.CostPerPatient, c.DoctorCertificate, c.WorkExperience, c.reversedCost, c.Descreption, c.DoctorImg }).ToArray();
            if (doctors.Length != 0)
            {
                return Ok(doctors);
            }
            else return Ok("empty");

        }




        [HttpPost("AddNewDoctor")]
        public IActionResult AddNewDoctor(string DoctorName,string DoctorPhone, string DoctorEmail,string DoctorAddrress,string DoctorGender,string DoctorSpecialization,int CostPerPatient,string DoctorCertificate,int WorkExperience, [FromForm] IFormFile DoctorImg,int DepartmentNum)

        {
            Doctor doctor = new Doctor();
            doctor.clincLocation = 1;
            doctor.reversedCost = 0;

            foreach (var file in Request.Form.Files)
            {


                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                doctor.DoctorImg = ms.ToArray();

                ms.Close();
                ms.Dispose();



            }

            var speclization = _context.Specializations.Find(Convert.ToInt32(DoctorSpecialization));
            doctor.DoctorSpecialization = speclization.SpecializationName;
            doctor.DoctorName=DoctorName;
            doctor.DoctorPhone=DoctorPhone;
            doctor.DoctorEmail=DoctorEmail;
            doctor.DoctorAddrress=DoctorAddrress;
            doctor.DoctorGender=DoctorGender;
            doctor.CostPerPatient=CostPerPatient;
            doctor.DoctorCertificate=DoctorCertificate;
            doctor.DepartmentNum=DepartmentNum;
            doctor.WorkExperience=WorkExperience;
            doctor = _context.Doctors.Add(doctor).Entity;
            _context.SaveChanges();

            return Ok(new { doctor = doctor });
        }


        [HttpPost("AddNewDepartment")]
        public IActionResult AddNewDepartment(string DepartmentName,string DepartmentDescription,[FromForm] IFormFile DepartmentImg)
        {
            Department department = new Department();

            department.DepartmentName = DepartmentName; 
            department.DepartmentDescription = DepartmentDescription;
           
            if (DepartmentImg != null)
            {
                foreach (var file in Request.Form.Files)
                {


                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    department.DepartmentImg = ms.ToArray();

                    ms.Close();
                    ms.Dispose();



                }
            }

            
            department = _context.Departments.Add(department).Entity;
            _context.SaveChanges();

            return Ok(new { department = department });
        }

        [HttpPost("AddNewSpecialization")]
        public IActionResult AddNewSpecialization(string SpecializationName, string SpecializationDescription,int DepNum, [FromForm] IFormFile Specializationmg)
        {
            Specialization specialization = new Specialization();

            specialization.Cost = 0;
            specialization.SessionDuration = 1;
            specialization.SpecializationName=SpecializationName;
            specialization.SpecializationDescription = SpecializationDescription;
            specialization.DepNum=DepNum;

            if (Specializationmg != null)
            {
                foreach (var file in Request.Form.Files)
                {


                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    specialization.Specializationmg = ms.ToArray();

                    ms.Close();
                    ms.Dispose();



                }
            }


            specialization = _context.Specializations.Add(specialization).Entity;
            _context.SaveChanges();

            return Ok(new { specialization = specialization });
        }





        [HttpPost("AddNewPatient")]
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/AddNewPatient?name=ayaNew123&phone=0000000000&age=100&gender=fmale&location=hoooooooooms&address=aldablaaaaaaaaaan&password=123
        //https://localhost:44318/api/MobileP/AddNewPatient?name=ayaNew123&phone=0000000000&age=20&gender=fmale&location=hoooooooooms&address=aldablaaaaaaaaaan&password=1234
        public IActionResult AddNewPatient([FromQuery]string name,[FromQuery] string phone,[FromQuery] int age,[FromQuery] string gender,[FromQuery] string location,[FromQuery] string address, [FromQuery] string password)
        {
            Patient patient = new Patient();
            patient.RegistrationDate = DateTime.Now;
            patient.PatientPhone = phone;



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddDays(1300),
              signingCredentials: creds);


            patient.PatientName = name;
            patient.PatientGender = gender;
            // patient.Token = new JwtSecurityTokenHandler().WriteToken(token);
            patient.Token = null;
            patient.IsActive = false;
            patient.IsDeleted = false;
            patient.PatientLocation = location;
            patient.PatientAddress = address;
            patient.ActivationDate = DateTime.Now;
            patient.ActivationCode = generatCode();
            patient.PatientPassword = password;
            patient.PatientAge = age;
            patient = _context.Patients.Add(patient).Entity;
            _context.SaveChanges();

            return Ok(new { patient = patient });
        }
        [HttpPost("updatePatient")]
        //https://localhost:44318/api/MobileP/updatePatient?id=1&PatientName=testtest&PatientAddress=homshoms&gender=fmalfmal&PatientPhone=093333333
        public async Task<IActionResult> updatePatient([FromQuery] int id, [FromQuery] string PatientName, [FromQuery] string PatientAddress, [FromQuery] string gender, [FromQuery] String PatientPhone)

        {
            try
            {
                Patient patient = await _context.Patients.FindAsync(id);
                System.Console.WriteLine(patient.PatientId);
                patient.PatientName = PatientName;
                patient.PatientGender = gender;
                patient.PatientAddress = PatientAddress;
                patient.PatientPhone = PatientPhone;

                patient.IsActive = true;

                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();

                return Ok(new { patient = patient });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return Ok(new { patient = "hello" });
            }
        }

       
        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
        [HttpGet("GetAllPatientAPPOi")]
        //https://localhost:44318/api/MobileP/GetAllPatientAPPOi?id=1
        public IActionResult GetAllPatientAPPOintment([FromQuery]int id)
        {
            
            var query = (from appo in _context.Appointments
                         join Patient in _context.Patients on appo.Patient equals Patient.PatientId
                         join Doctor in _context.Doctors on appo.Doctor equals Doctor.DoctorId
                         join Department in _context.Departments on Doctor.DepartmentNum equals Department.DepartmentId
                         join Specialization in _context.Specializations on Doctor.DoctorSpecialization equals Specialization.SpecializationName

                         where appo.Patient==id
                         select new
                         {
                             appointmentID=appo.AppointmentId,
                             endTime=appo.EndTime,
                             startTime=appo.StartTime,
                             AppointmentDate=appo.Date,
                             PatientID =appo.Patient,
                             DoctorID=appo.Doctor,
                             DoctorName=Doctor.DoctorName,
                             DoctorSpecialization=Specialization.SpecializationName,
                             note=appo.Note,
                             isApproved=appo.IsApproved,
                             isDone=appo.IsDone,
                             isReversed=appo.IsReversed,
                             rating=appo.Rating,
                             doctorNavigation=appo.DoctorNavigation,
                             patientNavigstion=appo.PatientNavigation


                         }
                         
                         );
            var res = query.ToArray();


            return Ok(new { appointments = res });
        }

        [HttpGet("allDep")]
        //https://localhost:44318/api/MobileP/allDep
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/allDep
        public IActionResult getAllDepartments()
        {
            Department[] departments = null;
            try
            {
                departments = _context.Departments.OrderBy(d => d.arrangement).ToArray();

                return Ok(new { departments = departments });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return Ok(new { departments = "hello" });
            }



        }


        [HttpGet("allSpec")]

        //https://alwadi-mc.sy/api/MobileP/allSpec
        public IActionResult getAllSpecializations()
        {
            Specialization[] Specializations = null;
            try
            {
                Specializations = _context.Specializations/*.OrderByDescending(c => c.arrangement)*/.Include(c=>c.DepNumNavigation).ToArray();

                return Ok(new { Specializations = Specializations });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return Ok(new { departments = "hello" });
            }



        }

        [HttpGet("GetDepartmentImg")]
        // https://localhost:44318/api/MobileP/GetDepartmentImg?id=7
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/GetDepartmentImg?id=25
        public IActionResult GetDepartmentImg([FromQuery] int id)
        {
            var departments = _context.Departments.Where(de=>de.DepartmentId==id).FirstOrDefault();
            if(departments.DepartmentImg==null)
            return Ok("not found");
            else { return Ok(departments.DepartmentImg); }
        }

        [HttpGet("GetDoctorInfo")]
        //https://localhost:44318/api/MobileP/Getpatient?id=1
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/Getpatient?id=11
        public async Task<ActionResult> GetDoctorInfo([FromQuery] int id)

        {
            if (id == null)
            {
                return NotFound();

            }

            var sub = from q in _context.DoctorSpecializations
                      //join q in _context.DoctorSpecializations on od.DoctorId equals q.Did
                      join pay in _context.Specializations on q.Cid equals pay.SpecializationId
                      
                      where q.Did == id
                      select new
                      {
                          
                          pay.SpecializationName
                      }; 
            //var sub = from od in _context.Doctors
                      //join q in _context.DoctorSpecializations on od.DoctorId equals q.Did
                      //join pay in _context.Specializations on q.Cid equals pay.SpecializationId
                      //where od.DoctorId == id
                      //select new
                      //{
                      //    od.DoctorId,
                      //    od.DoctorPhone,
                      //    od.clincLocation, 
                      //    od.CostPerPatient,
                      //    od.DepartmentNum,
                      //    od.Descreption,
                      //    od.DoctorAddrress,
                      //    od.DoctorCertificate,
                      //    od.DoctorEmail,
                      //    od.DoctorGender,
                      //    od.DoctorImg,
                      //    od.DoctorName,
                      //    od.reversedCost,
                      //    od.WorkExperience,
                      //    pay.SpecializationName
                      //};
            var doctor =  _context.Doctors.Where(d=>d.DoctorId==id).Include(d => d.DepartmentNumNavigation).Select(c=>new { c.DepartmentNumNavigation.DepartmentName,c.DepartmentNum,c.DoctorId,c.DoctorName,c.DoctorPhone,c.DoctorEmail,c.DoctorAddrress,c.clincLocation,c.DoctorGender,c.CostPerPatient,c.DoctorCertificate,c.WorkExperience,c.reversedCost,c.Descreption,c.DoctorImg}).FirstOrDefault();
            return Ok(new{ sub,doctor});
        }

        [HttpGet("Getpatient")]
        //https://localhost:44318/api/MobileP/Getpatient?id=1
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/Getpatient?id=11
        public async Task<ActionResult<Patient>> Getpatient([FromQuery] int id)

        {
            if (id == null)
            {
                return NotFound();

            }
            var patient = await _context.Patients.FindAsync(id);
            return patient;
        }

        [HttpGet("GetDepartmentSpecialization")]
       // https://localhost:44318/api/MobileP/GetDepartmentSpecialization?id=12
       //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/GetDepartmentSpecialization?id=25
        public IActionResult GetDepartmentSpecialization([FromQuery] int id)
        {
            Specialization[] specializations = _context.Specializations/*.OrderByDescending(c => c.arrangement)*/.Where(sp => sp.DepNum == id).ToArray();
            return Ok(new { specializations = specializations });
        }

        [HttpGet("GetSpecializationDoctor")]
        //http://ayaarnous-001-site1.ftempurl.com/api/MobileP/GetSpecializationDoctor?name=تقويم الأسنان والفكين
        public IActionResult GetSpecializationDoctor([FromQuery] int id)
        {


            var query = (from ds in _context.DoctorSpecializations
                         join s in _context.Specializations on ds.Cid equals s.SpecializationId
                         join Doctor in _context.Doctors on ds.Did equals Doctor.DoctorId
                         join Department in _context.Departments on Doctor.DepartmentNum equals Department.DepartmentId

                         where ds.Cid == id
                         select new
                         {
                             
                             Department=Department.DepartmentName,
                             DepartmentId=Department.DepartmentId,
                             SpecializationID=s.SpecializationId,
                             Specialization=s.SpecializationName,
                             doctorName = Doctor.DoctorName,
                             DoctorId = Doctor.DoctorId,
                             DoctorPhone=Doctor.DoctorPhone,
                             DoctorEmail=Doctor.DoctorEmail,
                             DoctorAddress=Doctor.DoctorAddrress,
                             DoctorCostperPatient=Doctor.CostPerPatient,
                             DoctorCertificate=Doctor.DoctorCertificate,
                             WorkExperience=Doctor.WorkExperience,
                             DoctorImg=Doctor.DoctorImg,
                             reversedCost=Doctor.reversedCost   ,
                             Descreption=Doctor.Descreption

                         }

                       );
            var res = query.ToArray();


            return Ok(new { result = res });

        }

        
        
      
        [HttpPost("GetAppoitment")]
        // https://localhost:44318/api/MobileP/GetAppoitment
        //https://localhost:44318/api/MobileP/GetAppoitment?DoctorId=32&AppoitmentstareDate=14:33:00&PatientId=11&endDate=15:33:00&date=9/4/2022
        public async Task<IActionResult> GetAppoitmentAsync(int DoctorId, DateTime HourAppoint, int PatientId,DateTime date)
        {
            //var patient = _context.Patients.Find(PatientId);
           
            var app = _context.Appointments.Where(a => a.IsReversed == true && a.Doctor == DoctorId && a.Date == date && a.StartTime.Value.Hour == HourAppoint.Hour && a.StartTime.Value.Minute == HourAppoint.Minute).FirstOrDefault();


            if (app == null)
            {

                if (findAppoByPat(PatientId, HourAppoint, date))
                {
                    return Ok("لديك موعد محجوز في هذا الوقت !الرجاء اختيار وقت آخر");
                }
               

              else   if (HourAppoint.Hour >= 9 && HourAppoint.Hour <= 21)

                {
                    Appointment appointment = new Appointment();

                    appointment.IsReversed = true;
                    appointment.IsApproved = false;
                    appointment.StartTime = HourAppoint;
                    appointment.EndTime = HourAppoint.AddMinutes(30);
                    appointment.IsDone = false;
                    appointment.Patient = PatientId;
                    appointment.Doctor = DoctorId;
                    appointment.Date = date;

                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    //

                    //var doc = _context.Doctors.Find(appointment.Doctor);
                    //Payment settings = new Payment();
                    //var sett = _context.Settings.Find(11);
                    //settings.centerCost = (float)(doc.reversedCost * sett.centerCost);
                    //settings.doctorCost = (float)(doc.reversedCost * sett.doctorCost);
                    //settings.webCost = (float)(doc.reversedCost * sett.webCost);
                    //settings.appoitmentId = appointment.AppointmentId;
                    //settings.ispaid = false;
                    //settings.PaymentDate = (DateTime)appointment.Date;
                    //_context.Add(settings);
                    //await _context.SaveChangesAsync();

                    //
                   // addpayment(appointment);

                    return Ok(new { Appointment = appointment });
                }
                
                else return Ok("هذا التوقيت خارج أوقات الدوام");
                

                
            }
            else
            {
                return Ok("هذا الموعد محجوز");
            } 
            


        }
        public void addpayment(Appointment appointment)
        {
            var doc = _context.Doctors.Find(appointment.Doctor);
            Payment settings = new Payment();
            var sett = _context.Settings.Find(11);
            settings.centerCost = (float)(doc.reversedCost * sett.centerCost);
            settings.doctorCost = (float)(doc.reversedCost * sett.doctorCost);
            settings.webCost = (float)(doc.reversedCost * sett.webCost);
            settings.appoitmentId = appointment.AppointmentId;
            settings.ispaid = false;
            settings.PaymentDate = (DateTime)appointment.Date;
            _context.Add(settings);
            _context.SaveChanges();
        }
        public bool findAppoByPat(int patientID, DateTime HourAppoint,DateTime date)
        {
            return _context.Appointments.Any(ap=>ap.Patient==patientID&& ap.Date == date && ap.StartTime.Value.Hour == HourAppoint.Hour && ap.StartTime.Value.Minute == HourAppoint.Minute);

        }


        [HttpGet("GetReversedAppointment")]
        public IActionResult GetReversedAppointment( int? id, DateTime date)
        {
            // var app=_context.Appointments.FindAsync()
            Appointment[] list = _context.Appointments.Where(a => a.IsReversed == true && a.Doctor == id && a.Date==date).ToArray();
            if (list.Length!=0)
            {
                return Ok(new { Appointment = list });
            }
            else return Ok("empty");

            // return _context.Appointments.Any(a => a.Doctor == id &&a.IsDone==false&& (((a.StartTime.Value.Hour == appntDate.Value.Hour&&a.StartTime.Value.Minute==appntDate.Value.Minute )||( a.EndTime.Value.Hour < appntDate.Value.Hour&& a.EndTime.Value.Minute < appntDate.Value.Minute))) );

        }

        [HttpGet("GetClinics")]
        public IActionResult getClinics()
        {
            Department[] depts = _context.Departments.OrderBy(d => d.arrangement).Where(c => c.DeptType == "عيادة").ToArray();
            return Ok(new { clinics = depts });
        }

        [HttpGet("GetBeauty")]
        public IActionResult getBeauty()
        {
            Department[] depts = _context.Departments.Where(c => c.DeptType == "تجميل").ToArray();
            return Ok(new { clinics = depts });
        }

        [HttpGet("GetAllAppointment")]
        //https://localhost:44318/api/MobileP/GetAllAppointment?DoctorId=37&date=2022/09/27
        public IActionResult GetAllAppointment(int doctorId, DateTime date)
        {
            List<Appointment> List = new List<Appointment>();



          

            var Appointmentlist = _context.Appointments.Where( a=>a.Date == date&&a.Doctor==doctorId).OrderBy(c=>c.StartTime.Value.TimeOfDay).ToArray();


          //Value.Date.ToShortDateString()
            var startTime = DateTime.Today.AddHours(8).AddMinutes(30);
            var endTime = DateTime.Today.AddHours(20);
            if (Appointmentlist.Length == 0)
            {
                // var startTime1 = DateTime.Today.AddHours(8).AddMinutes(30);
                //  var endTime1 = DateTime.Today.AddHours(21);
                while (startTime <= endTime)
                {
                    startTime = startTime.AddMinutes(30);
                    Appointment Empty = new Appointment();

                    Empty.StartTime = startTime;
                    Empty.IsReversed = false;
                    Empty.EndTime = startTime.AddMinutes(30);
                    Empty.Doctor = doctorId;
                    List.Add(Empty);
                }
                return Ok(List);
            }
            else
            {
                while (startTime <= endTime)
                {
                   // foreach (var item in Appointmentlist)
                    //{
                        startTime = startTime.AddMinutes(30);

                        var app=_context.Appointments.Where(a => a.Date == date && a.Doctor == doctorId&& a.StartTime.Value.Hour == startTime.Hour && a.StartTime.Value.Minute == startTime.Minute).FirstOrDefault();

                       // if (app.StartTime.Value.Hour == startTime.Hour && app.StartTime.Value.Minute == startTime.Minute)
                       // {
                       if(app!=null)
                            List.Add(app);
                            
                       // }
                        else
                        {


                            Appointment Empty = new Appointment();

                            Empty.StartTime = startTime;
                            Empty.IsReversed = false;
                            Empty.EndTime = startTime.AddMinutes(30);
                            Empty.Doctor = doctorId;
                            List.Add(Empty);

                            


                        }

                   // }







                }
                return Ok(List.ToArray());
            }

        }




        [HttpGet("GetAllAdvertismentImg")]
        //https://localhost:44318/api/MobileP/GetAllAdvertismentImg
        public IActionResult GetAllAdvertismentImg()
        {
            var advertisements = from s in _context.Advertisements
                                
                                 select new  { s.image,s.id};

            var adv = advertisements.ToArray();
            return Ok(new { advertisements = adv });
        }




        //  https://localhost:44318/api/MobileP/SearchInDoctors?search=سعد
        //    https://alwadi-mc.sy/api/MobileP/SearchInDoctors?search=سعد

        [HttpGet("SearchInDoctors")]
        public IActionResult SearchInDoctors(string search)
        {




          

            var data = from s in _context.Doctors
                       join Department in _context.Departments on s.DepartmentNum equals Department.DepartmentId
                       //join Doctorsp in _context.DoctorSpecializations on s.DoctorId equals Doctorsp.Did
                     //  join sp in _context.Specializations on Doctorsp.Cid equals sp.SpecializationId

                       where s.DoctorName.Contains(search)

                       select new { s.DoctorName, s.DoctorPhone, Department.DepartmentName/*, sp.SpecializationName*/, s.DoctorId };
            var data1 = data.AsEnumerable();

//            var result = data1.GroupBy(g => g.DoctorId)
//.Select(s => new
//{
//    Id = s.Key,
//    SpName = string.Join(",", s.Select(ss => ss.SpecializationName)),
//    Department=s.First().DepartmentName,
//    doctor=s.First().DoctorName,
//    Doctorphone=s.First().DoctorPhone,

//}).AsEnumerable();
           
          
            return Ok(new { list = data });
           
        }



        //  /api/MobileP/AddNewNotification?text=test&address=test
        //   https://alwadi-mc.sy/api/MobileP/AddNewNotification?text=test&address=test
        [HttpPost("AddNewNotification")]
        public IActionResult AddNewNotification(string text, string address)
        {
            Notification notification = new Notification();
            notification.NotificationDate = System.DateTime.Now;
            notification.NotificationText = text;
            notification.NotificationAddress = address;
            _context.Add(notification);
            _context.SaveChanges();
            return Ok(new { notification = notification });
        }
        //  /api/MobileP/GetNotification
        //     https://alwadi-mc.sy/api/MobileP/GetNotification
        [HttpGet("GetNotification")]
        public IActionResult GetNotification()
        {
            var list = _context.Notifications.OrderByDescending(n => n.NotificationDate).ToList();
            return Ok(new { list = list }); ;
        }





    }






    }

