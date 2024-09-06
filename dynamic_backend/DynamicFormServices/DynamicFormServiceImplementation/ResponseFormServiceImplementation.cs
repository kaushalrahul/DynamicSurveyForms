using DynamicFormPresentation.Models; // For DynamicFormPresentation.Models.Response
using DynamicFormRepos.DynamicFormRepoInterface;
using DynamicFormServices.Dto;
using DynamicFormServices.DynamicFormServiceInterface;
using Newtonsoft.Json;

namespace DynamicFormServices.DynamicFormServiceImplementation
{
    public class ResponseFormServiceImplementation : IResponseFormServiceInterface
    {
        private readonly IResponseFormRepoInterface _responseRepository;
        public ResponseFormServiceImplementation(IResponseFormRepoInterface responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public async Task<IEnumerable<int>> GetResponseIdsByFormIdAsync(int formId)
        {
            return await _responseRepository.GetResponseIdsByFormIdAsync(formId);
        }

        public async Task<int> GetResponseCountByFormIdAsync(int formId)
        {
            return await _responseRepository.GetResponseCountByFormIdAsync(formId);
        }

        public async Task<ResponseDto?> GetResponseByIdAsync(int id)
        {
            var response = await _responseRepository.GetResponseByIdAsync(id);
            return response != null ? MapToResponseDto(response) : null;
        }

        public async Task SubmitResponseAsync(ResponseDto submitResponseDto)
        {
            if (submitResponseDto == null)
                throw new ArgumentNullException(nameof(submitResponseDto));

            var response = MapToResponse(submitResponseDto);
            await _responseRepository.AddResponseAsync(response);
        }

        /* public async Task<ResponseDto?> GetResponseByEmailAndFormIdAsync(string email, int formId)
         {
             var response = await _responseRepository.GetResponseByEmailAndFormIdAsync(email, formId);
             return response != null ? MapToResponseDto(response) : null;
         }

         public async Task<IEnumerable<ResponseDto>> GetAllResponsesAsync()
         {
             var responses = await _responseRepository.GetAllResponsesAsync();
             return responses.Select(MapToResponseDto).ToList();
         }

         public async Task SubmitResponseAsync(SubmitResponseDto submitResponseDto)
         {
             if (submitResponseDto == null)
                 throw new ArgumentNullException(nameof(submitResponseDto));

             var response = MapToResponse(submitResponseDto);
             await _responseRepository.AddResponseAsync(response);
         }

         public async Task UpdateResponseAsync(int id, SubmitResponseDto submitResponseDto)
         {
             if (submitResponseDto == null)
                 throw new ArgumentNullException(nameof(submitResponseDto));

             var existingResponse = await _responseRepository.GetResponseByIdAsync(id);
             if (existingResponse == null)
                 throw new KeyNotFoundException("Response not found");

             MapToResponse(submitResponseDto, existingResponse);
             await _responseRepository.UpdateResponseAsync(existingResponse);
         }

         public async Task DeleteResponseAsync(int id)
         {
             await _responseRepository.DeleteResponseAsync(id);
         }*/

        // Mapping methods
        /*private ResponseDto MapToResponseDto(Response response)
        {
            if (response == null) return null;

            return new ResponseDto
            {
                Id = response.Id,
                FormID = response.FormId ?? 0, // Handle nullable FormId
                Email = response.Email ?? string.Empty, // Handle nullable Email
                Response = response.Response1 ?? string.Empty // Handle nullable Response1
            };
        }


        private Response MapToResponse(SubmitResponseDto dto)
        {
            return new Response
            {
                FormId = dto.FormID,
                Email = dto.Email,
                Response1 = JsonConvert.SerializeObject(dto.Responses) // Serialize responses
            };
        }

        private void MapToResponse(SubmitResponseDto dto, Response response)
        {
            response.FormId = dto.FormID;
            response.Email = dto.Email;
            response.Response1 = JsonConvert.SerializeObject(dto.Responses); // Serialize responses
        }*/

        private ResponseDto MapToResponseDto(Response response)
        {
            if (response == null) return null;

            return new ResponseDto
            {
                Id = response.Id,
                FormID = response.FormId ?? 0, // Handle nullable FormId
                Email = response.Email ?? string.Empty, // Handle nullable Email
                Response = response.Response1 ?? string.Empty // Handle nullable Response1
            };
        }


        private Response MapToResponse(ResponseDto dto)
        {
            return new Response
            {
                FormId = dto.FormID,
                Email = dto.Email,
                Response1 = dto.Response // Store the JSON string as-is
            };
        }


        private void MapToResponse(SubmitResponseDto dto, Response response)
        {
            response.FormId = dto.FormID;
            response.Email = dto.Email;
            response.Response1 = dto.Response; // Serialize responses
        }
    }
}
