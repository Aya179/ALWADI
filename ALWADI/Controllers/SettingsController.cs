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
using Syncfusion.EJ2.FileManager.Base;
using Syncfusion.EJ2.Inputs;

namespace ALWADI.Controllers
{
    public class SettingsController : Controller
    {
        private readonly AL_WADIContext _context;
        private readonly IWebHostEnvironment _env;


        public SettingsController(AL_WADIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Settings
        public async Task<IActionResult> Index()
        {

            

            return View(await _context.Settings.ToListAsync());
        }

        public async Task<IActionResult> addfixedimg()
        {



            return View();
        }
         public async Task<IActionResult> addfixedimg1(IFormFile img1,IFormFile img2,IFormFile img3,IFormFile img4)
        {
            if (img1 != null)
            {
                var folder = "FixedImg";
                var fileName = "أشعة.png";
                var uploadsFolderPath = Path.Combine(_env.WebRootPath, folder);
                
                var uploadsFolderPath1 = Path.Combine(_env.WebRootPath, folder, fileName);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

              //  string filePathdes = Path.Combine(uploadsFolderPath, img1.FileName);
                if (System.IO.File.Exists(uploadsFolderPath1))
                {
                    System.IO.File.Delete(uploadsFolderPath1);
                }
                string file_name = "أشعة.png";


                using (var imageFile = new FileStream(Path.Combine(uploadsFolderPath, file_name), FileMode.Create))
                {

                    img1.CopyTo(imageFile);
                }
            }
            if (img2 != null)
            {
                var folder = "FixedImg";
                var fileName = "تجميل.png";
                var uploadsFolderPath = Path.Combine(_env.WebRootPath, folder);
                var uploadsFolderPath1 = Path.Combine(_env.WebRootPath, folder,fileName);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

               // string filePathdes = Path.Combine(uploadsFolderPath, img2.FileName);
                if (System.IO.File.Exists(uploadsFolderPath1))
                {
                    System.IO.File.Delete(uploadsFolderPath1);
                }
                string file_name = "تجميل.png";


                using (var imageFile = new FileStream(Path.Combine(uploadsFolderPath, file_name), FileMode.Create))
                {

                    img2.CopyTo(imageFile);
                }
            }
            if (img3 != null)
            {
                var folder = "FixedImg";
                var fileName = "صيدلة.png";
                var uploadsFolderPath = Path.Combine(_env.WebRootPath, folder);
                var uploadsFolderPath1 = Path.Combine(_env.WebRootPath, folder,fileName);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                //string filePathdes = Path.Combine(uploadsFolderPath, img3.FileName);
                if (System.IO.File.Exists(uploadsFolderPath1))
                {
                    System.IO.File.Delete(uploadsFolderPath1);
                }
                string file_name = "صيدلة.png";


                using (var imageFile = new FileStream(Path.Combine(uploadsFolderPath, file_name), FileMode.Create))
                {

                    img3.CopyTo(imageFile);
                }
            }
            if (img4 != null)
            {
                var folder = "FixedImg";
                var fileName = "عيادات.png";
                var uploadsFolderPath = Path.Combine(_env.WebRootPath, folder);
                var uploadsFolderPath1 = Path.Combine(_env.WebRootPath, folder,fileName);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

               // string filePathdes = Path.Combine(uploadsFolderPath, img4.FileName);
                if (System.IO.File.Exists(uploadsFolderPath1))
                {
                    System.IO.File.Delete(uploadsFolderPath1);
                }
                string file_name = "عيادات.png";


                using (var imageFile = new FileStream(Path.Combine(uploadsFolderPath, file_name), FileMode.Create))
                {

                    img4.CopyTo(imageFile);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Settings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        // GET: Settings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,centerCost,webCost,doctorCost")] Settings settings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(settings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(settings);
        }

        // GET: Settings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings.FindAsync(id);
            if (settings == null)
            {
                return NotFound();
            }
            return View(settings);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,centerCost,webCost,doctorCost")] Settings settings)
        {
            if (id != settings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(settings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingsExists(settings.Id))
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
            return View(settings);
        }

        // GET: Settings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var settings = await _context.Settings.FindAsync(id);
            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> paymentShow()
        {
            var query = (from m in _context.Payments

                             // where m.PaymentDate.Date == DateTime.Now.Date
                         join appo in _context.Appointments on m.appoitmentId equals appo.AppointmentId
                         join Doctor in _context.Doctors on appo.Doctor equals Doctor.DoctorId
                         join Patient in _context.Patients on appo.Patient equals Patient.PatientId

                         select new
                         AppointmentGet
                         {
                             payId = m.Id,
                             // appId = m.appoitmentId,
                             centerCost = m.centerCost,
                             doctorCost = m.doctorCost,
                             webCost = m.webCost,
                             PaymentDate = m.PaymentDate,
                             PatientName = Patient.PatientName,
                             DoctorName=Doctor.DoctorName,
                             ispaid=m.ispaid

                             //paidedFor = Doctor.DoctorName


                         }) ;



            var res = query.ToList();
            ViewBag.count3 = query.Count();

            return View(res);



            // return View(await _context.Payments.ToListAsync());
        }

        private bool SettingsExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
