using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase;
using System;



class FirebaseDatabaseManager : MonoBehaviour
{
    public DatabaseReference FirebaseDatabase;

    public static FirebaseDatabaseManager Instance;

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
        FirebaseManager.Instance.OnFirebaseInit.AddListener(FirebaseDatabaseInit);

    }

    private void OnDestroy()
    {
        FirebaseManager.Instance.OnFirebaseInit.RemoveListener(FirebaseDatabaseInit);
    }

    private void FirebaseDatabaseInit()
    {
        FirebaseDatabase = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SavePlayerDatabase(PlayerData player)
    {
        player.LastSave = DateTime.Now;

        StartCoroutine(UpdateLevel(player.Level));
        StartCoroutine(UpdateGold(player.Gold));
        StartCoroutine(UpdateWins(player.Wins));
        StartCoroutine(UpdateLosses(player.Losses));
        StartCoroutine(UpdateSoloMatches(player.SoloMatches));
        StartCoroutine(UpdateTeamsMatches(player.TeamsMatches));
        StartCoroutine(UpdateLastSaveDate(player.LastSave));
    }

    public void UpdatePlayerUsername(string username)
    {

        StartCoroutine(UpdateUsernameDatabase(username));

    }

    public void LoadPlayerDatabase()
    {
        StartCoroutine(LoadUserData());
    }

    private IEnumerator LoadUserData()
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogError("Failed to load player data!");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.LogError("Data do not exist");

            if (PlayerPrefs.HasKey(PlayerPrefsManager.PLAYER_PROFILE_KEY))
            {
                // TODO logika za slucaj da u bazi ne postoji ali postoaji lokalno

                Debug.LogError("Data exist in prefs");
            }
            else
            {
                // TODO sve na nuli stavi i snimi igraca u bazi i lokalno

                Debug.LogError("New player created in database and in prefs");
            }
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            string level = snapshot.Child("level").Value.ToString();
            string gold = snapshot.Child("gold").Value.ToString();
            string wins = snapshot.Child("wins").Value.ToString();
            string losses = snapshot.Child("losses").Value.ToString();
            string solo = snapshot.Child("solo").Value.ToString();
            string teams = snapshot.Child("teams").Value.ToString();
            string date = snapshot.Child("lastSave").Value.ToString();

            PlayerData player = new PlayerData();

            player.Level = Int32.Parse(level);
            player.Gold = Int32.Parse(gold);
            player.Wins = Int32.Parse(wins);
            player.Losses = Int32.Parse(losses);
            player.SoloMatches = Int32.Parse(solo);
            player.TeamsMatches = Int32.Parse(teams);
            player.LastSave = DateTime.Parse(date);

            PlayerProfile.Instance.Player = player;
        }
    }

    public IEnumerator UpdateUsernameDatabase(string username)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var profileTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("username").SetValueAsync(username);

        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

        if (profileTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)profileTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
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
            Debug.Log("Username changed in database");
        }
    }

    public IEnumerator UpdateLevel(int level)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("level").SetValueAsync(level);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)DBTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                case AuthError.Cancelled:
                    Debug.LogError("Cancelled");
                    break;
                case AuthError.OperationNotAllowed:
                    Debug.LogError("Operation Not Allowed");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else
        {
            Debug.Log("Score is updated");
        }
    }

    public IEnumerator UpdateGold(int gold)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("gold").SetValueAsync(gold);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)DBTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                case AuthError.Cancelled:
                    Debug.LogError("Cancelled");
                    break;
                case AuthError.OperationNotAllowed:
                    Debug.LogError("Operation Not Allowed");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else
        {
            Debug.Log("Gold is updated");
        }
    }

    public IEnumerator UpdateWins(int wins)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("wins").SetValueAsync(wins);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)DBTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                case AuthError.Cancelled:
                    Debug.LogError("Cancelled");
                    break;
                case AuthError.OperationNotAllowed:
                    Debug.LogError("Operation Not Allowed");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else
        {
            Debug.Log("Wins is updated");
        }
    }

    public IEnumerator UpdateLosses(int losses)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("losses").SetValueAsync(losses);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)DBTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                case AuthError.Cancelled:
                    Debug.LogError("Cancelled");
                    break;
                case AuthError.OperationNotAllowed:
                    Debug.LogError("Operation Not Allowed");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else
        {
            Debug.Log("Losses is updated");
        }
    }

    public IEnumerator UpdateSoloMatches(int soloMatches)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("solo").SetValueAsync(soloMatches);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)DBTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                case AuthError.Cancelled:
                    Debug.LogError("Cancelled");
                    break;
                case AuthError.OperationNotAllowed:
                    Debug.LogError("Operation Not Allowed");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else
        {
            Debug.Log("Solo Matches is updated");
        }
    }

    public IEnumerator UpdateTeamsMatches(int teamsMatches)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("teams").SetValueAsync(teamsMatches);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)DBTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                case AuthError.Cancelled:
                    Debug.LogError("Cancelled");
                    break;
                case AuthError.OperationNotAllowed:
                    Debug.LogError("Operation Not Allowed");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else
        {
            Debug.Log("Teams Matches is updated");
        }
    }

    public IEnumerator UpdateLastSaveDate(DateTime dateTime)
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabase.Child("users").Child(user.UserId).Child("lastSave").SetValueAsync(dateTime.ToString());

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)DBTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            switch (error)
            {
                case AuthError.UserNotFound:
                    Debug.LogError("User Not Found");
                    break;
                case AuthError.Cancelled:
                    Debug.LogError("Cancelled");
                    break;
                case AuthError.OperationNotAllowed:
                    Debug.LogError("Operation Not Allowed");
                    break;
                default:
                    Debug.LogError("Uknown error");
                    break;
            }
        }
        else
        {
            Debug.Log("Last Save Date is updated");
        }
    }

}
