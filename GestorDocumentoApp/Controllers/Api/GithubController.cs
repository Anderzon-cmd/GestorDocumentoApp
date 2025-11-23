using GestorDocumentoApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Threading.Tasks;

namespace GestorDocumentoApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]

    [AllowAnonymous]
    public class GithubController : ControllerBase
    {
        public readonly GithubService _githubService;
        public GithubController(GithubService githubService)
        {
            this._githubService = githubService;
        }

        [HttpGet("repos")]
        
        public async Task<IReadOnlyList<Repository>> GetAll()
        {
            return await _githubService.GetReposAsync();
        }

    }
}
