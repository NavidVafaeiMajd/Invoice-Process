using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
 
namespace Invoice.Data
{
    public class MvcInvoiceContext : IdentityDbContext
    {
        public MvcInvoiceContext (DbContextOptions<MvcInvoiceContext> options)
            : base(options)
        {
        }

        public DbSet<InvoiceModel> InvoiceModel { get; set; } = default!;
    }
}
