using System.ComponentModel.DataAnnotations;

namespace GestorDocumentoApp.Models
{
    public class ElementType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
