namespace Domain.Enums
{
    public enum ApplicationStatus
    {
        Draft = 1,
        Submitted = 2,
        ApplicationRejected = 3,
        ApplicationApproved = 4,
        ApplicationPending = 5,

        // Added in cleanup: "Request More Information" previously flipped a
        // pending form back to pending (a no-op). This distinct status lets a
        // form awaiting applicant corrections be told apart from one that was
        // never reviewed. Still counts as a pending-ish state everywhere the
        // pending/approve/reject flows check the status.
        AwaitingMoreInformation = 6
    }
}