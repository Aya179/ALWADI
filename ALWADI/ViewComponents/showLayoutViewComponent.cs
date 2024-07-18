using ALWADI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALWADI.ViewComponents
{
    public class showLayoutViewComponent : ViewComponent
    {
        private readonly AL_WADIContext _context;
        public showLayoutViewComponent(AL_WADIContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           // ViewBag.count = _context.Departments.Count();

            var items = _context.Departments
                .ToList();
            
            
         
                return View("Default", items);

            
        }
       


    }
}
