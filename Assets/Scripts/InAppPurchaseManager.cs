using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class InAppPurchaseManager : MonoBehaviour,IDetailedStoreListener
{
    
    public string goldProductID = "com.DefaultCompany.Online-Services.Gold1";
    public string diamondProductID = "com.DefaultCompany.Online-Services.Diamond1";
    public string adsProductID = "com.DefaultCompany.Online-Services.Ads1";
    
    public int goldScore;
    public int diamondScore;
    public string adsStatus;
    [SerializeField] private Text diamondScoreText;
    [SerializeField] private Text goldScoreText;
    [SerializeField] private Text adsStatusText;


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Initialize Sucess");
        storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error,"InitializedFailed");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Initialize Failed");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase Failed");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        OnPurchaseFailed(product, PurchaseFailureReason.UserCancelled);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        if (product.definition.id == goldProductID)
        {
            AddGold();
        }

        else if (product.definition.id == diamondProductID)
        {
            AddDiamond();
        }

        else if (product.definition.id == adsProductID)
        {
            AddAds();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void AddGold()
    {

        Debug.Log("Gold Added");
        goldScore += 50;
        goldScoreText.text = goldScore.ToString();
        PlayerPrefs.SetInt("gold", goldScore);
        
       
    }

   
   

    
    public void AddDiamond()
    {
        Debug.Log("Diamond Added");
        diamondScore += 50;
        diamondScoreText.text = diamondScore.ToString();
        PlayerPrefs.SetInt("diamond", diamondScore);
    }

    public void AddAds()
    {
        Debug.Log("Ads Removed");
        adsStatus = "TRUE";
        adsStatusText.text = adsStatus;


    }

   



    IStoreController storeController;

    public void CheckNonConsumablePurchase()
    {
        Product product = storeController.products.WithID(adsProductID);
        if (product.hasReceipt)
        {
            adsStatusText.text = "Ads removed already";
            Debug.Log("Ads removed already");
        }
        else
        {
            Debug.Log("Spend something");
        }
    }

    public void InitializePurchase()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(goldProductID, ProductType.Consumable);
        builder.AddProduct(diamondProductID, ProductType.Consumable);
        builder.AddProduct(adsProductID, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void GoldPurchase()
    {
        storeController.InitiatePurchase(goldProductID);
    }

    public void DiamondPurchase()
    {
        storeController.InitiatePurchase(diamondProductID);
    }

    public void AdsPurchase()
    {
        storeController.InitiatePurchase(adsProductID);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializePurchase();
        goldScore = PlayerPrefs.GetInt("gold");
        goldScoreText.text = goldScore.ToString();

        diamondScore = PlayerPrefs.GetInt("diamond");
        diamondScoreText.text = diamondScore.ToString();
        CheckNonConsumablePurchase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
