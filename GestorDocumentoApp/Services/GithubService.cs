using Octokit;

namespace GestorDocumentoApp.Services
{
    public class GithubService
    {
    
        private readonly GitHubClient _client;

        public GithubService(IConfiguration configuration)
        {

            var token = configuration["GitHub:Token"];

            _client = new GitHubClient(new ProductHeaderValue("ScmDocumentApp"))
            {
                Credentials = new Credentials(token)
            };

        }

        public async Task<IReadOnlyList<Repository>> GetReposAsync()
        {
            return await _client.Repository.GetAllForCurrent(new RepositoryRequest
            {
                Type=RepositoryType.Public,
                Sort=RepositorySort.Created,
                Direction=SortDirection.Descending
            });
            
        }
    }
}
