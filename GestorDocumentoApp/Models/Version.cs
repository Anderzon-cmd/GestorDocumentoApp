
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GestorDocumentoApp.Models
{
    [Index(nameof(ChangeRequestId), IsUnique = true)]
    public class Version
    {

        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        public string ElementUrl { get; set; }

        public DateTime UploadDate { get; set; }


        public string State { get; set; }

        public string? ToolUrl { get; set; }

        public string VersionCode { get; set; }

        public int Phase { get; set; } = 1;

        public int iteration { get; set; } = 1;

        public int ChangeRequestId { get; set; }

        public ChangeRequest ChangeRequest { get; set; }


        // Cada versión pertenece a un elemento
        public int ElementId { get; set; }
        public Element Element { get; set; }

        // Cada versión está asociada a un usuario (quién la subió)
        public string UserId { get; set; }
        // public User User { get; set; } 
        public IdentityUser User { get; set; }
        // 🔗 Relación 1..* con RequirementType

        public int RequirementTypeId { get; set; }
        public RequirementType RequirementType { get; set; }

        // 🔗 Relación consigo mismo
        public int? ParentVersionId { get; set; }
        public Version? ParentVersion { get; set; }

    }
}
