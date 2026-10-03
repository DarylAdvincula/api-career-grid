namespace CG.Models
{
    public enum CompanyRole
    {
        Owner,
        Admin,
        Recruiter,
        HiringManager
    }

    public enum MembershipStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class EmployerProfile
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public int CompanyId { get; set; }
        public int ApproverByMemberId { get; set; }
        public CompanyRole CompanyRole { get; set; } = CompanyRole.Recruiter;
        public MembershipStatus MembershipStatus { get; set; } = MembershipStatus.Pending;
        public DateTime JoinedAt { get; init; } = DateTime.Now;
        public UserAccount UserAccount { get; set; } = null!;
        public Company Company { get; set; } = null!;
        public UserAccount ApprovedByMember { get; set; } = null!;

        // +2 checks
        // [x] MembershipStatus is in MembershipStatus enum
        // [x] CompanyRole is in CompanyRole enum

        // +1 index (non-clustered)
        // [x] CompanyId
    }
}