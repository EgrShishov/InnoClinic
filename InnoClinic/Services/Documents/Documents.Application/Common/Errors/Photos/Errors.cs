public static partial class Errors
{
    public static class Photos
    {
        public static Error NotFound => Error.NotFound(
            code: "Photos.NotFound",
            description: "Cannot found photo with such GUID");

        public static Error BlobNotFound => Error.NotFound(
            code: "Photos.BlobDoesNotExist",
            description: "Cannot found blob file in storage");
    }
}
