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
    public class client_pamentController : Controller
    {
        private readonly AL_WADIContext _context;

        public client_pamentController(AL_WADIContext context)
        {
            _context = context;
        }

        // GET: client_pament
        public async Task<IActionResult> Index()
        {
            var aL_WADIContext = _context.Client_Paments.Include(c => c.clientNavigation);
            return View(await aL_WADIContext.ToListAsync());
        }

        // GET: client_pament/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client_pament = await _context.Client_Paments
                .Include(c => c.clientNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client_pament == null)
            {
                return NotFound();
            }

            return View(client_pament);
        }

        // GET: client_pament/Create
        public IActionResult Create()
        {
            ViewData["Client_Id"] = new SelectList(_context.Clients, "clientId", "clientname");
            return View();
        }

        // POST: client_pament/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Payment_Num,Payment_value,payment_date,payment_details,Client_Id")] client_pament client_pament)
        {
            if (ModelState.IsValid)
            {
                client_pament.payment_date = DateTime.Today.Date;
                _context.Add(client_pament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Client_Id"] = new SelectList(_context.Clients, "clientId", "clientname", client_pament.Client_Id);
            return View(client_pament);
        }

        // GET: client_pament/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client_pament = await _context.Client_Paments.FindAsync(id);
            if (client_pament == null)
            {
                return NotFound();
            }
            ViewData["Client_Id"] = new SelectList(_context.Clients, "clientId", "clientId", client_pament.Client_Id);
            return View(client_pament);
        }

        // POST: client_pament/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Payment_Num,Payment_value,payment_date,payment_details,Client_Id")] client_pament client_pament)
        {
            if (id != client_pament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client_pament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!client_pamentExists(client_pament.Id))
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
            ViewData["Client_Id"] = new SelectList(_context.Clients, "clientId", "clientId", client_pament.Client_Id);
            return View(client_pament);
        }

        // GET: client_pament/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client_pament = await _context.Client_Paments
                .Include(c => c.clientNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client_pament == null)
            {
                return NotFound();
            }

            return View(client_pament);
        }

        // POST: client_pament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client_pament = await _context.Client_Paments.FindAsync(id);
            _context.Client_Paments.Remove(client_pament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool client_pamentExists(int id)
        {
            return _context.Client_Paments.Any(e => e.Id == id);
        }
    }
}
