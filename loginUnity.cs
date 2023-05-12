using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Login : MonoBehaviour
{
    DatabaseReference reference;

    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gametest-6239f-default-rtdb.firebaseio.com/");
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CheckLogin(string username, string password)
    {
        reference.Child("users").Child(username).GetValueAsync().ContinueWith(task => 
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Database Error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                if (snapshot.Exists)
                {
                    string dbPassword = snapshot.Child("password").Value.ToString();
                    if (dbPassword == password)
                    {
                        Debug.Log("Login successful!");
                    }
                    else
                    {
                        Debug.Log("Wrong password.");
                    }
                }
                else
                {
                    Debug.Log("User does not exist.");
                }
            }
        });
    }

    public void Register(string username, string password)
    {
        reference.Child("users").Child(username).Child("password").SetValueAsync(password).ContinueWith(task => 
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Database Error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("User registered successfully!");
            }
        });
    }
}
