using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALWADI.Models;

namespace ALWADI.Controllers
{
    public class PointsController : Controller
    {
        private readonly AL_WADIContext _context;

        public PointsController(AL_WADIContext context)
        {
            _context = context;
        }

        // GET: Points
        public async Task<IActionResult> Index()
        {

            //var aL_WADIContext = _context.Advertisements.Include(c => c.clientNavigation);

            var aL_WADIContext = _context.Points.Include(p => p.advertismentNavigation).Include(c => c.advertismentNavigation.clientNavigation);
            foreach(var i in aL_WADIContext){
                if (i.advertismentNavigation.image != null)
                {

                    string imageBase64Data =
        Convert.ToBase64String(i.advertismentNavigation.image);
                    string imageDataURL =
                string.Format("data:image/jpg;base64,{0}",
                imageBase64Data);
                    //ViewBag.ImageTitle = img.ImageTitle;
                    ViewBag.ImageDataUrl = imageDataURL;
                } }
            return View(await aL_WADIContext.ToListAsync());
        }

        // GET: Points/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Points
                .Include(p => p.advertismentNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // GET: Points/Create
        public IActionResult Create()
        {
            ViewData["ads_id"] = new SelectList(_context.Advertisements, "id", "id");
            return View();
        }

        // POST: Points/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ads_id,is_deleted,value")] Point point)
        {
            if (ModelState.IsValid)
            {
                _context.Add(point);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ads_id"] = new SelectList(_context.Advertisements, "id", "id", point.ads_id);
            return View(point);
        }

        // GET: Points/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Points.FindAsync(id);
            if (point == null)
            {
                return NotFound();
            }
            ViewData["ads_id"] = new SelectList(_context.Advertisements, "id", "id", point.ads_id);
            return View(point);
        }

        // POST: Points/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ads_id,is_deleted,value")] Point point)
        {
            if (id != point.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(point);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PointExists(point.id))
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
            ViewData["ads_id"] = new SelectList(_context.Advertisements, "id", "id", point.ads_id);
            return View(point);
        }

        // GET: Points/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Points
                .Include(p => p.advertismentNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // POST: Points/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var point = await _context.Points.FindAsync(id);
            _context.Points.Remove(point);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PointExists(int id)
        {
            return _context.Points.Any(e => e.id == id);
        }
    }
}
