using GestorDocumentoApp.Data;
using GestorDocumentoApp.Utils;
using GestorDocumentoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorDocumentoApp.Controllers
{
    
    public class DashboardController : Controller
    {
        private readonly ScmDocumentContext _scmDocumentContext;

        public DashboardController(ScmDocumentContext scmDocumentContext)
        {
            _scmDocumentContext = scmDocumentContext;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var changeSummary = await _scmDocumentContext.ChangeRequests
                    .Include(cr => cr.Element)
                    .ThenInclude(e => e.Project)
                    .GroupBy(cr => new { cr.Element.Project.Name, cr.Status })
                    .Select(g => new ChangeRequestSummaryVM
                    {
                        ProjectName = g.Key.Name,
                        Status = g.Key.Status.GetDisplayNames(),
                        Total = g.Count()
                    })
                    .ToListAsync();

                var projectConfigs = await _scmDocumentContext.Projects
                    .SelectMany(p => p.Elements.Select(e => new ProjectConfigurationVM
                    {
                        ProjectName = p.Name,
                        ElementName = e.Name,
                        LatestVersion = e.Versions
                            .OrderByDescending(v => v.UploadDate)
                            .Select(v => v.VersionCode)
                            .FirstOrDefault(),
                        VersionDate = e.Versions
                            .OrderByDescending(v => v.UploadDate)
                            .Select(v => v.UploadDate)
                            .FirstOrDefault(),
                        Status = e.Versions
                            .OrderByDescending(v => v.UploadDate)
                            .Select(v => v.State)
                            .FirstOrDefault(),
                    }))
                    .ToListAsync();

                var vm = new DasboardVM
                {
                    ChangeRequestSummary = changeSummary,
                    ProjectConfigurations = projectConfigs
                };

                return View(vm);
            }catch(Exception ex)
            {
                return NotFound();
            }

        }
    }
}
