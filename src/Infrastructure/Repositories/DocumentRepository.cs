using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Interfaces;
using MyWebApi.Infrastructure.Data;

namespace MyWebApi.Infrastructure.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly AppDbContext _context;

    public DocumentRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Document> Query()
    {
        return _context.Documents.AsNoTracking();
    }

    public void Add(Document data)
    {
        if (data.Id == Guid.Empty)
            data.Id = Guid.NewGuid();

        _context.Documents.Add(data);
        _context.SaveChanges();
    }

    public List<Document> GetAll()
    {
        return [.. _context.Documents];
    }

    public Document GetById(Guid id)
    {
        return _context.Documents.FirstOrDefault(d => d.Id == id)
            ?? throw new Exception("Document not found");
    }

    public void Update(Document data)
    {
        _context.Documents.Update(data);
        _context.SaveChanges();
    }

    public void Delete(Document data)
    {
        Document document = _context.Documents.FirstOrDefault(d => d.Id == data.Id)
            ?? throw new Exception("Document not found");

        _context.Documents.Remove(document);
        _context.SaveChanges();
    }
}