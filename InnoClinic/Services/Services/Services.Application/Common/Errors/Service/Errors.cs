public static partial class Errors
{
    public static class Service
    {
        public static Error NotFound => Error.NotFound(
            code: "Service.NotFound",
            description: "Service not found.");

        public static Error EmptyServicesList => Error.NotFound(
            code: "Services.EmptyList",
            description: "There are no services in our database");

        public static Error NoActiveServices => Error.NotFound(
            code: "Services.NoActive",
            description: "There are no active services or specializations at the current moment");
    }
}
