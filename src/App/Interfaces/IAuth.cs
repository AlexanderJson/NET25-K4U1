public interface IAuth 
{
    Task<string> Login(string username, string password);
}