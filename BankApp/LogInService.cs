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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Fel användarnamn eller lösenord.");
                Console.ResetColor();

                return null;
            }

            // Checks if the user account is already locked.
            if (CheckLockStatus(foundUser))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Användarkontot är låst.");
                Console.ResetColor();

                return null;
            }

            // Checks if the entered password is correct.
            if (foundUser.Password == password)
            {
                // Resets the failed login counter
                // after a successful login.
                foundUser.ResetFailedLogin();

                Console.WriteLine(
                    "Loggar in...");

                return foundUser;
            }

            // Increases the failed login counter
            // when the entered password is incorrect.
            foundUser.IncrementFailedLogin();

            Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Fel användarnamn eller lösenord.");
                Console.ResetColor();

            // Locks the account after three
            // unsuccessful login attempts.
            if (foundUser.FailedLogins >= 3)
            {
                foundUser.LockAccount();

            Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Användarkontot har låsts efter " +
                    "tre misslyckade försök.");
                Console.ResetColor();
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