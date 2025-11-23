using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDocumentoApp.ViewModels
{
    public class VersionVM
    {
        public int Id { get; set; }

        public string ElementName { get; set; }
        public int ElementId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La URL del elemento es requerida.")]
        [Display(Name = "Elemento URL")]
        public string ElementUrl { get; set; }


        [Required(ErrorMessage = "La fecha de subida es requerida.")]
        [Display(Name = "Fecha de subida")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El estado es requerido.")]
        public string State { get; set; }

        [Display(Name = "Herramienta URL")]
        public string? ToolUrl { get; set; }

        [Required(ErrorMessage = "El código de versión es requerido.")]
        [Display(Name = "Código de versión")]
        public string VersionCode { get; set; }

        [Required(ErrorMessage = "La fase es requerido.")]
        public int? Phase { get; set; } = 1;

        [Required(ErrorMessage = "Iteracion es requerido.")]
        public int? Iteration { get; set; } = 1;

        [Required(ErrorMessage ="La peticion de cambio es requerido.")]
        public int? ChangeRequestId { get; set; }


        [Required(ErrorMessage ="Tipo de requerimento es requerido.")]
        public int? RequirementTypeId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? RequirementTypes { get; set; }

        public int? ParentVersionId { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? PreviousVersions { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? Phases { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem?> ChangeRequests { get; set; }

    }

    public class RequirementTypeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        public string Name { get; set; }

        public string? Description
        {
            get; set;
        }
    }

    public static class PhaseHelper
    {
        public static string GetPhaseName(int valor) => valor switch
        {
            1 => "Planificación",
            2 => "Análisis",
            3 => "Diseño",
            4 => "Implementación",
            5 => "Pruebas",
            6 => "Mantenimiento",
            _ => "Desconocido"
        };
    }



}
