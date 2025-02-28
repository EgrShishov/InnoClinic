public static partial class Errors
{
    public static class Appointments
    {
        public static Error NotFound => Error.NotFound(
            code: "Appointments.NotFound",
            description: "Cannot found appointment with given id");

        public static Error RescheduleIsImpossibleBecauseIsApproved => Error.Conflict(
            code: "Appointments.CannotReschedule",
            description: "Only appointments that aren’t approved can be rescheduled");

        public static Error ThereAreNoAvaibaleTimeSlots => Error.NotFound(
            code: "Appointments.NoFreeSlots",
            description: "There are no free time slots for this date");

        public static Error AlreadyApproved => Error.Conflict(
            code: "Appointments.AlreadyApproved",
            description: "The appointment is already approved");

        public static Error AlreadyCreated => Error.Conflict(
            code: "Appointments.AlreadyExist",
            description: "Appointment is already created");

        public static Error EmptyList => Error.NotFound(
            code: "Appointments.EmptyList",
            description: "There are no appointments");

        public static Error EmptyHistory => Error.NotFound(
            code: "Appointments.EmptyHistory",
            description: "There are no appointments related to the patient");

        public static Error EmptySchedule => Error.NotFound(
            code: "Appointments.EmptySchedule",
            description: "There are no upcoming or approved appointments in schedule");
    }
}