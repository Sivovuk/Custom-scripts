using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PlayerData : MonoBehaviour
{
    public int Level;
    public int Gold;
    public int Wins;
    public int Losses;
    public int SoloMatches;
    public int TeamsMatches;

    public DateTime LastSave;

    //public PlayerData()
    //{
    //}

    //public PlayerData(int level, int wins, int losses, int soloMatches, int teamsMatches)
    //{
    //    this.Level = level;
    //    this.Wins = wins;
    //    this.Losses = losses;
    //    this.SoloMatches = soloMatches;
    //    this.TeamsMatches = teamsMatches;
    //}

}

[SerializeField]
public class PlayersData : MonoBehaviour
{
    public List<PlayerData> playersData = new List<PlayerData>();

    public PlayersData(List<PlayerData> data)
    {
        playersData = data;
    }
}


