namespace InventoryManagementSystem.Models
{
    public enum UserRole
    {
        Admin,
        Staff
    }

    public class User
    {
        private int _userId;
        private string _username;
        private string _passwordHash;
        private string _fullName;
        private UserRole _role;

        public int UserId
        {
            get => _userId;
            private set => _userId = value;
        }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                _username = value.Trim().ToLower();
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;
            private set => _passwordHash = value;
        }

        public string FullName
        {
            get => _fullName;
            set => _fullName = value?.Trim() ?? string.Empty;
        }

        public UserRole Role
        {
            get => _role;
            set => _role = value;
        }

        public User(int userId, string username, string password, string fullName, UserRole role = UserRole.Staff)
        {
            UserId = userId;
            Username = username;
            _passwordHash = HashPassword(password);
            FullName = fullName;
            Role = role;
        }

        public bool ValidatePassword(string password)
        {
            return _passwordHash == HashPassword(password);
        }

        private string HashPassword(string password)
        {
            // Simple hash for demo purposes
            int hash = 0;
            foreach (char c in password)
                hash = (hash * 31 + c) & 0x7FFFFFFF;
            return hash.ToString("X8");
        }

        public override string ToString()
        {
            return $"[{UserId}] {FullName} (@{Username}) - {Role}";
        }
    }
}
