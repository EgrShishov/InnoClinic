using Microsoft.EntityFrameworkCore;

public class PhotoRepository : IPhotoRepository
{
    private readonly DocumentsDbContext _context;

    public PhotoRepository(DocumentsDbContext context)
    {
        _context = context;
    }

    public async Task SavePhotoAsync(Photo photo)
    {
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
    }

    public async Task<Photo> GetPhotoAsync(string url)
    {
        return await _context.Photos.FirstOrDefaultAsync(p => p.Url == url);
    }

    public async Task DeletePhotoAsync(string url)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Url == url);

        if (photo != null)
        {
            _context.Entry(photo).State = EntityState.Deleted;
        }
    }
}
