using OpenHackathonWeb.Helpers;
using System;
using System.Linq;
using System.Security.Claims;

namespace OpenHackathonWeb.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.FindFirst(ClaimTypes.Email).Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return 0;

            ClaimsPrincipal currentUser = user;
            return Convert.ToInt32(currentUser.FindFirst(c => c.Type == Constants.UserIdType).Value);
        }

        public static string GetFirstName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.Claims.First(c => c.Type == Constants.FirstNameType).Value;
        }

        public static string GetLastName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.Claims.First(c => c.Type == Constants.LastNameType).Value;
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            return $"{GetFirstName(user)} {GetLastName(user)}";
        }

        public static string GetWalletAddress(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.Claims.First(c => c.Type == Constants.WalletAddressType).Value;
        }

        public static int GetRole(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return 0;

            ClaimsPrincipal currentUser = user;
            return Convert.ToInt32(currentUser.Claims.First(c => c.Type == Constants.UserRoleType).Value);
        }
    }
}
