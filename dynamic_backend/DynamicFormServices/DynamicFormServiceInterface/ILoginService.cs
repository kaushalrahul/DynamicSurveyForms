namespace DynamicFormServices.DynamicFormServiceInterface
{
    public interface ILoginService
    {
        
        bool ValidateUser(string email, string password);

        
        string GenerateJwtToken(string email);
    }
}
