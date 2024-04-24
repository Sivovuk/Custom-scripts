using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase;
using System;
using System.Linq;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject leaderboardElement;

    private void Start()
    {
        LoadScoreboard();
    }

    public void LoadScoreboard() 
    {
        StartCoroutine(LoadScoreboardCorutine());
    }


    private IEnumerator LoadScoreboardCorutine()
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        var DBTask = FirebaseDatabaseManager.Instance.FirebaseDatabase.Child("users").OrderByChild("gold").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogError("Failed to load gold data!");
        }

        else
        {
            DataSnapshot snapshots = DBTask.Result;

            foreach (Transform child in parent.transform) 
            {
                Destroy(child.gameObject);
            }

            int counter = 0;

            foreach (DataSnapshot child in snapshots.Children.Reverse<DataSnapshot>()) 
            {
                string username = user.DisplayName.ToString();
                string gold = child.Child("gold").Value.ToString();
                counter++;

                GameObject spawn = Instantiate(leaderboardElement);
                spawn.transform.parent = parent;
                spawn.GetComponent<ScoreboardElement>().SetupElement(counter.ToString(), username, gold);
                spawn.transform.localScale = new Vector3(1,1,1);
            }
        }
    }

}
