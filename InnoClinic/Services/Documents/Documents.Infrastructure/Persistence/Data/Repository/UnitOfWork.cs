public class UnitOfWork : IUnitOfWork
{
    private readonly DocumentsDbContext _context;
    private readonly Lazy<PhotoRepository> _photoRepository;
    private readonly Lazy<DocumentRepository> _documentRepository;

    public UnitOfWork(DocumentsDbContext context)
    {
        _context = context;

        _photoRepository = new(() => new PhotoRepository(_context));
        _documentRepository = new(() => new DocumentRepository(_context));
    }

    public IPhotoRepository Photos => _photoRepository.Value;
    public IDocumentRepository Documents => _documentRepository.Value;

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
