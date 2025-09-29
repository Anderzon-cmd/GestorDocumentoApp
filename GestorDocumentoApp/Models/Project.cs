using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace GestorDocumentoApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }

        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public List<Element> Elements { get; set; }

    }
}
