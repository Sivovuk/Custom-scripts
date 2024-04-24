using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupPrefab : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public void SetupPopup(string text) 
    {
        mainText.text = text;

        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);

    }

    public void OnPopupClose() 
    {
        LeanTween.scale(gameObject, new Vector3(0,0,0), 0.5f).setOnComplete(PopupManager.Instance.PopupClosed);
    }

}
