using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Invoice.Data;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceProcess.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly MvcInvoiceContext _context;

        public InvoiceController(MvcInvoiceContext context)
        {
            _context = context;
        }

        // GET: ّInvoice
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.InvoiceModel.ToListAsync());
        }

        // GET: ّInvoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceModel = await _context.InvoiceModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceModel == null)
            {
                return NotFound();
            }

            return View(invoiceModel);
        }

        // GET: ّInvoice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ّInvoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceDate,InvoiceBuyer,TotalAmount")] InvoiceModel invoiceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoiceModel);
        }

        // GET: ّInvoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceModel = await _context.InvoiceModel.FindAsync(id);
            if (invoiceModel == null)
            {
                return NotFound();
            }
            return View(invoiceModel);
        }

        // POST: ّInvoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceDate,InvoiceBuyer,TotalAmount")] InvoiceModel invoiceModel)
        {
            if (id != invoiceModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceModelExists(invoiceModel.Id))
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
            return View(invoiceModel);
        }

        // GET: ّInvoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceModel = await _context.InvoiceModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceModel == null)
            {
                return NotFound();
            }

            return View(invoiceModel);
        }

        // POST: ّInvoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceModel = await _context.InvoiceModel.FindAsync(id);
            if (invoiceModel != null)
            {
                _context.InvoiceModel.Remove(invoiceModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceModelExists(int id)
        {
            return _context.InvoiceModel.Any(e => e.Id == id);
        }
    }
}
