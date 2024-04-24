using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager Instance;
    public FirebaseUser User;

    public FirebaseAuth FirebaseAuth;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FirebaseManager.Instance.OnFirebaseInit.AddListener(FirebaseAuthInit);

    }

    private void OnDestroy()
    {
        FirebaseManager.Instance.OnFirebaseInit.RemoveListener(FirebaseAuthInit);
    }

    private void FirebaseAuthInit()
    {
        FirebaseAuth = FirebaseAuth.DefaultInstance;
        FirebaseAuth.StateChanged += AuthStateChange;
        AuthStateChange(this, null);
        Debug.Log("FIREBASE AUTH INIT");
    }

    public void AuthStateChange(object sender, System.EventArgs eventArgs)
    {
        if (FirebaseAuth.CurrentUser != User)
        {
            bool signedIn = User != FirebaseAuth.CurrentUser && FirebaseAuth.CurrentUser != null;

            if (!signedIn && User != null)
            {
                Debug.Log("Player signed out");
            }

            User = FirebaseAuth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Player signed in " + User.DisplayName);
            }
        }
    }

    public void LoginWithEmail(string email, string password)
    {
        StartCoroutine(LoginWithEmailCorutine(email, password));
    }

    public IEnumerator LoginWithEmailCorutine(string email, string password)
    {
        Credential credential = EmailAuthProvider.GetCredential(email, password);

        var loginTask = FirebaseAuth.SignInWithCredentialAsync(credential);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.MissingEmail:
                    PopupManager.Instance.AddText(0);
                    Debug.LogError("Missing Email");
                    break;
                case AuthError.InvalidEmail:
                    PopupManager.Instance.AddText(2);
                    Debug.LogError("Invalid Email");
                    break;
                case AuthError.MissingPassword:
                    PopupManager.Instance.AddText(3);
                    Debug.LogError("Missing Password");
                    break;
                case AuthError.WrongPassword:
                    PopupManager.Instance.AddText(4);
                    Debug.LogError("Wrong Password");
                    break;
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }

            // TODO dodaj logiku za UI
        }
        else
        {
            PopupManager.Instance.AddText(7);
            User = loginTask.Result;
            Debug.Log("User signed in : " + User.DisplayName + " , " + User.Email);

            ScenesController.Instance.SceneLoad(3);
        }
    }

    public void RegisterWithEmail(string email, string password, string username)
    {
        StartCoroutine(RegisterWithEmailCorutine(email, password, username));
    }

    public IEnumerator RegisterWithEmailCorutine(string email, string password, string username)
    {
        var registerTask = FirebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.InvalidEmail:
                    Debug.LogError("Invalid Email");
                    break;
                case AuthError.EmailAlreadyInUse:
                    Debug.LogError("Email Already In Use");
                    PopupManager.Instance.AddText(1);
                    break;
                case AuthError.MissingPassword:
                    PopupManager.Instance.AddText(3);
                    Debug.LogError("Missing Password");
                    break;
                case AuthError.WeakPassword:
                    Debug.LogError("Weak Password");
                    break;
                case AuthError.MissingEmail:
                    PopupManager.Instance.AddText(0);
                    Debug.LogError("Missing Email");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }

            // TODO dodaj logiku za UI
        }
        else
        {
            PopupManager.Instance.AddText(6);
            Debug.Log("Register user set");
            User = registerTask.Result;

            if (User != null)
            {
                UserProfile profile = new UserProfile
                {
                    DisplayName = username

                    //  ovde moze kod za sliku
                };

                var profileTask = User.UpdateUserProfileAsync(profile);

                yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                if (profileTask.Exception != null)
                {
                    FirebaseException firebaseException = (FirebaseException)profileTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;

                    switch (error)
                    {
                        case AuthError.Cancelled:
                            Debug.LogError("Cancelled");
                            break;
                        case AuthError.SessionExpired:
                            Debug.LogError("Session Expired");
                            break;
                        default:
                            Debug.LogError("Uknown error");
                            break;
                    }

                    // TODO dodaj logiku za UI
                }
                else
                {
                    UserValidationUI.Instance.OpenLoginForm();

                    Debug.Log("Firebase user create successfully : " + User.DisplayName + " - " + User.UserId);


                    FirebaseDatabaseManager.Instance.SavePlayerDatabase(PlayerProfile.Instance.Player);
                }
            }
        }
    }

    // TODO povezi sa dugme
    public void UpdatePlayerUsername(string username)
    {
        StartCoroutine(UpdateUsernameAuth(username));
        StartCoroutine(FirebaseDatabaseManager.Instance.UpdateUsernameDatabase(username));
    }

    public IEnumerator UpdateUsernameAuth(string username)
    {
        UserProfile profile = new UserProfile { DisplayName = username };

        var profileTask = User.UpdateUserProfileAsync(profile);

        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

        if (profileTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)profileTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.Cancelled:
                    Debug.LogError("Changing Username Cancelled");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else 
        {
            Debug.Log("Username changed in auth");
        }
    }

    //public IEnumerator SendVerificationEmail()
    //{
    //    if (User != null)
    //    {
    //        Debug.Log(User.Email);
    //        var emailTask = User.SendEmailVerificationAsync();

    //        yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

    //        if (emailTask.Exception != null)
    //        {
    //            FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
    //            AuthError error = (AuthError)firebaseException.ErrorCode;

    //            switch (error)
    //            {
    //                case AuthError.Cancelled:
    //                    Debug.LogError("Verification Email Cancelled");
    //                    break;
    //                case AuthError.InvalidRecipientEmail:
    //                    Debug.LogError("Invalid Recipient Email");
    //                    break;
    //                case AuthError.TooManyRequests:
    //                    Debug.LogError("Too Many Requests");
    //                    break;
    //                default:
    //                    Debug.LogError("Uknown error");
    //                    break;
    //            }

    //            // TODO dodaj logiku za UI
    //        }
    //        else
    //        {
    //            // TODO dodaj logiku za UI

    //            Debug.Log("Email sent successfully");
    //        }
    //    }
    //    else 
    //    {
    //        Debug.LogError("User not selected! (email varification)");
    //    }
    //}
}
