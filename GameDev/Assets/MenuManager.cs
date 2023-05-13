using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using UnityEditor.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;


public class MenuManager : MonoBehaviour
{
    //DataBase
    DatabaseReference mDataBaseR;
    FirebaseApp app;
    //Menu Controller
    [SerializeField] private GameObject m_menuUI = null;
    [SerializeField] private GameObject m_loginUI = null;
    [SerializeField] private GameObject m_RegisterUI = null;
    //Login Form
    [SerializeField] private InputField inputUserLogin = null;
    [SerializeField] private InputField inputPaswordLogin = null;
    [SerializeField] private Button btnConnectLogin = null;
    [SerializeField] private Button btnRegisterLogin = null;
    //Register Form
    [SerializeField] private InputField inputUserRegister = null;
    [SerializeField] private InputField inputPaswordRegister = null;
    [SerializeField] private InputField inputEmaillRegister = null;
    [SerializeField] private Button btnRegisterRegister = null;
    [SerializeField] private Button btnLoginRegister = null;
    //Global Variables
    [SerializeField] private Text txtError = null;
    private User userReturn = null;
    private Boolean userFound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        mDataBaseR = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnClickLogin()
    {
        StartCoroutine(CheckUserCoroutineLogin());
    }

    public void OnClickRegister()
    {
        StartCoroutine(CheckUserCoroutineRegister());
    }

    private IEnumerator CheckUserCoroutineLogin()
    {
        string username = inputUserLogin.text;
        string password = inputPaswordLogin.text;
        
        yield return CheckUser(username);
        
        if (userFound)
        {
            if (userReturn.password.Equals(password))
            {
                userFound = false;
                ViewMenu();
            }
        }
        else
        {
            txtError.text = "Usuario Invalido";
        }
    }
    private IEnumerator CheckUserCoroutineRegister()
    {
        string username = inputUserRegister.text;
        string email = inputEmaillRegister.text;
        string password = inputPaswordRegister.text;
        
        yield return CheckUser(username);
        
        if (!userFound)
        {
            User user = new User(username,password,email);
            string json = JsonUtility.ToJson(user);
            mDataBaseR.Child("Users").Child(username).SetRawJsonValueAsync(json);
        }
        else
        {
            txtError.text = "usuario registrado.";
        }
    }

    private IEnumerator CheckUser(string username)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("Users")
            .GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            // Handle the error...
            txtError.text = "Error.";
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
            // Do something with snapshot...

            foreach (var user in snapshot.Children)
            {
                User userCheck = JsonUtility.FromJson<User>(user.GetRawJsonValue());

                if (userCheck.username.Equals(username))
                {
                    userReturn = userCheck;
                    userFound = true;
                    break;
                }
            }
        }
    }

    //View 
    public void ViewLogin()
    {
        m_menuUI.SetActive(false);
        m_RegisterUI.SetActive(false);
        m_loginUI.SetActive(true);
    }

    public void ViewRegister()
    {
        m_menuUI.SetActive(false);
        m_loginUI.SetActive(false);
        m_RegisterUI.SetActive(true);
    }

    public void ViewMenu()
    {
        m_loginUI.SetActive(false);
        m_RegisterUI.SetActive(false);
        m_menuUI.SetActive(true);
    }
    

}
public class User {
    public string username;
    public string password;
    public string email;
    public bool gender;
    public int score;
    public int level;


    public User() {
    }

    public User(string username, string password, string email, bool gender = false, int score = 0, int level = 1) {
        this.username = username;
        this.email = email;
        this.password = password;
        this.gender = gender;
        this.score = score;
        this.level = level;

    }
}
