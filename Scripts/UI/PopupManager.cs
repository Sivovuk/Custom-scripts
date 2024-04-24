using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    private bool isManagerActive = false;

    private List<string> texts = new List<string> 
    {
        "Email is missing, please enter again!",
        "Email is already in use, please try another one!",
        "Email is is invalid, please try again!",
        "Password is empty!",
        "Password is not correct, try again!",
        "Passwords are not same!",
        "User register",
        "User loged",
        "Player bought 99 gold coins!",
        "Player bought no ads!",
        "Player bought subscription!"
    };
    private List<string> textsToShow = new List<string>();

    [SerializeField] private PopupPrefab popup;

    public static PopupManager Instance;

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

    private void DisplayPopup() 
    {
        popup.SetupPopup(textsToShow[0]);
    }

    public void PopupClosed() 
    {
        textsToShow.RemoveAt(0);

        if (textsToShow.Count > 0)
        {
            DisplayPopup();
        }
        else 
        {
            isManagerActive = false;
        }
    }

    public void AddText(int index = -1)
    {
        
        if (index > -1)
        {
            textsToShow.Add(texts[index]);
            isManagerActive = true;
            DisplayPopup();
        }
        else 
        {
            Debug.LogError("Text not found!");
        }
    }
}