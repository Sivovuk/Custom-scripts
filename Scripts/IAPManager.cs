using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    private string subscription = "com.magmadev.ludostar.subscription";
    private string noADS = "com.magmadev.ludostar.noads";
    private string coin99 = "com.magmadev.ludostar.goldcoin99";


    public void OnPurchaseComplete(Product product) 
    {
        if (product.definition.id == coin99) 
        {
            PopupManager.Instance.AddText(8);
            Debug.Log("Player bought 100 gold coins");
        }
        else if (product.definition.id == noADS)
        {
            PopupManager.Instance.AddText(9);
            Debug.Log("Player bought no ads!");
        }
        else if (product.definition.id == subscription)
        {
            PopupManager.Instance.AddText(10);
            Debug.Log("Player bought subscription");
        }
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReson) 
    {
        Debug.Log(product.definition.id + " failed because " + failureReson);
    }
}
