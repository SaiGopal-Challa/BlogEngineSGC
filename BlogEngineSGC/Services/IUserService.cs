namespace BlogEngineSGC.Services
{
    public interface IUserService
    {
        bool ValidateUser(string username, string password);
    }
}
