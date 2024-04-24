using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserValidationUI : MonoBehaviour
{
    public enum LoginTypes
    {
        email = 1,
        google = 2,
        facebook = 3,
        apple = 4
    }

    [Header("Email Register")]
    [SerializeField] private TextMeshProUGUI emailReg;
    [SerializeField] private TextMeshProUGUI passwordReg;
    [SerializeField] private TextMeshProUGUI varefyPassword;
    [SerializeField] private TextMeshProUGUI usernameReg;

    [Header("Email Login")]
    [SerializeField] private TextMeshProUGUI emailLogin;
    [SerializeField] private TextMeshProUGUI passwordLogin;

    [Space(10)]
    [SerializeField] private GameObject emailLoginForm;
    [SerializeField] private GameObject emailRegisterForm;

    public static UserValidationUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckValidationStateLogin(int index)
    {
        if (index == ((int)LoginTypes.email))
        {
            if (string.IsNullOrEmpty(emailLogin.text))
            {
                Debug.LogError("Enter email");
            }
            else if (string.IsNullOrEmpty(passwordLogin.text))
            {
                Debug.LogError("Enter password");
            }
            else 
            {
                Debug.Log("Login checked");
                FirebaseAuthManager.Instance.LoginWithEmail(emailLogin.text, passwordLogin.text);
            }
        }
    }

    public void CheckValidationStateRegister(int index)
    {
        if (index == ((int)LoginTypes.email))
        {
            if (string.IsNullOrEmpty(emailReg.text))
            {
                Debug.LogError("Enter email");
            }
            else if (string.IsNullOrEmpty(passwordReg.text))
            {
                Debug.LogError("Enter password");
            }
            else if (varefyPassword.text != passwordReg.text)
            {
                Debug.LogError("Passwords dont match");
            }
            else if (string.IsNullOrEmpty(usernameReg.text))
            {
                Debug.LogError("Enter password");
            }
            else
            {
                Debug.Log("Registry checked");
                FirebaseAuthManager.Instance.RegisterWithEmail(emailReg.text, passwordReg.text, usernameReg.text);
            }
        }
    }

    public void OpenLoginForm() 
    {
        emailLoginForm.SetActive(true);
        emailRegisterForm.SetActive(false);
    }
}
