using CG.Models;

namespace CG.DTO.EmployerProfile
{
    public class EmployerProfileDto
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public int CompanyId { get; set; }
        public int? ApprovedByMemberId { get; set; }
        public CompanyRole CompanyRole { get; set; }
        public MembershipStatus MembershipStatus { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}