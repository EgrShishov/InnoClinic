using Microsoft.EntityFrameworkCore;
public class DocumentRepository : IDocumentRepository
{
    private readonly DocumentsDbContext _context;
    public DocumentRepository(DocumentsDbContext context)
    {
        _context = context;
    }

    public async Task SaveDocumentAsync(Document document)
    {
        _context.Documents.Add(document);

        await _context.SaveChangesAsync();
    }

    public async Task<Document> GetDocumentAsync(string filename)
    {
        return await _context.Documents.FirstOrDefaultAsync(d => d.Url.EndsWith(filename));
    }    

    public async Task<Document> GetDocumentByResultAsync(int resultId)
    {
        return await _context.Documents.FirstOrDefaultAsync(d => d.ResultId == resultId);
    }

    public async Task DeleteDocumentAsync(Guid id)
    {
        var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);

        if (document != null)
        {
            _context.Entry(document).State = EntityState.Deleted;
        }
    }
}
