
public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Auth.InvalidCred",
            description: "Invalid credentials.");

        public static Error GenerationFailed => Error.Failure(
            code: "Auth.JwtGenerationFailed",
            description: "Cannot generate JWT token");

        public static Error InvalidAccessToken => Error.Failure(
            code: "Auth.InvalidToken",
            description: "Invalid access token");       

        public static Error InvalidRefreshToken => Error.Failure(
            code: "Auth.InvalidToken",
            description: "Invalid refresh token");

        public static Error PrincipalError(string message) => Error.Failure(
            code: "Auth.GetPrincipalError",
            description: $"There are error with getting principal from token {message}");

        public static Error InvalidEmailVerificationLink => Error.Forbidden(
            code: "Auth.InvalidLink",
            description: "Your link is expired or invalid. Try later or generate a new one");
    }

    public static class Account
    {
        public static Error NotFound(int id) => Error.NotFound(
            code: "Account.DoesNotExist",
            description: $"Cannot find account with given id: {id}");

        public static Error DeleteFailed => Error.Failure(
            code: "Account.DeleteFailure",
            description: "Deletion operation error");

        public static Error UpdateFailed => Error.Failure(
            code: "Account.UpdateFailure",
            description: "Update operation error");

        public static Error DuplicateEmail => Error.Conflict(
           code: "Auth.DuplicateEmail",
           description: "Email is already in use");

        public static Error DuplicateUsername => Error.Conflict(
            code: "Auth.DuplicateUsername",
            description: "Username is already in use");

        public static Error CreationFailed(string description) => Error.Failure(
            code: "Auth.CreationFailed",
            description: $"Cannot create a new account entity: {description}");
    }
}