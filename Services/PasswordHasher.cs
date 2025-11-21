using BCrypt.Net;

namespace CMCSP3.Services
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}