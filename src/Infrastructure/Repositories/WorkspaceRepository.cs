using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Interfaces;
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;

namespace MyWebApi.Infrastructure.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly AppDbContext _context;

    public WorkspaceRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Workspace> Query()
    {
        return _context.Workspaces.AsNoTracking();
    }

    public void Add(Workspace data)
    {
        if (data.Id == Guid.Empty)
            data.Id = Guid.NewGuid();

        _context.Workspaces.Add(data);
        _context.SaveChanges();
    }

    public List<Workspace> GetAll()
    {
        return [.. _context.Workspaces];
    }

    public Workspace GetById(Guid id)
    {
        return _context.Workspaces.FirstOrDefault(w => w.Id == id)
            ?? throw new Exception("Workspace not found");
    }

    public void Update(Workspace data)
    {
        _context.Workspaces.Update(data);
        _context.SaveChanges();
    }

    public void Delete(Workspace data)
    {
        Workspace workspace = _context.Workspaces.FirstOrDefault(w => w.Id == data.Id)
            ?? throw new Exception("Workspace not found");

        _context.Workspaces.Remove(workspace);
        _context.SaveChanges();
    }
}