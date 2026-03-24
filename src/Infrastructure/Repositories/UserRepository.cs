
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;
using MyWebApi.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace MyWebApi.Infrastructure.Repositories.Users;

//TODO async?
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }


    public IQueryable<User> Query()
    {
        return _context.Users.AsNoTracking();;
    }




    

    public void Add(User user)
    {
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    // Never use
    public List<User> GetAll()
    {
        return [.. _context.Users];
    }



	public User GetById(Guid id)
	{
        return  _context.Users.FirstOrDefault(u => u.Id == id) ?? throw new Exception("User not found");
    }

    public void Delete(User data)
    {
        User user = _context.Users.FirstOrDefault(u => u.Id == data.Id) ?? throw new Exception("User not found");
        _context.Remove(user);
        _context.SaveChanges();
    }


    public bool IsUsernameTaken(string username)
    {
        return  _context.Users.Any(u => u.Username == username);
    }


    public void Update(User user)
    {
        _context.Users.Update(user); // Since I use GetById in Service it tracks the entity anyways, but kept this for safety
        _context.SaveChanges(); 
    }

    public bool IsEmailTaken(string email)
    {
        return  _context.Users.Any(u => u.Email == email);
    }
}










// LÄGG IN I EGEN DOKUMENTATION SEN

/*

    /// <summary>
    /// Simple logic to return how many pages to return.
    /// Now integers only allow whole numbers. So integer-division truncates any floating point
    /// in the result. This will eventually lead to incorrect pagenumbers returning.
    /// 
    /// Example: 
    /// Say we have 29 users returned + we only want to show 10 users per page.
    /// 
    /// Problem:
    /// With integer division: 
    /// 29/10 = 2.9 
    /// BUT integers truncates any floating point so it would be 2
    /// This means that 9 users would not show.
    /// 
    /// Solution:
    /// We need to factor in a margin for error to handle edge cases like my above example.
    /// So we add padding <see cref="TotalCount + PageSize"/> and a safety "nudge" <see cref="-1"/> 
    /// to make sure we dont round up too much. Then we can divide it like normal. 
    /// (29 + 10 - 1) / PageSize ----> 38/10 = 3.8 (returning 3)
    /// This is faster than converting to a floating-point division then back to integer
    /// (excluding the time I had to take to write this comment)
    /// </summary>
    /// 
    /// 
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;   


*/


/*

    /// <summary>
    /// Instead of using "(TotalCount + PageSize - 1) / PageSize)".
    /// This is a way to return a correct number of pages and handle integer overflow.
    /// The other way is faster but the trade-off in readability is worth it
    /// since the loss of speed is negligible. 
    /// 
    /// It performs integer division but also calculates (left - (quotient * right))
    /// so it can return the remainder sum too.
    /// 
    /// We do this because integers truncates floating points which will eventually lead to
    /// incorrect pagenumbers being returned.  
    /// </summary>
    public int TotalPages
    {
        get
        {
            (var quotient, var remainder) = Math.DivRem(TotalCount, PageSize);
            return remainder > 0 ? quotient+1 : quotient;
        }
    } 


    /// <summary>
    /// Instead of using "(TotalCount + PageSize - 1) / PageSize)".
    /// This is a way to return a correct number of pages and handle integer overflow.
    /// The other way is faster but the trade-off in readability is worth it
    /// since the loss of speed is negligible. 
    /// 
    /// It performs integer division but also calculates (left - (quotient * right))
    /// so it can return the remainder sum too.
    /// 
    /// We do this because integers truncates floating points which will eventually lead to
    /// incorrect pagenumbers being returned.  
    /// </summary>

*/



/*
        /// <summary>
        /// Simple username search. 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IQueryable<T> SearchByString(string term)
        {
            return string.IsNullOrWhiteSpace(term)
            ? query
            : query.Where(u => u().Contains(term));
        }

        // For registration. Better to fetch a bool than full object 
        public bool Exists(string username) => query.Any(u => u.Username == username);

*/


    /*
        Returns data that inherits the IQueryable interace.
        This is needed so we can build queries in-code.
        IQueryable needs to be triggered to actually send anything to the DB. 
        Best way to look at it, is like a set of pre-defined instructions (intermediate operations).
        When we chain more methods, they build and append to the expression tree. 
        The query only runs when a trigger (Terminal operation) is applied. 
        So best way to explain it is that we have a set of instructions (find X after n anounts of data etc)
        that can be chained together to make a complicated inustruction. Then that can be triggered.

        basically;
        When typing IQueryable, 
        we assert that these methods are not to be run, 
        instead they are to be applied to a "Expression Tree" that holds the order of the insutrctions (the query)
    */