public static partial class Errors
{
    public static class Doctors
    {
        public static Error NotFound(int id) => Error.NotFound(
            code: "Doctor.DoesNotExist",
            description: $"Doctor with id: {id} not found");
        
        public static Error EmptyList => Error.NotFound(
            code: "Doctor.EmptyList",
            description: "There are no doctors profiles");        
        
        public static Error NotFoundByFullName => Error.NotFound(
            code: "Doctor.DoesNotExist",
            description: "There are no doctors with such fullname");

        public static Error InvalidSpecialization => Error.Validation(
            code: "Doctor.InvalidSpecialization",
            description: "The specialization provided is invalid.");

        public static Error LicenseNumberAlreadyExists => Error.Conflict(
            code: "Doctor.LicenseNumberAlreadyExists",
            description: "A doctor with this license number already exists.");
    }
}
