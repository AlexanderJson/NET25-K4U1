namespace MyWebApi.App.DTO;


public class PagedUserDTO
{
    public Guid Id {get; set;}
    public string Name {get; set;} = "";
    public string Username {get; set;} = "";
    public int CurrentPage {get; set;}
    public int TotalPages {get; set;}
}