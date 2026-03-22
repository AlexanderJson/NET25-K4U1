namespace MyWebApi.App.Exceptions;

public class NotFoundException(string msg) : Exception(msg)
{
}
