using ALWADI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.Controllers
{
    public class AppointmentGetController : Controller
    {
        private readonly AL_WADIContext _context;

        public AppointmentGetController(AL_WADIContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            ViewData["Doctor"] = new SelectList(_context.Doctors, "DoctorId", "DoctorName");

            
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         //where dm.DoctorName == keywords
                         select new
                         AppointmentGet {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = dist.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date=m.Date,

                         });



            var res = query.ToList();
            
            return View(res);
            
        }
        //[HttpPost]
        public ActionResult Search(int keywords, DateTime date)
        {
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         where dm.DoctorId == keywords && m.Date>=date
                         select new
                         AppointmentGet
                         {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = dist.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,

                         });



            var res = query.ToList();
            var doctorName = _context.Doctors.Where(f => f.DoctorId == keywords).FirstOrDefault().DoctorName;
            ViewBag.count = query.Count();


            ViewBag.keywords =doctorName ;
            return View(res);


        }

        public async Task<IActionResult> location()
        {
            // ViewData["Doctor"] = new SelectList(_context.Doctors, "DoctorId", "DoctorName");
            ViewData["location"] = new SelectList(_context.Locations, "Id", "location");


            var query = (from Doctor in _context.Doctors
                         join Department in _context.Departments on Doctor.DepartmentNum equals Department.DepartmentId
                         join Location in _context.Locations on Doctor.clincLocation equals Location.Id
                         select new
                         AppointmentGet
                         {
                            
                             DepartmentName = Department.DepartmentName,
                             DoctorName =Doctor.DoctorName,
                             location = Location.location,

                         });



            var res = query.ToList();

            return View(res);

        }
        public async Task<IActionResult> locationSearch(int keywords)
        {


            var query = (from Doctor in _context.Doctors
                         join Department in _context.Departments on Doctor.DepartmentNum equals Department.DepartmentId
                         join Location in _context.Locations on Doctor.clincLocation equals Location.Id

                         where Doctor.clincLocation== keywords
                         select new
                         AppointmentGet
                         {

                             DepartmentName = Department.DepartmentName,
                             DoctorName = Doctor.DoctorName,
                             location=Location.location,

                         });



            var res = query.ToList();

            return View(res);

        }


        public async Task<IActionResult> DailyAppointment()
        {


            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         where m.Date.Value.Date==DateTime.Now.Date&&m.IsReversed==true
                         select new
                         AppointmentGet
                         {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = dist.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,

                         });



            var res = query.ToList();
            ViewBag.count = query.Count();

            return View(res);

        }
        public void bagger()
        {
            float count = 0;
            float count1 = 0;
            float count2 = 0;
            Payment[] payments = _context.Payments.Where(p => p.PaymentDate.Date == DateTime.Now.Date).ToArray();
            for (int item = 0; item < payments.Length; item++)
            {
                count += payments[item].centerCost;
                count1 += payments[item].doctorCost;
                count2 += payments[item].webCost;
            }
            ViewBag.count = count;
            ViewBag.count1 = count1;
            ViewBag.count2 = count2;
        }

        public async Task<IActionResult> DailyPayment()
        {
            // bagger();


            float count = 0;
            float count1 = 0;
            float count2 = 0;
            Payment[] payments = _context.Payments.Where(p => p.PaymentDate.Date == DateTime.Now.Date).ToArray();
            for (int item = 0; item < payments.Length; item++)
            {
                count += payments[item].centerCost;
                count1 += payments[item].doctorCost;
                count2 += payments[item].webCost;
            }
            ViewBag.count = count;
            ViewBag.count1 = count1;
            ViewBag.count2 = count2;

            var query = (from m in _context.Payments
                        
                         where m.PaymentDate.Date == DateTime.Now.Date
                         join appo in _context.Appointments on m.appoitmentId equals appo.AppointmentId
                         join Doctor in _context.Doctors on appo.Doctor equals Doctor.DoctorId

                         select new
                         AppointmentGet
                         {
                             payId= m.Id,
                            // appId = m.appoitmentId,
                             centerCost = m.centerCost,
                             doctorCost = m.doctorCost,
                             webCost = m.webCost,
                             Date=m.PaymentDate,
                             paidedFor=Doctor.DoctorName


                         });



            var res = query.ToList();
            ViewBag.count3 = query.Count();

            return View(res);

        }





        public async Task<IActionResult> selectedDateReportIndex()
        {
            DateTime dt3 = new DateTime(2015, 12, 31, 5, 10, 20);
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         where m.Date == dt3 && m.IsReversed == true
                         select new
                         AppointmentGet
                         {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = dist.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,

                         });



            var res = query.ToList();
            ViewBag.count = query.Count();

            return View(res);

        }

        public async Task<IActionResult> selectedDateReport(DateTime start,DateTime end)
        {
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         where m.Date.Value.Date >=start.Date&&m.Date<=end.Date && m.IsReversed == true
                         select new
                         AppointmentGet
                         {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = dist.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,

                         });



            var res = query.ToList();
            ViewBag.count = query.Count();

            return View(res);

        }



        public async Task<IActionResult> specilizationReportIndex()
        {


            ViewData["specilization"] = new SelectList(_context.Specializations, "SpecializationId", "SpecializationName");
            DateTime dt3 = new DateTime(2015, 12, 31, 5, 10, 20);
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join Department in _context.Departments on dm.DepartmentNum equals Department.DepartmentId
                         join Specialization in _context.Specializations on Department.DepartmentId equals Specialization.DepNum
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         where m.Date == dt3 && m.IsReversed == true
                         select new
                         AppointmentGet
                         {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = Department.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,
                             specilizationName=Specialization.SpecializationName

                         });



            var res = query.ToList();
           // ViewBag.count = query.Count();

            return View(res);

        }

        public async Task<IActionResult> specilizationReport(DateTime start, DateTime end,int specilization)
        {
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join Department in _context.Departments on dm.DepartmentNum equals Department.DepartmentId
                         join Specialization in _context.Specializations on Department.DepartmentId equals Specialization.DepNum
                         join Patient in _context.Patients on m.Patient equals Patient.PatientId
                         where m.Date.Value.Date >= start.Date && m.Date <= end.Date && m.IsReversed == true&&Specialization.SpecializationId==specilization
                         select new
                         AppointmentGet
                         {
                             Id = Patient.PatientId,
                             PatientName = Patient.PatientName,
                           //  DepartmentName = Department.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,
                             specilizationName = Specialization.SpecializationName

                         });



            var res = query.ToList();

            var specName = _context.Specializations.Where(f => f.SpecializationId == specilization).FirstOrDefault().SpecializationName;
            ViewBag.spec = specName;

            return View(res);

        }





        public async Task<IActionResult> departmentReportIndex()
        {


            ViewData["department"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            DateTime dt3 = new DateTime(2015, 12, 31, 5, 10, 20);
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join Department in _context.Departments on dm.DepartmentNum equals Department.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         where m.Date == dt3 && m.IsReversed == true
                         select new
                         AppointmentGet
                         {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = Department.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,
                            // specilizationName = Specialization.SpecializationName

                         });



            var res = query.ToList();
            // ViewBag.count = query.Count();

            return View(res);

        }

        public async Task<IActionResult> departmentReport(DateTime start, DateTime end, int department)
        {
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join Department in _context.Departments on dm.DepartmentNum equals Department.DepartmentId
                         join Patient in _context.Patients on m.Patient equals Patient.PatientId
                         where m.Date.Value.Date >= start.Date && m.Date <= end.Date && m.IsReversed == true && Department.DepartmentId == department
                         select new
                         AppointmentGet
                         {
                             Id = Patient.PatientId,
                             PatientName = Patient.PatientName,
                              DepartmentName = Department.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,
                            // specilizationName = Specialization.SpecializationName

                         });



            var res = query.ToList();

            var depName = _context.Departments.Where(f => f.DepartmentId == department).FirstOrDefault().DepartmentName;
            ViewBag.dep = depName;

            return View(res);

        }



    }
}
//SQL query::
//Select pat.PatientName, doct.DoctorName, dep.DepartmentName from  Doctor doct, Appointment appo, Department dep, Patient pat
//where appo.Doctor= doct.DoctorID and
//dep.DepartmentID= doct.Department_Num and
//pat.PatientID= appo.Patient;