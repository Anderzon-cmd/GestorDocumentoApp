using GestorDocumentoApp.Data;
using GestorDocumentoApp.Models;
using GestorDocumentoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestorDocumentoApp.Controllers
{
    public class VersionController : Controller
    {
        private readonly ScmDocumentContext _scmDocumentContext;
        private readonly ILogger<VersionController> _logger;

        public const string VERSION_STATE_ACTIVE = "active";
        public const string VERSION_STATE_INACTIVE = "inactive";

        public VersionController(ScmDocumentContext scmDocumentContext, ILogger<VersionController> logger)
        {
            _scmDocumentContext = scmDocumentContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int elementId)
        {
            var element = await _scmDocumentContext.Elements.Include(x => x.Versions.OrderByDescending(x => x.UploadDate))
                .ThenInclude(x => x.ParentVersion)
                .ThenInclude(x => x.RequirementType).FirstOrDefaultAsync(x => x.Id == elementId);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);


        }

        public async Task<IActionResult> Create(int elementId)
        {
            var element = await _scmDocumentContext.Elements.FindAsync(elementId);

            if (element is null)
            {
                return NotFound();
            }

            var requirementTypes = await _scmDocumentContext.RequirementTypes.AsNoTracking()
                .OrderBy(r => r.Name).ToListAsync();

            var versions = await _scmDocumentContext.Versions.AsNoTracking().Where(x => x.ElementId == element.Id)
                .OrderBy(v => v.VersionCode).ToListAsync();

            var changeRequests = await _scmDocumentContext.ChangeRequests
                .AsNoTracking()
                .Where(x => x.ElementId == element.Id)
                .Where(x => x.Action == ActionCR.Approved)
                .Where(x=>x.Status==StatusCR.Action).ToListAsync();

            return View(new VersionVM
            {
                ElementName = element.Name,
                ElementId = element.Id,
                RequirementTypes = requirementTypes.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }),
                PreviousVersions = versions.Select(v => new SelectListItem { Text = v.VersionCode + " | " + v.Id, Value = v.Id.ToString() }),
                ChangeRequests = changeRequests.Select(x => new SelectListItem { Text = x.Code, Value = x.Id.ToString() }),
                Phases = new SelectListItem[] {
                    new SelectListItem { Value="1",Text="Planificación"},
                    new SelectListItem { Value="2",Text="Análisis"},
                    new SelectListItem { Value="3",Text="Diseño"},
                    new SelectListItem { Value="4",Text="Implementación"},
                    new SelectListItem { Value="5",Text="Prueba"},
                    new SelectListItem { Value="6",Text="Mantenimiento"},
                }

            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(int elementId, VersionVM versionVM)
        {
            try
            {
                var element = await _scmDocumentContext.Elements.FindAsync(elementId);

                if (element is null)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    await LoadDropDowns(versionVM);
                    return View(versionVM);
                }

                var version = new Models.Version
                {
                    Name = versionVM.Name,
                    ElementUrl = versionVM.ElementUrl,
                    UploadDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    State = VERSION_STATE_INACTIVE,
                    ToolUrl = versionVM.ToolUrl,
                    VersionCode = versionVM.VersionCode,
                    ElementId = element.Id,
                    ChangeRequestId = versionVM.ChangeRequestId.Value,
                    Phase = versionVM.Phase.Value,
                    iteration = versionVM.Iteration.Value,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    RequirementTypeId = versionVM.RequirementTypeId.Value,
                    ParentVersionId = versionVM.ParentVersionId
                };

                _scmDocumentContext.Add(version);
                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { elementId = element.Id });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving Version {VersionName}", versionVM.Name);

                TempData["Message"] = "El CR ya esta asociado a una version.";
                TempData["MessageType"] = "warning";

                return RedirectToAction(nameof(Index), new { elementId = elementId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Version {VersionName}", versionVM.Name);
                return RedirectToAction(nameof(Index), new { elementId = elementId });
            }
        }

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var version = await _scmDocumentContext.Versions.FindAsync(id);

            if (version is null)
            {
                return NotFound();
            }

            var vm = new VersionVM
            {
                Id = version.Id,
                Name = version.Name,
                ElementId = version.ElementId,
                ElementName = version.Name,
                ElementUrl = version.ElementUrl,
                UploadDate = version.UploadDate,
                ChangeRequestId=version.ChangeRequestId,
                State = version.State,
                Phase = version.Phase,
                Iteration = version.iteration,
                ToolUrl = version.ToolUrl,
                VersionCode = version.VersionCode,

                RequirementTypeId = version.RequirementTypeId,
                ParentVersionId = version.ParentVersionId

            };

            await LoadDropDowns(vm);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, VersionVM versionVM)
        {
            try
            {
                var version = await _scmDocumentContext.Versions.FindAsync(id);

                if (version is null)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    await LoadDropDowns(versionVM);
                    return View(versionVM);
                }

                version.Name = versionVM.Name;
                version.ElementUrl = versionVM.ElementUrl;
                version.UploadDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                version.ToolUrl = versionVM.ToolUrl;
                version.iteration = versionVM.Iteration.Value;
                version.Phase = versionVM.Phase.Value;
                version.ChangeRequestId = versionVM.ChangeRequestId.Value;
                version.VersionCode = versionVM.VersionCode;
                version.RequirementTypeId = versionVM.RequirementTypeId.Value;
                version.ParentVersionId = versionVM.ParentVersionId;

                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating Version {VersionName}", versionVM.Name);

                TempData["Message"] = "El CR ya esta asociado a una version.";
                TempData["MessageType"] = "warning";

                return RedirectToAction(nameof(Index), new { elementId = versionVM.ElementName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Version {VersionName}", versionVM.Name);
                return RedirectToAction(nameof(Index), new { elementId = versionVM.ElementId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var version = await _scmDocumentContext.Versions.FindAsync(id);

                if (version is null)
                {
                    return NotFound();
                }

                _scmDocumentContext.Versions.Remove(version);
                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Version {VersionId}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetVersion(int id)
        {
            var version = await _scmDocumentContext.Versions.Include(x=>x.ChangeRequest).Where(x=>x.Id==id).FirstOrDefaultAsync();
            if (version is null)
            {
                TempData["Message"] = "No existe la version.";
                TempData["MessageType"] = "warning";
                return NotFound();
            }


            try
            {
                var activeVersion = await _scmDocumentContext.Versions
                    .Where(x => x.ElementId == version.ElementId).AnyAsync(x => x.State == VERSION_STATE_ACTIVE);

                if (activeVersion)
                {
                    TempData["Message"] = "Ya existe una version.";
                    TempData["MessageType"] = "warning";

                    return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
                }

                TempData["Message"] = "Version activada correctamente.";
                TempData["MessageType"] = "success";

                version.State = VERSION_STATE_ACTIVE;
                version.ChangeRequest.Status = StatusCR.Baselined;

                await _scmDocumentContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
            }
            catch (Exception ex)
            {
                {
                    TempData["Message"] = "Error al establecer la nueva version.";
                    TempData["MessageType"] = "error";
                    _logger.LogError(ex, "Error set new version.");
                    return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpVersion(int id)
        {
            var version = await _scmDocumentContext.Versions
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (version is null)
            {
                return NotFound();
            }

            if (version.State == VERSION_STATE_INACTIVE)
            {

                TempData["Message"] = "La version es inactiva.";
                TempData["MessageType"] = "warning";

                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
            }

            try
            {
                var versionLevelUp = await _scmDocumentContext.Versions
                    .Include(x => x.ParentVersion)
                    .Include(x=>x.ChangeRequest)
                    .FirstOrDefaultAsync(x => x.ParentVersion.Id == version.Id);

                if (versionLevelUp is null)
                {
                    TempData["Message"] = "No existe una version padre.";
                    TempData["MessageType"] = "warning";
                    return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
                }

                version.State = VERSION_STATE_INACTIVE;

                versionLevelUp.State = VERSION_STATE_ACTIVE;
                versionLevelUp.ChangeRequest.Status = StatusCR.Baselined;

                await _scmDocumentContext.SaveChangesAsync();

                TempData["Message"] = "Version subida de nivel.";
                TempData["MessageType"] = "success";

                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Erro to up version");
                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownVersion(int id)
        {

            var version = await _scmDocumentContext.Versions.Include(x => x.ParentVersion).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (version is null)
            {
                return NotFound();
            }

            if (version.State == VERSION_STATE_INACTIVE)
            {

                TempData["Message"] = "La version es inactiva.";
                TempData["MessageType"] = "warning";
                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
            }

            try
            {
                if (version.ParentVersion is null)
                {
                    TempData["Message"] = "No existe una version padre.";
                    TempData["MessageType"] = "warning";
                    return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
                }

                var versionParent = await _scmDocumentContext.Versions.FindAsync(version.ParentVersionId);


                version.State = VERSION_STATE_INACTIVE;

                versionParent.State = VERSION_STATE_ACTIVE;

                await _scmDocumentContext.SaveChangesAsync();

                TempData["Message"] = "Version bajada de nivel.";
                TempData["MessageType"] = "success";

                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error to down version");
                return RedirectToAction(nameof(Index), new { elementId = version.ElementId });
            }
        }

        private async Task LoadDropDowns(VersionVM vm)
        {
            var requirementTypes = await _scmDocumentContext.RequirementTypes.OrderBy(r => r.Name).ToListAsync();
            var versions = await _scmDocumentContext.Versions.Where(x => x.ElementId == vm.ElementId).OrderBy(v => v.VersionCode).ToListAsync();
            var changeRequests = await _scmDocumentContext.ChangeRequests.
                Where(x => x.ElementId == vm.ElementId).
                Where(x => x.Action == ActionCR.Approved).ToListAsync();

            vm.RequirementTypes = requirementTypes.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() });
            vm.PreviousVersions = versions.Select(v => new SelectListItem { Text = v.VersionCode + " | " + v.Id, Value = v.Id.ToString() });
            vm.ChangeRequests = changeRequests.Select(x => new SelectListItem { Text = x.Code, Value = x.Id.ToString() });
            vm.Phases = new SelectListItem[] {
                    new SelectListItem { Value="1",Text="Planificación"},
                    new SelectListItem { Value="2",Text="Análisis"},
                    new SelectListItem { Value="3",Text="Diseño"},
                    new SelectListItem { Value="4",Text="Implementación"},
                    new SelectListItem { Value="5",Text="Prueba"},
                    new SelectListItem { Value="6",Text="Mantenimiento"},
                };
        }
    }


}
