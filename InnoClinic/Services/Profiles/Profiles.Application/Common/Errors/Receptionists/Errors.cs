public static partial class Errors
{
    public static class Receptionists
    {
        public static Error NotFound(int id) => Error.NotFound(
            code: "Receptionist.DoesNotExist",
            description: $"Receptionist with id: {id} not found.");    

        public static Error EmptyList => Error.NotFound(
            code: "Receptionist.EmptyList",
            description: $"There are no receptionists profiles.");

        public static Error EmailAlreadyExists => Error.Conflict(
            code: "Receptionist.EmailAlreadyExists",
            description: "A receptionist with this email already exists.");

        public static Error InvalidOffice => Error.Validation(
            code: "Receptionist.InvalidOffice",
            description: "The office provided is invalid.");
    }
}
