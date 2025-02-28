public static partial class Errors
{
    public static class Office
    {
        public static Error NotFound => Error.NotFound(
            code: "Office.NotFound",
            description: "Office not found.");
    }
}