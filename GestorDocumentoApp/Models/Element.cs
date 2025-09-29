using System.ComponentModel.DataAnnotations;

namespace GestorDocumentoApp.Models
{
    public class Element
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ElementTypeId { get; set; }
        public ElementType? ElementType { get; set; }
    }
}
