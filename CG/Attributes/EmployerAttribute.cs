using CG.Models;
using Microsoft.AspNetCore.Authorization;

namespace CG.Attributes
{
    public class EmployerAttribute : AuthorizeAttribute
    {
        public EmployerAttribute(params CompanyRole[] companyRoles)
        {
            Roles = companyRoles.Length == 0
                ? "Employer"
                : string.Join(",", companyRoles.Select(role => role.ToString()));
        }
    }
}