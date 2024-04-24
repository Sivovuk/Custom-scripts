using System;
using System.Collections;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{

    public const string PLAYER_PROFILE_KEY = "playerProfile";


    public static PlayerPrefsManager Instance;

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

    public bool SavePlayerPrefs() 
    {
        PlayerProfile.Instance.Player.LastSave = DateTime.Now;

        var data = JsonUtility.ToJson(PlayerProfile.Instance.Player);

        PlayerPrefs.SetString(data, PLAYER_PROFILE_KEY);

        return true;
    }

    public PlayerData LoadPlayerPrefs() 
    {
        var data = JsonUtility.FromJson<PlayerData>(PLAYER_PROFILE_KEY);

        return data;
    }
}