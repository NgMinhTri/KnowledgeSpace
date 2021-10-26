using System.Linq;
using System.Security.Claims;

namespace KnowledgeSpace.WebPortal.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .SingleOrDefault(x => x.Type == "fullname");
            return claim.Value;
        }
    }
}
