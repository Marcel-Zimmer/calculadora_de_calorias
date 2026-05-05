using Microsoft.AspNetCore.Identity;
using CalculadoraCalorias.Core.Domain.Entities;
using BCrypt.Net;

namespace CalculadoraCalorias.Infrastructure.Services
{
    public class CustomPasswordHasher : PasswordHasher<ApplicationUser>
    {
        public override PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            // BCrypt hashes usually start with $2a$, $2b$ or $2y$
            if (hashedPassword != null && hashedPassword.StartsWith("$2"))
            {
                try
                {
                    bool isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
                    if (isValid)
                    {
                        // SuccessRehashNeeded informs Identity to update the hash to the current default (PBKDF2)
                        return PasswordVerificationResult.SuccessRehashNeeded;
                    }
                }
                catch
                {
                    // If BCrypt verification fails due to invalid format, fall back to default
                }
            }

            return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
