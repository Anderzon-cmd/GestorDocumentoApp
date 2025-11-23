using GestorDocumentoApp.Data;
using GestorDocumentoApp.Extensions;
using GestorDocumentoApp.Models;
using GestorDocumentoApp.Utils;
using GestorDocumentoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace GestorDocumentoApp.Controllers
{
    public class ChangeRequestController : Controller
    {
        public readonly ScmDocumentContext _scmDocumentContext;

        public ChangeRequestController(ScmDocumentContext scmDocumentContext)
        {
            _scmDocumentContext = scmDocumentContext;
        }

        public async Task<IActionResult> Index(int? elementId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                Element? element = null;

                if (elementId.HasValue)
                {
                    element = await _scmDocumentContext.Elements.FindAsync(elementId);
                    if (element is null)
                    {
                        return NotFound();
                    }
                }

                IQueryable<ChangeRequest> query = _scmDocumentContext.ChangeRequests
                    .Include(c => c.Element);

                if (elementId.HasValue)
                {
                    query = query.Where(c => c.ElementId == elementId);
                }

                var changeRequests = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .AsNoTracking()
                    .ToPagedListAsync(pageNumber, pageSize);

                var elements = await _scmDocumentContext.Elements.Include(x => x.Project).AsNoTracking().OrderBy(x => x.Name).ToListAsync();

                var vmList = changeRequests.Items.Select(c => new ChangeRequestVM
                {
                    Id = c.Id,
                    Code = c.Code,
                    Description = c.Description,
                    Priority = EnumHelper.GetDisplayName(c.Priority),
                    Clasification =EnumHelper.GetDisplayName(c.ClasificationType),
                    Action = c.Action??ActionCR.InWait,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt,
                }).ToList();

                return View(
                    new ChangeRequestIndexVM
                    {
                        Items = vmList,
                        PageNumber = changeRequests.PageNumber,
                        PageSize = changeRequests.PageSize,
                        TotalCount = changeRequests.TotalCount,
                        ElementId = element?.Id,
                        ElementName = element?.Name,
                        HasNext=changeRequests.HasNext,
                        HasPrevious = changeRequests.HasPrevious,
                        Elements = elements.Select(x => new SelectListItem { Text = $"{x.Project.Name} - {x.Name}", Value = x.Id.ToString() })

                    });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> Create()
        {
            var elements = await _scmDocumentContext.Elements.AsNoTracking().Include(x => x.Project).OrderBy(x => x.Name).ToListAsync();
            var changeVM = new ChangeRequestCreateVM
            {
                ClasificationOptions = EnumHelper.GetSelectList<ClasificationTypeCR>(),
                PriorityOptions = EnumHelper.GetSelectList<PriorityCR>(),
                StatusOptions = EnumHelper.GetSelectList<StatusCR>(),
                ActionOptions = EnumHelper.GetSelectList<ActionCR>(),
                Elements = elements.Select(x => new SelectListItem { Text = $"{x.Name} - {x.Project.Name}", Value = x.Id.ToString() })

            };
            return View(changeVM);

        }

        [HttpPost]
        public async Task<IActionResult> Create(ChangeRequestCreateVM vm)
        {

            if (!ModelState.IsValid)
            {
                var elements = await _scmDocumentContext.Elements.AsNoTracking().Include(x => x.Project).OrderBy(x => x.Name).ToListAsync();
                vm.ClasificationOptions = EnumHelper.GetSelectList<ClasificationTypeCR>();
                vm.PriorityOptions = EnumHelper.GetSelectList<PriorityCR>();
                vm.StatusOptions = EnumHelper.GetSelectList<StatusCR>();
                vm.ActionOptions = EnumHelper.GetSelectList<ActionCR>();
                vm.Elements = elements.Select(x => new SelectListItem { Text = $"{x.Name} - {x.Project.Name}", Value = x.Id.ToString() });

                return View(vm);
            }

            var changeRequest = new ChangeRequest
            {
                ElementId = vm.ElementId.Value,
                ClasificationType = vm.ClasificationType.Value,
                Description = vm.Description,
                Priority = vm.Priority.Value,
                Status = vm.Status.Value,
                Remarks = vm.Remarks,
                Code = vm.Code,
                Action = vm.Action,
                CreatedAt = DateTime.UtcNow
            };

            _scmDocumentContext.ChangeRequests.Add(changeRequest);
            await _scmDocumentContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { elementId = vm.ElementId });

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ChangeRequestCreateVM vm)
        {
            var changeRequest = await _scmDocumentContext.ChangeRequests.FindAsync(id);

            if (changeRequest is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var elements = await _scmDocumentContext.Elements.AsNoTracking().Include(x => x.Project).OrderBy(x => x.Name).ToListAsync();
                vm.ClasificationOptions = EnumHelper.GetSelectList<ClasificationTypeCR>();
                vm.PriorityOptions = EnumHelper.GetSelectList<PriorityCR>();
                vm.StatusOptions = EnumHelper.GetSelectList<StatusCR>();
                vm.ActionOptions = EnumHelper.GetSelectList<ActionCR>();
                vm.Elements = elements.Select(x => new SelectListItem { Text = $"{x.Name} - {x.Project.Name}", Value = x.Id.ToString() });

                return View(vm);
            }

            changeRequest.ElementId = vm.ElementId.Value;
            changeRequest.ClasificationType = vm.ClasificationType.Value;
            changeRequest.Description = vm.Description;
            changeRequest.Priority = vm.Priority.Value;
            changeRequest.Status = vm.Status.Value;
            changeRequest.Remarks = vm.Remarks;
            changeRequest.Code = vm.Code;
            changeRequest.Action = vm.Action;

            await _scmDocumentContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { elementId = vm.ElementId });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var changeRequest = await _scmDocumentContext.ChangeRequests.FindAsync(id);

            if (changeRequest is null)
            {
                return NotFound();
            }

            var elements = await _scmDocumentContext.Elements.AsNoTracking().Include(x => x.Project).OrderBy(x => x.Name).ToListAsync();

            var vm = new ChangeRequestCreateVM
            {
                Action = changeRequest.Action,
                ElementId = changeRequest.ElementId,
                Code = changeRequest.Code,
                Priority = changeRequest.Priority,
                Status = changeRequest.Status,
                Description = changeRequest.Description,
                Remarks = changeRequest.Remarks,
                ClasificationType = changeRequest.ClasificationType,


                ClasificationOptions = EnumHelper.GetSelectList<ClasificationTypeCR>(),
                PriorityOptions = EnumHelper.GetSelectList<PriorityCR>(),
                StatusOptions = EnumHelper.GetSelectList<StatusCR>(),
                ActionOptions = EnumHelper.GetSelectList<ActionCR>(),
                Elements = elements.Select(x => new SelectListItem { Text = $"{x.Name} - {x.Project.Name}", Value = x.Id.ToString() }),

            };

            return View(vm);

        }





        public async Task<IActionResult> CreateByElementAsync(int id)
        {
            var element = await _scmDocumentContext.Elements.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }
                

            var vm = new ChangeRequestCreateByElementVM
            {
                ElementId = id,
                ElementName = element.Name,
                ClasificationOptions = EnumHelper.GetSelectList<ClasificationTypeCR>(),
                PriorityOptions = EnumHelper.GetSelectList<PriorityCR>(),
                StatusOptions = EnumHelper.GetSelectList<StatusCR>(),
                ActionOptions = EnumHelper.GetSelectList<ActionCR>()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByElement(int id,ChangeRequestCreateByElementVM vm)
        {
            var element = await _scmDocumentContext.Elements.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm.ClasificationOptions = EnumHelper.GetSelectList<ClasificationTypeCR>();
                vm.PriorityOptions = EnumHelper.GetSelectList<PriorityCR>();
                vm.StatusOptions = EnumHelper.GetSelectList<StatusCR>();
                vm.ActionOptions = EnumHelper.GetSelectList<ActionCR>();
                vm.ElementId = element.Id;
                vm.ElementName = element.Name;

                return View(vm);
            }

            

            var changeRequest = new ChangeRequest
            {
                ElementId = element.Id,
                ClasificationType = vm.ClasificationType,
                Description = vm.Description,
                Priority = vm.Priority,
                Status = vm.Status,
                Remarks = vm.Remarks,
                Code = vm.Code,
                Action = vm.Action,
                CreatedAt = DateTime.UtcNow
            };

            _scmDocumentContext.ChangeRequests.Add(changeRequest);
            await _scmDocumentContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { elementId = element.Id });
        }


    }
}
