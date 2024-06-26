using ErrorOr;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Auth.InvalidCred",
            description: "Invalid credentials.");

        public static Error PasswordNotCoincide => Error.Validation(
            code: "Auth.PasswordNotCoincide",
            description: "The password and repeated password must coincide.");

        public static Error GenerationFailed => Error.Failure(
            code: "Auth.JwtGenerationFailed",
            description: "Cannot generate JWT token");

        public static Error GoogleAuthError => Error.Failure(
            code: "Auth.FailedToAuth",
            description: "Failed to authorize through third-party service. Google");

        public static Error DuplicateEmail => Error.Conflict(
            code: "Auth.DuplicateEmail",
            description: "Email is already in use");

        public static Error DuplicateUsername => Error.Conflict(
            code: "Auth.DuplicateUsername",
            description: "Username is already in use");

        public static Error NotFound => Error.NotFound(
            code: "Auth.NotFound",
            description: "Cannot found the user");

        public static Error InvalidToken => Error.Failure(
            code: "Auth.InvalidToken",
            description: "Invalid access token or refresh token");
    }
}