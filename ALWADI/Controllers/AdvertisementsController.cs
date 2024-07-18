using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALWADI.Models;
using System.IO;
using System.Numerics;

namespace ALWADI.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly AL_WADIContext _context;

        public AdvertisementsController(AL_WADIContext context)
        {
            _context = context;
        }

        // GET: Advertisements
        public async Task<IActionResult> Index()
        {
            var aL_WADIContext = _context.Advertisements.Include(a => a.categoryNavigation).Include(a => a.clientNavigation);
            return View(await aL_WADIContext.ToListAsync());
        }

        // GET: Advertisements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .Include(a => a.categoryNavigation)
                .Include(a => a.clientNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (advertisement.image != null)
            {

                string imageBase64Data =
    Convert.ToBase64String(advertisement.image);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }

            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // GET: Advertisements/Create
        public IActionResult Create()
        {
            ViewData["Ads_category_id"] = new SelectList(_context.Categories, "id", "category_name");
            ViewData["client_id"] = new SelectList(_context.Clients, "clientId", "clientname");
            return View();
        }

        // POST: Advertisements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,image,URL,client_id,Ads_StartDate,Ads_cost,Ads_place,Ads_EndDate,is_deleted,Ads_profit,Ads_category_id")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                var cat = _context.Categories.Where(c => c.id == advertisement.Ads_category_id).FirstOrDefault();
                advertisement.Ads_EndDate = advertisement.Ads_StartDate.AddDays(cat.duration);
                foreach (var file in Request.Form.Files)
                {


                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    advertisement.image = ms.ToArray();

                    ms.Close();
                    ms.Dispose();

                    

                }


                _context.Add(advertisement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            ViewData["Ads_category_id"] = new SelectList(_context.Categories, "id", "category_name", advertisement.Ads_category_id);
            ViewData["client_id"] = new SelectList(_context.Clients, "clientId", "clientname", advertisement.client_id);
            return View(advertisement);
        }

        // GET: Advertisements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            ViewData["Ads_category_id"] = new SelectList(_context.Categories, "id", "category_name", advertisement.Ads_category_id);
            ViewData["client_id"] = new SelectList(_context.Clients, "clientId", "clientname", advertisement.client_id);
            return View(advertisement);
        }

        // POST: Advertisements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,image,URL,client_id,Ads_StartDate,Ads_cost,Ads_place,Ads_EndDate,is_deleted,Ads_profit,Ads_category_id")] Advertisement advertisement)
        {
            if (id != advertisement.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertisement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementExists(advertisement.id))
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
            ViewData["Ads_category_id"] = new SelectList(_context.Categories, "id", "category_name", advertisement.Ads_category_id);
            ViewData["client_id"] = new SelectList(_context.Clients, "clientId", "clientname", advertisement.client_id);
            return View(advertisement);
        }

        // GET: Advertisements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .Include(a => a.categoryNavigation)
                .Include(a => a.clientNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // POST: Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementExists(int id)
        {
            return _context.Advertisements.Any(e => e.id == id);
        }
        public void bager()
        {
            ViewBag.count = _context.Clients.Count();
            ViewBag.count1 = _context.Advertisements.Count();
            ViewBag.count2 = _context.Advertisements.Where(adv=>adv.Ads_EndDate>=DateTime.Today).Count();//عدد الإعلانات النشطة
            ViewBag.count3 = _context.Appointments.Count();//مجموع الأرباح


        }
        

        public IActionResult Statistics()
        {
            bager();

            
            var query = (from Advertisement in _context.Advertisements
                         join client in _context.Clients on Advertisement.client_id equals client.clientId
                         join Category in _context.Categories on Advertisement.Ads_category_id equals Category.id
                         where Advertisement.Ads_EndDate>=DateTime.Today
                         select new
                         advStatistics
                         {
                            
                             client = client.clientname,
                             Ads_StartDate = Advertisement.Ads_StartDate,
                             duration = Category.duration,
                             profits = 0,
                             Ads_endDate=Advertisement.Ads_EndDate,
                             
                             image=Advertisement.image
                         });
            var res = query.ToList();

            foreach (var i in res)
            {
                if (i.image != null)
                {
                    byte[] buffer = i.image;
                    i.ImageToShow = Convert.ToBase64String(buffer);

                }
            }
            return View(res);

        }

    }
}
