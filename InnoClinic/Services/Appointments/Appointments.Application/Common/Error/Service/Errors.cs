public static partial class Errors
{
    public static class Service
    {
        public static Error NotFound(int id) => Error.NotFound(
            code: "Service.DoesNotExist",
            description: $"Cannot found service with id : {id}");
    }
}