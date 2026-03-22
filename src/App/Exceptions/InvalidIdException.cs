namespace MyWebApi.App.Abstracts;

public class InvalidIdException(string msg) : Exception(msg)
{
}
