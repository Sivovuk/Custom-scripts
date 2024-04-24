using Facebook.Unity;
using Facebook.Unity.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacebookLogic : MonoBehaviour
{
    private static string applink = "";

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    Debug.LogError("Couldn't initialized");
                }
            },
                isGameShown =>
                {
                    if (!isGameShown)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                }
            );


        }
        else 
        {
            FB.ActivateApp();
        }
    }

    public void FacebookLogin() 
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions);
        Debug.Log("Facebook login");
    }

    public void FacebookLogout() 
    {
        FB.LogOut();
        Debug.Log("Facebook logout");
    }

    public void ShareLink() 
    {
        FB.ShareLink(new System.Uri("https://play.google.com"), 
            "Check it out", 
            "Text 2", 
            new System.Uri("https://play.google.com/console/u/0/developers/7680067494359301884/app/4974070177842943920/app-dashboard?timespan=thirtyDays")
            );
        Debug.Log("Link shared");
    }

    public void FacebookGameRequest()
{
        FB.AppRequest("Let's play", title : "Ludo star game");
        Debug.Log("Facebook Game Request sent");
    }

    public void GetFriendsPlayingThisGame() 
    {
        string querry = "/me/friends";
        FB.API(querry, HttpMethod.GET, result =>
        {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var frindsList = (List<object>)dictionary["data"];

            foreach (var dic in frindsList) 
            {
                Debug.Log(((Dictionary<string, object>)dic)["name"]);
            }
        });


        Debug.Log("Facebook friends list populated");
    }
}
