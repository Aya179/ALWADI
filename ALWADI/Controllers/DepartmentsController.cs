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
using System.Numerics;

namespace ALWADI.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AL_WADIContext _context;

        public DepartmentsController(AL_WADIContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var aL_WADIContext = _context.Departments;

        //    foreach (var i in aL_WADIContext)
        //    {
        //        if (i.DepartmentImg != null)
        //        {

        //            string imageBase64Data =
        //Convert.ToBase64String(i.DepartmentImg);
        //            string imageDataURL =
        //        string.Format("data:image/jpg;base64,{0}",
        //        imageBase64Data);
        //            //ViewBag.ImageTitle = img.ImageTitle;
        //            ViewBag.ImageDataUrl = imageDataURL;
        //        }
            //}

            return View(await aL_WADIContext.OrderBy(d=>d.arrangement).ToListAsync() );
        }
        public IActionResult AllDepartment()
        {
            var dep = _context.Departments.OrderBy(d => d.arrangement).ToList();
           



            return View(dep);
        }

       
        public IActionResult CheckValue(int value)
        {
            // Use a database query to check if the value already exists in the database
            var existingValue = _context.Departments.FirstOrDefault(x => x.arrangement == value);

            if (existingValue == null)
            {
                
                return Json("notfound");
            }
            else
            {
               
                return Json("found");
            }
        }


        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department.DepartmentImg != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(department.DepartmentImg);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,DepartmentName,DeptType,DepartmentDescription,DepartmentImg,arrangement")] Department department)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in Request.Form.Files)
                {

                    //  img.ImageTitle = file.FileName;

                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    department.DepartmentImg = ms.ToArray();

                    ms.Close();
                    ms.Dispose();


                    
                }
              
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllDepartment));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewBag.arrang = department.arrangement;
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,DepartmentName,DeptType,DepartmentDescription,arrangement")] Department department, IFormFile DepartmentImg)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                        _context.Update(department);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        
                        var existingdepartment = _context.Departments.Find(id);
                        existingdepartment.DepartmentName=department.DepartmentName;
                        existingdepartment.DepartmentDescription=department.DepartmentDescription;
                        existingdepartment.DeptType=department.DeptType;
                        existingdepartment.arrangement=department.arrangement;
                        _context.Update(existingdepartment);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllDepartment));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);

            if (department.DepartmentImg != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(department.DepartmentImg);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllDepartment));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
