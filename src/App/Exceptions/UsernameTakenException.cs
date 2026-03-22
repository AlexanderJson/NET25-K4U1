namespace MyWebApi.App.Exceptions;

public class UsernameTakenException(string msg) : Exception(msg)
{
}
