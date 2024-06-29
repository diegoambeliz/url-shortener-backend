using System.Security.Cryptography;
using System.Text;
using UrlShortenerApi.Entity.DBEntities;
using UrlShortenerApi.Repository.Repositories;

namespace UrlShortenerApi.Service.Services
{
    public class UserService
    {
        protected readonly UserRepository _repo;

        public UserService(UserRepository repo) => _repo = repo;
        public UserEntity? GetById(string cs, string username) => _repo.GetById(cs, username);
        public string? Login(string cs, string username, string pwd) {


            var user = _repo.GetById(cs, username);

            if (user == null)
                throw new ArgumentException("User not found");

            if (!user.activated)
                throw new ArgumentException("The user has not yet been activated, please verify your email address.");

            string storedHashedPassword = user.pwd;
            byte[] storedSaltBytes = user.salt;
            string enteredPassword = pwd;

            byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);

            byte[] saltedPassword = new byte[enteredPasswordBytes.Length + storedSaltBytes.Length];
            Buffer.BlockCopy(enteredPasswordBytes, 0, saltedPassword, 0, enteredPasswordBytes.Length);
            Buffer.BlockCopy(storedSaltBytes, 0, saltedPassword, enteredPasswordBytes.Length, storedSaltBytes.Length);

            string enteredPasswordHash = HashPassword(enteredPassword, storedSaltBytes);

            if (enteredPasswordHash == user.pwd)
                return user.username;

            return null;
        }
        public void Register(string cs, UserEntity entity)
        {
            byte[] saltBytes = GenerateSalt();
            // Hash the password with the salt
            string hashedPassword = HashPassword(entity.pwd, saltBytes);
            string base64Salt = Convert.ToBase64String(saltBytes);

            byte[] retrievedSaltBytes = Convert.FromBase64String(base64Salt);

            entity.pwd = hashedPassword;
            entity.salt = retrievedSaltBytes;

            _repo.Insert(cs, entity);
        }
        public void Activate(string cs, string username)
        {
            var user = _repo.GetById(cs, username);

            if (user == null)
                throw new ArgumentException("The user doesn't exists");


            if (user.activated == true)
                throw new ArgumentException("User already activated");

            user.activated = true;
            _repo.Update(cs, user);
        }

        #region hash methods
        private string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                // Hash the concatenated password and salt
                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                // Concatenate the salt and hashed password for storage
                byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

                return Convert.ToBase64String(hashedPasswordWithSalt);
            }
        }

        private byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // Adjust the size based on your security requirements
                rng.GetBytes(salt);
                return salt;
            }
        }
        #endregion
    }
}
