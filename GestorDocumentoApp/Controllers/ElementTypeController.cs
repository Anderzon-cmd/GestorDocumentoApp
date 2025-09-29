using GestorDocumentoApp.Data;
using GestorDocumentoApp.Models;
using GestorDocumentoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorDocumentoApp.Controllers
{
    public class ElementTypeController : Controller
    {
        private readonly ScmDocumentContext _scmDocumentContext;
        private readonly ILogger<ElementTypeController> _logger;

        public ElementTypeController(ScmDocumentContext scmDocumentContext, ILogger<ElementTypeController> logger)
        {
            _scmDocumentContext = scmDocumentContext;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            //var elementTypes = new List<ElementType>
            //{
            //    new ElementType{ Id=1,Name="Manual",Description="Manual de usuarios"},
            //    new ElementType { Id=2,Name="Procedimientos"},
            //    new ElementType {Id=3,Name="Plan"},
            //    new ElementType {Id=4,Name="Instructivo" },
            //    new ElementType {Id=5,Name="Políticas" },
            //    new ElementType { Id = 6,Name="Reporte" },
            //    new ElementType { Id=7,Name="Guía" }
            //};
            var elementTypes = await _scmDocumentContext.ElementTypes.OrderBy(elementType => elementType.Name).ToListAsync();
            return View(elementTypes);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ElementTypeVM elementTypeVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(elementTypeVM);
                }
                var elementType = new ElementType() { Name = elementTypeVM.Name, Description = elementTypeVM.Description };
                _scmDocumentContext.Add(elementType);
                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Element Type {ElementName}", elementTypeVM.Name);

                return RedirectToAction(nameof(Index));
            }

        }


        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var elementType = await _scmDocumentContext.ElementTypes.FindAsync(id);

            if (elementType is null)
            {
                return NotFound();
            }

            return View(new ElementTypeVM { Id = elementType.Id, Name = elementType.Name, Description = elementType.Description });

        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, ElementTypeVM elementTypeVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(elementTypeVM);
                }

                var elementType = await _scmDocumentContext.ElementTypes.FindAsync(id);
                
                if(elementType is null)
                {
                    return NotFound();
                }

                elementType.Name = elementTypeVM.Name;
                elementType.Description = elementTypeVM.Description;

                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating Element Type {ElementName}", elementTypeVM.Name);
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var elementType = await _scmDocumentContext.ElementTypes.FindAsync(id);

                if(elementType is null)
                {
                    return NotFound();
                }
                _scmDocumentContext.ElementTypes.Remove(elementType);
                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error delete Element Type {ElementId}", id);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
