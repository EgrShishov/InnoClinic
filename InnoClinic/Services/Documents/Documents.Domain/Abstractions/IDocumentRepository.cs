public interface IDocumentRepository
{
    public Task<Document> GetDocumentAsync(string filename);
    public Task<Document> GetDocumentByResultAsync(int resultId);
    public Task SaveDocumentAsync(Document document);
    public Task DeleteDocumentAsync(Guid id);
}