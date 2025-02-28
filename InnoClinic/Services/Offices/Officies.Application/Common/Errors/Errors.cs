public static partial class Errors
{
    public static class Offices
    {
        public static Error NotFound => Error.NotFound(
            code: "Offices.NotFound",
            description: "Cannot found the office");

        public static Error EmptyOfficesList => Error.NotFound(
            code: "Offices.Empty",
            description: "There are no offices in our database");
    }

    public static class FilesApi
    {
        public static Error UploadingError => Error.Failure(
            code: "FilesApi.UploadingError",
            description: "Cannot upload photo to files service");

        public static Error DeletingError => Error.Failure(
            code: "FilesApi.DeletingError",
            description: "Cannot delete photo from files service");
    }
}
