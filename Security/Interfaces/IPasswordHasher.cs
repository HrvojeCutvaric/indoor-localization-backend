namespace IndoorLocalization.Security.Interfaces
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password, string salt);

        public string GenerateSalt();
    }
}
