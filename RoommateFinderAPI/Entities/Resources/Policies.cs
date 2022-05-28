using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace RoommateFinderAPI.Entities.Resources
{
    public class Policies
    {
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string Owner = "Owner";
        public const string Roommate = "Roommate";
        public static AuthorizationPolicy Policy(string role)
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser().RequireRole(role);

            return policy.Build();
        }
    }
}