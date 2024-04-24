using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabsController : MonoBehaviour
{
    [Header("Collections")]
    public List<GameObject> tabs = new List<GameObject>();

    public List<GameObject> objectsToDeactivate = new List<GameObject>();

    public void OpenTab(GameObject tabToOpen) 
    {
        foreach (GameObject tab in tabs) 
        {
            tab.SetActive(false) ;
        }

        foreach (GameObject obj in objectsToDeactivate) 
        {
            obj.SetActive(false);
        }

        tabToOpen.SetActive(true);
    }

}
