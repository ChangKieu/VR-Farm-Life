using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI cornSeedText;
    [SerializeField] private TextMeshProUGUI beanSeedText;
    [SerializeField] private TextMeshProUGUI pumpkinSeedText;

    private void Start()
    {
        UpdateSeedTexts();
    }

    public void UpdateSeedTexts()
    {
        cornSeedText.text = SeedInventory.GetSeedCount("Corn").ToString();
        beanSeedText.text = SeedInventory.GetSeedCount("Bean").ToString();
        pumpkinSeedText.text = SeedInventory.GetSeedCount("Squash").ToString();
    }
}
