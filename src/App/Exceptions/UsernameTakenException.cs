namespace MyWebApi.App.Abstracts;

public class UsernameTakenException(string msg) : Exception(msg)
{
}
