using GestorDocumentoApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GestorDocumentoApp.ViewModels
{
    public class ChangeRequestVM
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Priority { get; set; }
        public string Clasification { get; set; }
        public StatusCR Status { get; set; }
        public ActionCR Action { get; set; }
        public string Code { get; set; }

        public DateTime CreatedAt { get; set; }


    }

    public class ChangeRequestIndexVM:PagedList<ChangeRequestVM>
    {
       
        public string? ElementName { get; set; }
        public int? ElementId { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public IEnumerable<SelectListItem> Elements { get; set; }
    }
    public class ChangeRequestCreateByElementVM
    {

        public int ElementId { get; set; }
        public string? ElementName { get; set; } 

        [Required]
        public ClasificationTypeCR ClasificationType { get; set; }

        public string? Description { get; set; }

        [Required]
        public PriorityCR Priority { get; set; }

        [Required]
        public StatusCR Status { get; set; }

        public string? Remarks { get; set; }

        public string Code { get; set; }

        public ActionCR? Action { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? ClasificationOptions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? PriorityOptions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? StatusOptions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? ActionOptions { get; set; }


    }
    public class ChangeRequestCreateVM
    {
        [Required(ErrorMessage="Clasificacion es requerido.")]
        public ClasificationTypeCR? ClasificationType { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Prioridad es requerido.")]
        public PriorityCR? Priority { get; set; }

        [Required(ErrorMessage = "Proceso es requerido.")]
        public StatusCR? Status { get; set; }

        public string? Remarks { get; set; }

        [Required(ErrorMessage = "Codigo es requerido.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Estado es requerido.")]
        public ActionCR? Action { get; set; }

        [Required(ErrorMessage ="Elemento es requerido.")]
        public int? ElementId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? ClasificationOptions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? PriorityOptions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? StatusOptions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? ActionOptions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Elements { get; set; }

    }
}
