using System;
using EasyMobile;
using UnityEngine;

public class IAPPurchaser : MonoBehaviour
{
    public void OnEnable()
    {
        IAPManager.PurchaseCompleted += BuyProduct;
        IAPManager.PurchaseFailed += BuyFailed;
    }

    private void BuyFailed(IAPProduct obj)
    {
        Debug.LogError("Purchase failed! Product - " + obj.Id);
    }

    private void BuyProduct(IAPProduct product)
    {
        
    }

    private void OnDisable()
    {
        IAPManager.PurchaseCompleted -= BuyProduct;
        IAPManager.PurchaseFailed -= BuyFailed;
    }
}
