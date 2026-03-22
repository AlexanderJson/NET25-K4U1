namespace MyWebApi.App.Exceptions;

public class InvalidIdException(string msg) : Exception(msg)
{
}
