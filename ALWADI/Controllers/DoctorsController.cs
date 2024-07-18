using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALWADI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ALWADI.Controllers
{
    // [Authorize]
    public class DoctorsController : Controller
    {
        private readonly AL_WADIContext _context;

        public DoctorsController(AL_WADIContext context)
        {
            _context = context;
        }
        public IActionResult EditImg(int id, [FromForm] IFormFile Img)
        {
            var doctor = _context.Doctors.Where(i => i.DoctorId == id).FirstOrDefault();
            //doctor.DoctorImg = Img;
            if (Img != null)
            {
                foreach (var file in Request.Form.Files)
                {


                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    doctor.DoctorImg = ms.ToArray();

                    ms.Close();
                    ms.Dispose();



                }
                _context.Update(doctor);
                _context.SaveChangesAsync();
            }
            return Json(new { doctor = doctor });

        }


        public JsonResult Getspec(int depId)
        {
            var listspec = _context.Specializations.Where(x => x.DepNum == depId).ToList();

            return Json(listspec);
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var aL_WADIContext = _context.Doctors.Include(d => d.DepartmentNumNavigation).Include(l => l.clincLocationNavigation);

            return View(await aL_WADIContext.ToListAsync());
        }


        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.avg = getDoctorAppoitments(id);

            var doctor = await _context.Doctors
                .Include(d => d.DepartmentNumNavigation)
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            var doctorspec = _context.DoctorSpecializations.Where(s => s.Did == id).Include(s => s.Specialization);
            ViewBag.sp = doctorspec;
            if (doctor.DoctorImg != null)
            {

                string imageBase64Data =
    Convert.ToBase64String(doctor.DoctorImg);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }

           


            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["DepartmentNum"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");

            ViewData["DoctorSpecialization"] = new SelectList(_context.Specializations, "SpecializationId", "SpecializationName");

            ViewData["location"] = new SelectList(_context.Locations, "Id", "location");

            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,DoctorName,DoctorPhone,DoctorEmail,DoctorAddrress,DoctorGender,DoctorSpecialization,CostPerPatient,DoctorCertificate,WorkExperience,DoctorImg,DepartmentNum,clincLocation,reversedCost,Descreption")] Doctor doctor, List<int> spId)
        {
            if (ModelState.IsValid)
            {


                if (doctor != null)
                {
                    foreach (var file in Request.Form.Files)
                    {


                        MemoryStream ms = new MemoryStream();
                        file.CopyTo(ms);
                        doctor.DoctorImg = ms.ToArray();

                        ms.Close();
                        ms.Dispose();



                    }
                }
                doctor.DoctorSpecialization = "-";
                //var speclization = _context.Specializations.Find(Convert.ToInt32(doctor.DoctorSpecialization));
                //doctor.DoctorSpecialization = speclization.SpecializationName;
                // doctor.reversedCost = 0;
                if (doctor.Descreption == null)
                {
                    doctor.Descreption = "-";
                }
                _context.Add(doctor);
                await _context.SaveChangesAsync();

                foreach (var item in spId)
                {
                    DoctorSpecialization doctorSpecialization = new DoctorSpecialization()
                    {
                        Cid = item,
                        Did = doctor.DoctorId,
                        Specialization = _context.Specializations.Where(x => x.SpecializationId == item).FirstOrDefault(),
                        Doctor = doctor
                    };
                    _context.DoctorSpecializations.Add(doctorSpecialization);
                    _context.SaveChanges();
                }
               // _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentNum"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", doctor.DepartmentNum);
            ViewData["location"] = new SelectList(_context.Locations, "Id", "location", doctor.clincLocation);

            return View(doctor);
        }


        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor =  _context.Doctors.Where(i=>i.DoctorId==id).FirstOrDefault(); 

            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["location"] = new SelectList(_context.Locations, "Id", "location",doctor.clincLocation );

            ViewData["DepartmentNum"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", doctor.DepartmentNum);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorId,DoctorName,DoctorPhone,DoctorEmail,DoctorAddrress,DoctorGender,DoctorSpecialization,CostPerPatient,DoctorCertificate,WorkExperience,DepartmentNum,clincLocation,reversedCost,Descreption")] Doctor doctor, IFormFile DoctorImg)
        {
            if (id != doctor.DoctorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (DoctorImg != null)
                    {
                        foreach (var file in Request.Form.Files)
                        {


                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                           doctor.DoctorImg = ms.ToArray();

                            ms.Close();
                            ms.Dispose();



                        }
                        _context.Update(doctor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var existingDoctor = _context.Doctors.Find(id);
                       // doctor.DoctorImg=existingDoctor.DoctorImg;
                        existingDoctor.clincLocation = doctor.clincLocation;
                        existingDoctor.DoctorGender = doctor.DoctorGender;
                        existingDoctor.DoctorCertificate = doctor.DoctorCertificate;
                        existingDoctor.DoctorPhone = doctor.DoctorPhone;
                        existingDoctor.DoctorEmail = doctor.DoctorEmail;
                        existingDoctor.DepartmentNum = doctor.DepartmentNum;
                        existingDoctor.DoctorAddrress = doctor.DoctorAddrress;
                       // existingDoctor.DoctorImg = doctor.DoctorImg;
                        existingDoctor.DoctorName = doctor.DoctorName;
                        existingDoctor.DoctorSpecialization = doctor.DoctorSpecialization;
                        existingDoctor.reversedCost = doctor.reversedCost;
                        existingDoctor.WorkExperience = doctor.WorkExperience;
                        existingDoctor.CostPerPatient = doctor.CostPerPatient;
                        existingDoctor.Descreption=doctor.Descreption;
                        _context.Update(existingDoctor);
                        await _context.SaveChangesAsync();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.DoctorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentNum"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", doctor.DepartmentNum);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.DepartmentNumNavigation)
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            var ds = _context.DoctorSpecializations.Where(s => s.Did == id);
            foreach(var d in ds)
            {
                _context.DoctorSpecializations.Remove(d);
               // _context.SaveChanges();

            }
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
        public float getDoctorAppoitments(int? id)
        {

            var sum = 0;
            var appointments = _context.Appointments.Where(app => app.Doctor == id).ToList();
            foreach (var item in appointments)
            {
                sum += item.Rating;


            }
            if (appointments.Count != 0)
            { return sum / appointments.Count; }
            else { return 0; }

        }

        public async Task<IActionResult> DoctorProfile(string name, string phone)
        {


            var selectedDoctor = _context.Doctors.Where(d => d.DoctorName == name && d.DoctorPhone == phone).FirstOrDefault();


       
            if (name == null || phone == null)
                return Content("يرجى إدخال الاسم والرقم");
            else if (selectedDoctor != null && name != null && phone != null)
            {
                ViewBag.name = selectedDoctor.DoctorName;
                var query = (from m in _context.Appointments
                             join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                             join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                             join adrs in _context.Patients on m.Patient equals adrs.PatientId
                             where dm.DoctorName == selectedDoctor.DoctorName
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

                return View(res);






               // return View();
            }

            else return Content("not found");



        }

        public IActionResult DoctorLogin()
        {

            return View();

        }

        public ActionResult Search(string keywords, DateTime date)
        {

            var query = (from m in _context.Appointments
                         join dm in _context.Doctors on m.Doctor equals dm.DoctorId
                         join dist in _context.Departments on dm.DepartmentNum equals dist.DepartmentId
                         join adrs in _context.Patients on m.Patient equals adrs.PatientId
                         where dm.DoctorName == keywords && m.Date >= date
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
           var doctorName = _context.Doctors.Where(f => f.DoctorName == keywords).FirstOrDefault();
            ViewBag .phone= doctorName.DoctorPhone;
            ViewBag.doctorName = doctorName.DoctorName;
            ViewBag.count = query.Count();


            ViewBag.keywords = doctorName.DoctorName;
            return View(res);


        }

            }
}
