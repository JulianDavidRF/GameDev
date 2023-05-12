// See https://aka.ms/new-console-template for more information
using System;

namespace LoginApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Please enter password:");
            string password = Console.ReadLine();

            User user = new User(username, password);

            LoginService loginService = new LoginService();

            if (loginService.Login(user))
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Login failed.");
            }
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }

    public class LoginService
    {
        public bool Login(User user)
        {
            // This is a very simplistic check, a real-world application
            // would have more complex logic and likely check a database
            if (user.Username == "admin" && user.Password == "password")
            {
                return true;
            }
            return false;
        }
    }
}
