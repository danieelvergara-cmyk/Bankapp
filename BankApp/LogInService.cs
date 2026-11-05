namespace BankApp
{
    public class LoginService
    {
        // Provides access to the same user list
        // that is managed by UserService.
        private UserService userService;

        // Receives the shared UserService instance
        // when LoginService is created.
        public LoginService(UserService userService)
        {
            this.userService = userService;
        }

        // Authenticates the user.
        // Returns the authenticated User object if successful.
        // Returns null if the authentication fails.
        public User? Authenticate(
            string username,
            string password)
        {
            User? foundUser = null;

            // Searches for a user with the given username.
            foreach (User user in userService.Users)
            {
                if (user.Username == username)
                {
                    foundUser = user;
                    break;
                }
            }

            // Stops the login attempt if no user was found.
            if (foundUser == null)
            {
                Console.WriteLine(
                    "Invalid username or password.");

                return null;
            }

            // Checks if the user account is already locked.
            if (CheckLockStatus(foundUser))
            {
                Console.WriteLine(
                    "Account is locked.");

                return null;
            }

            // Checks if the entered password is correct.
            if (foundUser.Password == password)
            {
                // Resets the failed login counter
                // after a successful login.
                foundUser.ResetFailedLogin();

                Console.WriteLine(
                    "Login successful.");

                return foundUser;
            }

            // Increases the failed login counter
            // when the entered password is incorrect.
            foundUser.IncrementFailedLogin();

            Console.WriteLine(
                "Invalid username or password.");

            // Locks the account after three
            // unsuccessful login attempts.
            if (foundUser.FailedLogins >= 3)
            {
                foundUser.LockAccount();

                Console.WriteLine(
                    "Account has been locked after " +
                    "three failed login attempts.");
            }

            return null;
        }

        // Returns true if the user account is locked.
        // Otherwise, it returns false.
        public bool CheckLockStatus(User user)
        {
            return user.IsLocked;
        }
    }
}