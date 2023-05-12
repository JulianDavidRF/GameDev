using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class Login : MonoBehaviour
{
    DatabaseReference reference;

    private async void Start()
    {
        // Initialize Firebase
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to initialize Firebase with {task.Exception}");
                return;
            }

            // Set up Firebase Database
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://your-database-url.firebaseio.com/");
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });

        // Example usage:
        string username = "testuser";
        string password = "password";
        
        // Register the user
        await RegisterNewUser(username, password);
        
        // Try to log in
        bool loginSuccess = await TryLogin(username, password);
        Debug.Log($"Login success: {loginSuccess}");
    }

    public async Task<bool> TryLogin(string username, string password)
    {
        User user = await GetUser(username);
        if (user == null)
        {
            Debug.LogError("Login failed: user not found");
            return false;
        }

        if (user.password == password)
        {
            Debug.Log("Login successful");
            return true;
        }
        else
        {
            Debug.LogError("Login failed: incorrect password");
            return false;
        }
    }

    public async Task RegisterNewUser(string username, string password)
    {
        var user = new User(username, password);
        string json = JsonUtility.ToJson(user);

        await reference.Child("users").Child(username).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed to register user with {task.Exception}");
                return;
            }

            Debug.Log("User registration successful");
        });
    }

    public async Task<User> GetUser(string username)
    {
        User user = null;

        await reference.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed to retrieve user with {task.Exception}");
                return;
            }

            if (task.Result.Exists)
            {
                user = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());
                Debug.Log("User retrieved successfully");
            }
            else
            {
                Debug.Log("No user found");
            }
        });

        return user;
    }
}

[Serializable]
public class User
{
    public string username;
    public string password;

    public User(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
