using GestorDocumentoApp.Data;
using GestorDocumentoApp.Models;
using GestorDocumentoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestorDocumentoApp.Controllers
{
    public class ElementController : Controller
    {
        private ScmDocumentContext _scmDocumentContext;
        private ILogger<ElementController> _logger;

        public ElementController(ScmDocumentContext scmDocumentContext, ILogger<ElementController> logger)
        {
            _scmDocumentContext = scmDocumentContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            //var elements = new List<Element> { 
            //    new Element{Id=1,Name="PAPS",Description="Descripciont",CreatedDate=DateTime.Now,ElementType=new ElementType{Id=1,Name="Software Descripction"}},
            //    new Element{Id=2,Name="Manual de usuario",CreatedDate=DateTime.Now.AddDays(-3)}
            //};
            var elements = await _scmDocumentContext.Elements.OrderByDescending(element => element.CreatedDate).ToListAsync();
            return View(elements);
        }

        public async Task<IActionResult> Create()
        {
            var elementTypes = await _scmDocumentContext.ElementTypes.OrderBy(elementType=>elementType.Name).ToListAsync();

            return View(new ElementVM { ElementTypes=elementTypes.Select(elementType=>new SelectListItem { Text=elementType.Name,Value=elementType.Id.ToString()})});
        }

        [HttpPost]
        public async Task<IActionResult> Create(ElementVM elementVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var elementTypes=await _scmDocumentContext.ElementTypes.OrderBy(elementType => elementType.Name).ToListAsync();
                    elementVM.ElementTypes = elementTypes.Select(elementType => new SelectListItem { Text = elementType.Name, Value = elementType.Id.ToString() });
                    return View(elementVM);
                }
                var element = new Element { 
                    Name = elementVM.Name, 
                    Description = elementVM.Description,
                    CreatedDate = DateTime.SpecifyKind(elementVM.CreatedDate,DateTimeKind.Utc),
                    ElementTypeId= elementVM.ElementTypeId };

                _scmDocumentContext.Add(element);
                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Element Type {ElementName}", elementVM.Name);

                return RedirectToAction(nameof(Index));
            }

        }


        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var element = await _scmDocumentContext.Elements.FindAsync(id);
            
            if (element is null)
            {
                return NotFound();
            }

            var elementTypes = await _scmDocumentContext.ElementTypes.AsNoTracking().OrderBy(element => element.Name).ToListAsync();

            return View(
                new ElementVM {
                    Id = element.Id,
                    Name = element.Name, 
                    Description = element.Description,
                    CreatedDate= element.CreatedDate,
                    ElementTypeId=element.ElementTypeId,
                    ElementTypes=elementTypes.Select(elementType=>new SelectListItem { Text=elementType.Name,Value=elementType.Id.ToString()})}
                );

        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, ElementVM elementVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var elementTypes = await _scmDocumentContext.ElementTypes.OrderBy(elementType => elementType.Name).ToListAsync();
                    elementVM.ElementTypes = elementTypes.Select(elementType => new SelectListItem { Text = elementType.Name, Value = elementType.Id.ToString() });
                    return View(elementVM);
                }

                var element = await _scmDocumentContext.Elements.FindAsync(id);

                if (element is null)
                {
                    return NotFound();
                }

                element.Name = elementVM.Name;
                element.Description = elementVM.Description;
                element.CreatedDate = DateTime.SpecifyKind(elementVM.CreatedDate, DateTimeKind.Utc);
                element.ElementTypeId = elementVM.ElementTypeId;

                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Element {ElementName}", elementVM.Name);
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var element = await _scmDocumentContext.Elements.FindAsync(id);

                if (element is null)
                {
                    return NotFound();
                }
                _scmDocumentContext.Elements.Remove(element);
                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error delete Element {ElementId}", id);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
