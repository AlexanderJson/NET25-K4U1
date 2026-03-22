namespace MyWebApi.App.Abstracts;

public class NotFoundException(string msg) : Exception(msg)
{
}
