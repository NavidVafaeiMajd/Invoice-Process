using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice.Data;

namespace InvoiceProcess.Services
{
    public interface IInvoiceWorkflowService
    {
        Task Execute(int id, InvoiceAction action);
    }

    public class InvoiceWorkflowService : IInvoiceWorkflowService
    {
        private readonly MvcInvoiceContext _context;

        public InvoiceWorkflowService(MvcInvoiceContext context)
        {
            _context = context;
        }

        public record WorkflowTransition(InvoiceStatus From, InvoiceAction Action, InvoiceStatus To);

        private readonly List<WorkflowTransition> _transitions = new()
        {
            new(InvoiceStatus.Draft, InvoiceAction.Submit, InvoiceStatus.PendingApproval),
            new(InvoiceStatus.PendingApproval, InvoiceAction.Approve, InvoiceStatus.Approved),
            new(InvoiceStatus.PendingApproval, InvoiceAction.Reject, InvoiceStatus.Draft),
            new(InvoiceStatus.Approved, InvoiceAction.Deliver, InvoiceStatus.Delivered),
        };

        public async Task Execute(int invoiceId, InvoiceAction action)
        {
            var invoice = await _context.InvoiceModel.FindAsync(invoiceId);

            if (invoice is null)
                throw new Exception("Invoice not found");

            var currentStatus = invoice.Status;
            var transition = _transitions.FirstOrDefault(x =>
                x.From == currentStatus && x.Action == action
            );
            if (transition is null)
                throw new Exception("Transition Not Allowed");

            invoice.Status = transition.To;

            // Optionally record history if the model exists


            await _context.SaveChangesAsync();
        }
    }
}
