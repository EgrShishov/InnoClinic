public static partial class Errors
{
    public static class Documents
    {
        public static Error NotFound => Error.NotFound(
            code:"Documents.NotFound",
            description:"Cannot found docuemnt with such GUID");

        public static Error NotFoundWithResultsId(int resultId) => Error.NotFound(
            code: "Documents.NotFound",
            description: $"Cannot found docuemnt with results id : {resultId}");

        public static Error InvalidFile => Error.Validation(
            code: "Documents.WrongFile",
            description: "There are some issues with file");

        public static Error BlobNotFound => Error.NotFound(
            code: "Documents.BlobDoesNotExist",
            description: "Cannot found blob file in storage");
    }
}
