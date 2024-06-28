using ErrorOr;

namespace Officies.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Offices
        {
            public static Error NotFound => Error.NotFound(
                code: "Offices.NotFound",
                description: "Cannot found the office");
        }
    }
}
