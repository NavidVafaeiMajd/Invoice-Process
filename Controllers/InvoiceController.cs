using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice.Data;
using InvoiceProcess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcess.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly MvcInvoiceContext _context;
        private readonly IInvoiceWorkflowService _workflow;

        public InvoiceController(MvcInvoiceContext context, IInvoiceWorkflowService workflow)
        {
            _context = context;
            _workflow = workflow;
        }

        // GET: ّInvoice
        [Authorize(Roles = "Employee,Manager")]
        public async Task<IActionResult> Index()
        {
            var invoices = _context.InvoiceModel.AsQueryable();

            if (User.IsInRole("Manager"))
            {
                invoices = invoices.Where(i => i.Status != InvoiceStatus.Draft);
            }

            return View(await invoices.ToListAsync());
        }

        // GET: ّInvoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceModel = await _context.InvoiceModel.FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceModel == null)
            {
                return NotFound();
            }

            return View(invoiceModel);
        }

        // GET: ّInvoice/Create
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ّInvoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,InvoiceDate,InvoiceBuyer,TotalAmount")] InvoiceModel invoiceModel
        )
        {
            if (ModelState.IsValid)
            {
                var invoice = new InvoiceModel
                {
                    InvoiceDate = invoiceModel.InvoiceDate,
                    InvoiceBuyer = invoiceModel.InvoiceBuyer,
                    TotalAmount = invoiceModel.TotalAmount,
                    Status = InvoiceStatus.Draft,
                };
                Console.WriteLine(invoice);
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoiceModel);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _workflow.Execute(id, InvoiceAction.Approve);

            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> SendForApproval(int id)
        {
            await _workflow.Execute(id, InvoiceAction.Submit);

            return RedirectToAction("Details", new { id });
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
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,InvoiceDate,InvoiceBuyer,TotalAmount")] InvoiceModel invoiceModel
        )
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

            var invoiceModel = await _context.InvoiceModel.FirstOrDefaultAsync(m => m.Id == id);
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
