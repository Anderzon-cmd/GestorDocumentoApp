using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorDocumentoApp.Models
{
    public class RequirementType
    {
        
        public int Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

      
      
    }
}
