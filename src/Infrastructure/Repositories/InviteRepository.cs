using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Interfaces;
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;

namespace MyWebApi.Infrastructure.Repositories;

public class InviteRepository : IInviteRepository
{
    private readonly AppDbContext _context;

    public InviteRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Invite> Query()
    {
        return _context.Invites.AsNoTracking();
    }

    public void Add(Invite data)
    {
        if (data.Id == Guid.Empty)
            data.Id = Guid.NewGuid();

        _context.Invites.Add(data);
        _context.SaveChanges();
    }

    public List<Invite> GetAll()
    {
        return [.. _context.Invites];
    }

    public Invite GetById(Guid id)
    {
        return _context.Invites.FirstOrDefault(i => i.Id == id)
            ?? throw new Exception("Invite not found");
    }

    public void Update(Invite data)
    {
        _context.Invites.Update(data);
        _context.SaveChanges();
    }

    public void Delete(Invite data)
    {
        Invite invite = _context.Invites.FirstOrDefault(i => i.Id == data.Id)
            ?? throw new Exception("Invite not found");

        _context.Invites.Remove(invite);
        _context.SaveChanges();
    }

    public bool TokenExists(string token)
    {
        return _context.Invites.Any(i => i.Token == token);
    }
}