namespace OpenHackathonWeb.Data
{
    public class Users
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public string Password { get; set; }

        public string WalletAddress { get; set; }

        public int UserRole { get; set; }
    }
}
