using DynamicFormPresentation.Models;

namespace DynamicFormRepos.DynamicFormRepoInterface
{
    public interface IResponseFormRepoInterface
    {
        Task<Response> GetResponseByIdAsync(int id);
        /* Task<Response> GetResponseByEmailAndFormIdAsync(string email, int formId);
         Task AddResponseAsync(Response response);
         Task UpdateResponseAsync(Response response);
         Task DeleteResponseAsync(int id);
         Task<IEnumerable<Response>> GetAllResponsesAsync();*/
        Task<IEnumerable<int>> GetResponseIdsByFormIdAsync(int formId);
        Task<int> GetResponseCountByFormIdAsync(int formId);
        Task AddResponseAsync(Response response);

    }
}
