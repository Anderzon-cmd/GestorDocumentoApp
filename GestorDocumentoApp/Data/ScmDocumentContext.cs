using GestorDocumentoApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestorDocumentoApp.Data
{
    public class ScmDocumentContext : IdentityDbContext<IdentityUser>
    {
        public ScmDocumentContext(DbContextOptions<ScmDocumentContext> options) : base(options)
        {
        }

        public ScmDocumentContext()
        {

        }

        public DbSet<ElementType> ElementTypes { get; set; }
        public DbSet<Element> Elements { get; set; }

        
    }
}
