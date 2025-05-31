using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public List<SeedItem> seedItems;            
    public GameObject seedButtonPrefab;
    public Transform seedContainer;             
    public TextMeshProUGUI goldText;                       

    public GameObject landItem;

    public LandManager landManager;

    private int basePrice = 50;
    private int multiplier = 50;
    void Start()
    {
        CurrencyManager.Instance.LoadGold();
        UpdateGoldUI();
        GenerateShopUI();
    }

    void GenerateShopUI()
    {
        foreach (SeedItem item in seedItems)
        {
            GameObject buttonGO = Instantiate(seedButtonPrefab, seedContainer);
            buttonGO.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
            switch(item.seedName)
            {
                case "Corn":
                    buttonGO.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Ngô";
                    break;
                case "Bean":
                    buttonGO.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Đậu";
                    break;
                case "Squash":
                    buttonGO.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Bí";
                    break;
                default:
                    break;
            }
            buttonGO.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.price.ToString();

            buttonGO.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => BuySeed(item));
        }

        int currentLandCount = PlayerPrefs.GetInt("LandCount", 1);
        landItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = GetLandPrice(currentLandCount).ToString();
        landItem.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => BuyLand());
    }

    void BuySeed(SeedItem seed)
    {
        if (CurrencyManager.Instance.SpendGold(seed.price))
        {
            SeedInventory.AddSeed(seed.seedName, 1);
            Debug.Log($"Đã mua: {seed.seedName}. Tổng: {SeedInventory.GetSeedCount(seed.seedName)}");
            MenuManager[] allMenus = FindObjectsOfType<MenuManager>();
            foreach (var menu in allMenus)
            {
                menu.UpdateSeedTexts();
            }
            AudioManager.Instance.PlayButtonBuy();
            UpdateGoldUI();
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }
    void BuyLand()
    {
        int currentLandCount = PlayerPrefs.GetInt("LandCount", 1);
        int price = GetLandPrice(currentLandCount);

        if (CurrencyManager.Instance.SpendGold(price))
        {
            currentLandCount++;
            PlayerPrefs.SetInt("LandCount", currentLandCount);
            PlayerPrefs.Save();

            Debug.Log("Đã mua đất mới. Tổng đất: " + currentLandCount);
            AudioManager.Instance.PlayButtonBuy();
            landManager.InstantiateSingleLand(currentLandCount - 1);
            UpdateGoldUI();
            UpdateLandPriceUI();
        }
        else
        {
            Debug.Log("Không đủ tiền để mua đất!");
        }
    }
    void UpdateLandPriceUI()
    {
        int currentLandCount = PlayerPrefs.GetInt("LandCount", 1);
        int price = GetLandPrice(currentLandCount);
        landItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =price.ToString();
    }

    void UpdateGoldUI()
    {
        goldText.text = "Gold: " + CurrencyManager.Instance.currentGold;
    }
    int GetLandPrice(int index)
    {
        int basePrice = 50;
        int multiplier = 50;
        return basePrice + (index * index * multiplier);
    }
}
