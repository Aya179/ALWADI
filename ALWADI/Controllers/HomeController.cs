using ALWADI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syncfusion.EJ2.Navigations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ALWADI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AL_WADIContext _context;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public HomeController(AL_WADIContext context)
        {
            _context = context;
        }
        [Authorize]

        public IActionResult Index()
        {
            bager();
            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                       //  join pay in _context.Payments on m.AppointmentId equals pay.appoitmentId
                        // where m.Date.Value.Date == DateTime.Today.Date && m.IsReversed == true
                         where m.Date.Value.Date == DateTime.UtcNow.Date &&m.StartTime.Value.Hour>=DateTime.Now.Hour&&m.StartTime.Value.Minute>=DateTime.Now.Minute&& m.IsReversed == true
                         select new
                         AppointmentGet
                         {
                             Id = adrs.PatientId,
                             PatientName = adrs.PatientName,
                             DepartmentName = dist.DepartmentName,
                             DoctorName = dm.DoctorName,
                             Date = m.Date,
                             phone = adrs.PatientPhone,
                             StartTime=m.StartTime,
                             appId=m.AppointmentId,
                             IsDone=m.IsDone,
                            // ispaid=pay.ispaid
                         }) ;
            var res = query.ToList();
            return View(res);


          //  return View();
        }



        public IActionResult landing()
        {
           

              return View();
        }
        public IActionResult landing1()
        {
            var dep = _context.Departments.ToList();
          //  var dep1 = _context.Departments.Where(d => d.DepartmentId == 12).FirstOrDefault();
             // string imageBase64Data =
//Convert.ToBase64String(dep1.DepartmentImg);
//            string imageDataURL =
//        string.Format("data:image/jpg;base64,{0}",
//        imageBase64Data);
//            //ViewBag.ImageTitle = img.ImageTitle;
//            ViewBag.ImageDataUrl = imageDataURL;



            return View(dep);
        }
        public IActionResult getImg(int Did)
        {
            var dep = _context.Departments.Where(d=>d.DepartmentId==Did).FirstOrDefault();

            string imageBase64Data =
Convert.ToBase64String(dep.DepartmentImg);
            string imageDataURL =
        string.Format("data:image/jpg;base64,{0}",
        imageBase64Data);
            //ViewBag.ImageTitle = img.ImageTitle;
           // ViewBag.ImageDataUrl = imageDataURL;
        


            return Json(imageDataURL);
        }
        public bool findAppoByPat(int patientID, DateTime HourAppoint, DateTime date)
        {
            return _context.Appointments.Any(ap => ap.Patient == patientID && ap.Date == date && ap.StartTime.Value.Hour == HourAppoint.Hour && ap.StartTime.Value.Minute == HourAppoint.Minute);

        }



       // https://alwadi-mc.sy/Home/getAppoitment?Pname=aya&Paddress=homs&Page=20&Pgender=ذكر&Pphone=0900000&DoctorId=36&startTime=10:30:00&date=2022/12/08
        public IActionResult getAppoitment(string Pname,string Paddress,int Page ,string Pgender,string Pphone,int DoctorId, DateTime startTime, DateTime date)

        {
            var app = _context.Appointments.Where(a => a.IsReversed == true && a.Doctor == DoctorId && a.Date == date && a.StartTime.Value.Hour == startTime.Hour && a.StartTime.Value.Minute == startTime.Minute).FirstOrDefault();


            if (app == null)
            {
                var patient = _context.Patients.Where(p => p.PatientPhone == Pphone).FirstOrDefault();
                if (patient == null)
                {
                     patient = new Patient();
                    patient.PatientName = Pname;
                    patient.PatientLocation = Paddress;
                    patient.PatientAddress = Paddress;
                    patient.PatientPassword = Pphone;
                    patient.PatientAge = Page;
                    patient.PatientGender = Pgender;
                    patient.PatientPhone = Pphone;
                    _context.Patients.Add(patient);
                    _context.SaveChanges();
                }


                if (findAppoByPat(patient.PatientId, startTime, date))
                {
                    return Json("لديك موعد محجوز في هذا الوقت !الرجاء اختيار وقت آخر");
                }


                else if (startTime.Hour >= 9 && startTime.Hour <= 21)

                {
                    Appointment appointment = new Appointment();

                    appointment.IsReversed = true;
                    appointment.IsApproved = false;
                    appointment.StartTime = startTime;
                    appointment.EndTime = startTime.AddMinutes(30);
                    appointment.IsDone = false;
                    appointment.Patient = patient.PatientId;
                    appointment.Doctor = DoctorId;
                    appointment.Date = date;

                    _context.Add(appointment);
                    _context.SaveChanges();

                    return Json(patient.PatientName+"تم حجز الموعد للمريض ");
                }

                else return Json("هذا التوقيت خارج أوقات الدوام");



            }
            else
            {
                return Json ("هذا الموعد محجوز");
            }







           




            




        }
        public IActionResult getDoctorsBydep(int Depid)
        {
            var doctors = _context.Doctors.Where(d => d.DepartmentNum == Depid);

            


            return Json(doctors);
        }



        public IActionResult TOP5Doctors()
        {

            var topProductsIds = _context.Appointments// table with a row for each view of a product
         .Include(x=>x.DoctorNavigation)
          .Where(x => x.Date.Value.Month == DateTime.Now.Month&&x.Date.Value.Year==DateTime.Now.Year)
        .GroupBy(x => x.Doctor) //group all rows with same product id together
        .OrderByDescending(g => g.Count()) // move products with highest views to the top
        .Take(5) // take top 5
        .Select(x => x.Key) // get id of products
        .ToList(); // execute query and convert it to a list








            var topProducts = _context.Doctors// table with products information
                .Include(d=>d.DepartmentNumNavigation)

                .Where(x => topProductsIds.Contains(x.DoctorId));
            return Json(topProducts);





        }
        public IActionResult TOP5Patient()
        {

            var topProductsIds = _context.Appointments// table with a row for each view of a product
         .Include(x => x.PatientNavigation)
         .Where(x=>x.Date.Value.Month==DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year)
        .GroupBy(x => x.Patient) //group all rows with same product id together
        .OrderByDescending(g => g.Count()) // move products with highest views to the top
        .Take(5) // take top 5
        .Select(x => x.Key) // get id of products
        .ToList(); // execute query and convert it to a list








            var topProducts = _context.Patients// table with products information

                .Where(x => topProductsIds.Contains(x.PatientId));
            return Json(topProducts);





        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void bager()
        {
            ViewBag.count = _context.Departments.Count();
            ViewBag.count1 = _context.Specializations.Count();
            ViewBag.count2 = _context.Doctors.Count();
            ViewBag.count3 = _context.Appointments.Where(a=>a.IsDone==false).Count();


        }

        public async Task<IActionResult> Test(int ? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var aL_WADIContext = _context.Doctors.Where(d => d.DepartmentNum == id).Include(d => d.DepartmentNumNavigation).Include(l => l.clincLocationNavigation);
            var allDepDoctor = _context.Departments.Find(id);
            ViewBag.s = allDepDoctor.DepartmentName;
            return View(await aL_WADIContext.ToListAsync());

        }




        public async Task<IActionResult> updateIsDone(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var aL_WADIContext = _context.Appointments.Find(id);
            if (ModelState.IsValid)
            {
                aL_WADIContext.IsDone = true;




                _context.Update(aL_WADIContext);
                _context.SaveChanges();

            }
            return RedirectToAction(nameof(Index));

        }
        //public async Task<IActionResult> AddPayment(int? id)
        //{
        //  //  var isPaymentExist = _context.Payments.Where(d => d.appoitmentId == id&&d.ispaid==true).FirstOrDefault();

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var isPaymentExist = _context.Payments.Where(d => d.appoitmentId == id && d.ispaid == false).FirstOrDefault();

        //    if (ModelState.IsValid)
        //    {


        //        if (isPaymentExist !=null&&isPaymentExist.ispaid==false)
        //        {
        //            var pay = _context.Payments.Find(isPaymentExist.Id);
        //            //var appointment = _context.Appointments.Find(id);
        //            // var app = _context.Doctors.Find(appointment.Doctor);
        //            //Payment payment = new Payment();
        //            //var sett = _context.Settings.Find(11);
        //            //payment.centerCost = (float)(app.reversedCost * sett.centerCost);
        //            //payment.doctorCost = (float)(app.reversedCost * sett.doctorCost);
        //            //payment.webCost = (float)(app.reversedCost * sett.webCost);
        //            //payment.appoitmentId = appointment.AppointmentId;
        //            //payment.PaymentDate = (DateTime)appointment.Date;
        //            //_context.Add(payment);
        //            pay.ispaid = true;
        //            _context.Update(pay);
        //            _context.SaveChanges();
        //        }
        //        else { TempData["AlertMessage"] = "تمت عملية الدفع لهذا الموعد مسبقا...!"; }
                

        //        //  await _context.SaveChangesAsync();

        //    }
        //    return RedirectToAction(nameof(Index));

        //}




    }
}
