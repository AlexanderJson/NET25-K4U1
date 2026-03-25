public interface IJwTService
{
    string  GenerateToken(Guid userId, string username);
    
}