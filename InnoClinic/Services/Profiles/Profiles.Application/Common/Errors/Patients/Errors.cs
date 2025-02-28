public static partial class Errors
{
    public static class Patients
    {
        public static Error NotFound(int id) => Error.NotFound(
            code: "Patient.DoesNotExist",
            description: $"Patient with id: {id} not found.");        
        
        public static Error EmptyList => Error.NotFound(
            code: "Patient.EmptyList",
            description: $"There are no patients profiles.");

        public static Error NotFoundByFullName => Error.NotFound(
            code: "Patient.DoesNotExist",
            description: "There are no patients with such fullname");

        public static Error InvalidEmail => Error.Validation(
            code: "Patient.InvalidEmail",
            description: "The email provided is invalid.");

        public static Error EmailAlreadyExists => Error.Conflict(
            code: "Patient.EmailAlreadyExists",
            description: "A patient with this email already exists.");

        public static Error InvalidPhoneNumber => Error.Validation(
            code: "Patient.InvalidPhoneNumber",
            description: "The phone number provided is invalid.");        
        
        public static Error AlreadyLinked => Error.Conflict(
            code: "Patient.ProfileAlreadyLinked",
            description: "Profile is already linked to an account.");
    }
}
