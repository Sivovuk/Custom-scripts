using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProfile : MonoBehaviour
{
    private PlayerData player;
    public PlayerData Player { get => player; set => player = value; }

    //  sve sto prati azuriranje igraca ide preko ovog eventa
    [HideInInspector] public UnityEvent OnPlayerUpdated = new UnityEvent();

    public static PlayerProfile Instance;


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
        player = new PlayerData();
    }

    public void LoadPlayerDataFromDatabse(PlayerData database) 
    {
        PlayerData prefs = PlayerPrefsManager.Instance.LoadPlayerPrefs();

        if (prefs.LastSave < database.LastSave)
        {
            player = database;

            PlayerPrefsManager.Instance.SavePlayerPrefs();
        }
        else if (prefs.LastSave > database.LastSave) 
        {
            player = prefs;

            FirebaseDatabaseManager.Instance.SavePlayerDatabase(player);
        }
        else
        {
            player = database;
        }

        // TODO logika za ucitavanje igraca na Ui
    }

    public void UpdateLevel(int value)
    {
        player.Level += value;

        OnPlayerUpdated.Invoke();
    }

    public void UpdateWins(int value)
    {
        player.Wins += value;

        OnPlayerUpdated.Invoke();
    }

    public void UpdateLosses(int value)
    {
        player.Losses += value;

        OnPlayerUpdated.Invoke();
    }

    public void UpdateSoloMatches(int value)
    {
        player.SoloMatches += value;

        OnPlayerUpdated.Invoke();
    }

    public void UpdateTemsMatches(int value)
    {
        player.TeamsMatches += value;

        OnPlayerUpdated.Invoke();
    }



}