public interface IPhotoRepository
{
    public Task<Photo> GetPhotoAsync(string url);
    public Task SavePhotoAsync(Photo photo);
    public Task DeletePhotoAsync(string url);
}