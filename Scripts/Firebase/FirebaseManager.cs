using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;


public class FirebaseManager : MonoBehaviour
{
    public FirebaseApp FirebaseApp;

    [HideInInspector] public UnityEvent OnFirebaseInit = new UnityEvent();

    public static FirebaseManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependacies());
    }

    private IEnumerator CheckAndFixDependacies() 
    {
        var checkAndFixDependaciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependaciesTask.IsCompleted);

        var dependacyResult = checkAndFixDependaciesTask.Result;

        if (dependacyResult == DependencyStatus.Available)
        {
            OnFirebaseInit.Invoke();
            //StartCoroutine(CheckAutoLogin());

            Debug.Log("Firebase init");
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependacy : " + dependacyResult);
        }
    }


    private IEnumerator CheckAutoLogin() 
    {
        yield return new WaitForEndOfFrame();

        FirebaseUser user = FirebaseAuthManager.Instance.User;

        if (user != null)
        {
            var reloadTask = user.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadTask.IsCompleted);

            AutoLogin();
        }
        else 
        { 
            // salji ga na login ekran
        }
    }

    private void AutoLogin() 
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        if (user != null)
        {
            // TO DO email varification
            // ostala logika za auto logovanje
        }
        else 
        {
            //  salji ga na login ekran
        }
    }


}
