using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderNumberText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI goldText;

    public void SetupElement(string number, string username, string gold) 
    {
        orderNumberText.text = number;
        usernameText.text = username;
        goldText.text = gold;
    }
}
