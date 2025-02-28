public static partial class Errors
{
    public static class Specialization
    {
        public static Error NotFound => Error.NotFound(
            code: "Specialization.NotFound",
            description: "Specialization not found.");

        public static Error AddingError => Error.Failure(
            code: "Specialization.Error",
            description: "Error occured while adding specialization");

        public static Error EmptySpecializationsList => Error.NotFound(
            code: "Specializations.EmptyList",
            description: "There are no specializations in out database");
    }
}
