using System;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace LoginCSharp
{
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

    public class Program
    {
        static async Task Main(string[] args)
        {
            var firebaseClient = new FirebaseClient("https://gametest-6239f-default-rtdb.firebaseio.com/");

            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.Write("Choose an option: ");
            var option = Console.ReadLine();

            Console.Write("Enter username: ");
            var username = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            if (option == "1") // Login
            {
                var user = (await firebaseClient
                  .Child("Users")
                  .OnceAsync<User>())
                  .Where(u => u.Object.Username == username)
                  .Where(u => u.Object.Password == password)
                  .FirstOrDefault();

                if (user != null)
                    Console.WriteLine("User login successful");
                else
                    Console.WriteLine("User not found");
            }
            else if (option == "2") // Register
            {
                User newUser = new User(username, password);
                await firebaseClient
                    .Child("Users")
                    .PostAsync(newUser);
                Console.WriteLine("User registration successful");
            }
        }
    }
}
