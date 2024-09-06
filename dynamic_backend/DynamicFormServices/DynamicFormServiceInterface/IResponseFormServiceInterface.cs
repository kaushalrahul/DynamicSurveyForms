using DynamicFormPresentation.Models;
using DynamicFormServices.Dto;

namespace DynamicFormServices.DynamicFormServiceInterface
{
    public interface IResponseFormServiceInterface
    {
        Task<ResponseDto> GetResponseByIdAsync(int id); // Nullable return type
        /* Task<ResponseDto?> GetResponseByEmailAndFormIdAsync(string email, int formId); // Nullable return type
         Task<IEnumerable<ResponseDto>> GetAllResponsesAsync(); // Ensure this is included
         Task SubmitResponseAsync(SubmitResponseDto submitResponseDto);
         Task UpdateResponseAsync(int id, SubmitResponseDto submitResponseDto);
         Task DeleteResponseAsync(int id);*/
        Task<IEnumerable<int>> GetResponseIdsByFormIdAsync(int formId);
        Task<int> GetResponseCountByFormIdAsync(int formId);
        Task SubmitResponseAsync(ResponseDto submitResponseDto);

    }
}
