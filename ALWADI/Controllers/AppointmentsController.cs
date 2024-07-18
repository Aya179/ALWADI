using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALWADI.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Abp.Web.Mvc.Alerts;

namespace ALWADI.Controllers
{
    public class AppointmentsController : Controller
    {
        
        private readonly AL_WADIContext _context;


        public AppointmentsController(AL_WADIContext context)
        {
            _context = context;
        }


        public  void checkapprovedAppointment()
        {
            var app= _context.Appointments.Where(a => a.StartTime < DateTime.Now && a.EndTime>DateTime.Now).ToList();
            
                foreach (var item in app)
                {
                item.IsApproved = true;

                if (ModelState.IsValid)
                {
                    item.IsApproved = true;




                    _context.Update(item);
                     _context.SaveChanges();

                }
            }
            
        }
        public void checkdoneAppointment()
        {
            var done = _context.Appointments.Where(a => a.EndTime < DateTime.Now).ToList();

            foreach (var item in done)
            {
                item.IsDone = true;

                if (ModelState.IsValid)
                {
                    item.IsDone = true;




                    _context.Update(item);
                    _context.SaveChanges();

                }
            }


        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {

            checkapprovedAppointment();
            checkdoneAppointment();


            var aL_WADIContext = _context.Appointments.Include(a => a.DoctorNavigation).Include(a => a.PatientNavigation).OrderByDescending(c=>c.Date);
            return View(await aL_WADIContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.DoctorNavigation)
                .Include(a => a.PatientNavigation)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["Doctor"] = new SelectList(_context.Doctors, "DoctorId", "DoctorName");
            ViewData["Patient"] = new SelectList(_context.Patients, "PatientId", "PatientName");
            return View();
        }

        public bool ValidateAppointment(DateTime ?appntDate, int? id)
        {
            return _context.Appointments.Any(a => a.StartTime == appntDate && a.Doctor == id);
        }


        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,StartTime,Patient,Doctor,Date,Note,IsApproved,IsDone,IsReversed,Rating")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                
               if(appointment.IsReversed==null)
                { appointment.IsReversed = false; }
                if (ValidateAppointment(appointment.StartTime, appointment.Doctor))
                {

                    TempData["AlertMessage"] = "هذا الموعد محجوز...!";
                    //ViewBag.alert = "this date is not valid";
                    // return Content("this date is not valid");
                    //return View("InvalidAppointment");
                    ViewData["Doctor"] = new SelectList(_context.Doctors, "DoctorId", "DoctorName", appointment.Doctor);
                    ViewData["Patient"] = new SelectList(_context.Patients, "PatientId", "PatientName", appointment.Patient);

                }
                else
                {
                    appointment.IsDone = false;
                    appointment.IsApproved = false;
                    appointment.EndTime = appointment.StartTime.Value.AddMinutes(30);
                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    var app = _context.Doctors.Find(appointment.Doctor);
                    Payment settings = new Payment();
                    var sett = _context.Settings.Find(11);
                    settings.centerCost = (float)(app.reversedCost * sett.centerCost);
                    settings.doctorCost = (float)(app.reversedCost * sett.doctorCost);
                    settings.webCost = (float)(app.reversedCost * sett.webCost);
                    settings.appoitmentId = appointment.AppointmentId;
                    settings.ispaid = false;
                    settings.PaymentDate = (DateTime)appointment.Date;
                    _context.Add(settings);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Doctor"] = new SelectList(_context.Doctors, "DoctorId", "DoctorName", appointment.Doctor);
            ViewData["Patient"] = new SelectList(_context.Patients, "PatientId", "PatientName", appointment.Patient);




            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["Doctor"] = new SelectList(_context.Doctors, "DoctorId", "DoctorName", appointment.Doctor);
            ViewData["Patient"] = new SelectList(_context.Patients, "PatientId", "PatientName", appointment.Patient);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,StartTime,EndTime,Patient,Doctor,Date,Note,IsApproved,IsDone,IsReversed")] Appointment appointment)
        {
            
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
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
            var docName = _context.Appointments.Where(appointment=>appointment.DoctorNavigation.DoctorId == appointment.Doctor).Select(appointment => appointment.DoctorNavigation.DoctorName);
            ViewBag.doctor_Name = docName;
            ViewData["Doctor"] = new SelectList(_context.Doctors, "DoctorId", "DoctorName", appointment.Doctor);
            ViewData["Patient"] = new SelectList(_context.Patients, "PatientId", "PatientName", appointment.Patient);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.DoctorNavigation)
                .Include(a => a.PatientNavigation)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }

        
    }
}
