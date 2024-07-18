using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALWADI.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Syncfusion.EJ2.Linq;

namespace ALWADI.Views
{
    public class SpecializationsController : Controller
    {
        private readonly AL_WADIContext _context;

        public SpecializationsController(AL_WADIContext context)
        {
            _context = context;
        }

        // GET: Specializations
        public IActionResult Index()
        {
            var aL_WADIContext = _context.Specializations.Include(s => s.DepNumNavigation);
            return View( aL_WADIContext.ToList());
        }

        // GET: Specializations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specializations
                .Include(s => s.DepNumNavigation)
                .FirstOrDefaultAsync(m => m.SpecializationId == id);

            if (specialization.Specializationmg != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(specialization.Specializationmg);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // GET: Specializations/Create
        public IActionResult Create()
        {
            ViewData["DepNum"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Specializations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecializationId,SpecializationName,SpecializationDescription,Specializationmg,DepNum,Cost,SessionDuration")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {

                foreach (var file in Request.Form.Files)
                {

                    //  img.ImageTitle = file.FileName;

                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    specialization.Specializationmg = ms.ToArray();

                    ms.Close();
                    ms.Dispose();




                }
                _context.Add(specialization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepNum"] = new SelectList(_context.Departments,"DepartmentId","DepartmentName", specialization.DepNum);
            return View(specialization);
        }

        // GET: Specializations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }
            ViewData["DepNum"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", specialization.DepNum);
            return View(specialization);
        }

        // POST: Specializations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecializationId,SpecializationName,SpecializationDescription,DepNum,Cost,SessionDuration")] Specialization specialization, IFormFile Specializationmg)
        {
            if (id != specialization.SpecializationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                        _context.Update(specialization);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {

                        var existingspec = _context.Specializations.Find(id);
                       existingspec.SessionDuration=specialization.SessionDuration;
                        existingspec.Cost=specialization.Cost;
                        existingspec.DepNum=specialization.DepNum;
                        existingspec.SpecializationDescription=specialization.SpecializationDescription;    
                        existingspec.SpecializationName=specialization.SpecializationName;
                       
                        _context.Update(existingspec);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecializationExists(specialization.SpecializationId))
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
            ViewData["DepNum"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", specialization.DepNum);
            return View(specialization);
        }

        // GET: Specializations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specializations
                .Include(s => s.DepNumNavigation)
                .FirstOrDefaultAsync(m => m.SpecializationId == id);
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // POST: Specializations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            _context.Specializations.Remove(specialization);
            var ds = _context.DoctorSpecializations.Where(s => s.Cid == id);
            foreach (var d in ds)
            {
                _context.DoctorSpecializations.Remove(d);
                // _context.SaveChanges();

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.SpecializationId == id);
        }
    }
}
