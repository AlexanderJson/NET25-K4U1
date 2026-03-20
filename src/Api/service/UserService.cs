public class UserService
{
    public bool Auth()
    {
        var u = returnUser();
        if (string.IsNullOrEmpty(u.Username) || string.IsNullOrEmpty(u.Password))
            return false;
        
        if (u.Username == "Alex" && u.Password == "123") 
            return true;
        
        return false;
    }

    public User returnUser()
    {
        return new User{Name="Alex", Username="Alexander", Password="123"};
    }
}


public class User
{
    public string Name {get; set;} = "";
    public string Username {get; set;} = "";
    public string Password {get; set;} = "";

}