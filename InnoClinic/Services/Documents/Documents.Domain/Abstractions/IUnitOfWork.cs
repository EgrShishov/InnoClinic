public interface IUnitOfWork : IDisposable
{
    IPhotoRepository Photos { get; }
    IDocumentRepository Documents { get; }
    public Task<int> CompleteAsync();
}