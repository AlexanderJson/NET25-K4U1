namespace MyWebApi;

public class Program
{

       
    public static void Main(string[] args)
    {
        Test t = new Test();
        t.Hey(1);
    }
}


public class Test()
{
     
    public void Hey(int name)
    {
        if(name == 1)
        {
            Console.WriteLine("hej");
        }
        else
        {
            Console.WriteLine("då");
        }
    }
}
