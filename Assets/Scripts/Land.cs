using NUnit.Framework;
using UnityEngine;

public enum LandState
{
    Dry,        
    Farmed,    
    Planted     
}
public class Land : MonoBehaviour
{
    [SerializeField] PlantData[] listPlant;
    [SerializeField] GameObject plantPrefab, insectPrefab;
    [SerializeField] GameObject farmedLand;
    [SerializeField] Transform plantPos;
    public LandState currentState = LandState.Dry;
    [SerializeField] private int timeShovel=0;
    [SerializeField] private GameObject currentPlant;
    [SerializeField] private GameObject plantSelectionUI; 
    [SerializeField] private GameObject textPrompt;

    private void OnEnable()
    {
        ResetLand();
    }
    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shovel") && currentState == LandState.Dry)
        {
            AudioManager.Instance.PlayShovel();

            if (timeShovel <3)
            {
                timeShovel++;
            }
            else
            {
                currentState = LandState.Farmed;
                timeShovel = 0;
                farmedLand.SetActive(true);
                Debug.Log("Đất đã được xới!");
            }
            UpdateTextPrompt();
        }
        else if (other.CompareTag("Shovel") && currentState == LandState.Farmed)
        {

        }

        else if (other.CompareTag("WateringCan") && currentPlant != null)
        {
            currentPlant.GetComponent<Plant>().WaterPlant();
        }
    }

    public void PlantSeed(int type)
    {
        Debug.Log(type);
        if (currentPlant == null && SeedInventory.UseSeed(listPlant[type].plantName))
        {
            currentPlant = Instantiate(plantPrefab, plantPos.position, Quaternion.identity, this.transform);
            currentPlant.GetComponent<Plant>().plantData = listPlant[type];
            currentPlant.GetComponent<Plant>().SetInsectPrefab(insectPrefab);
            currentState = LandState.Planted;
            currentPlant.GetComponent<Plant>().landScript = this;
            plantPos.gameObject.SetActive(false);
            plantSelectionUI.SetActive(false);
            switch (type)
            {
                case 0:
                    MissionManager.Instance.AddProgress(MissionType.PlantCorn);
                    break;
                case 1:
                    MissionManager.Instance.AddProgress(MissionType.PlantBeans);
                    break;
                case 2:
                    MissionManager.Instance.AddProgress(MissionType.PlantSquash);
                    break;
                default:
                    Debug.LogError("Unknown plant type");
                    break;
            }
            UpdateTextPrompt();
        }
    }

    public void OnClickLand()
    {
        if(currentState == LandState.Farmed)
        {
            UpdateTextPrompt();
            plantSelectionUI.SetActive(true);
        }
    }
    public void TestPlant()
    {
        Debug.Log("Test");
    }
    private void UpdateTextPrompt()
    {
        switch (currentState)
        {
            case LandState.Dry:
                textPrompt.transform.parent.gameObject.SetActive(true);
                textPrompt.GetComponent<TextMesh>().text = "Đào đất " +(3 - timeShovel).ToString() +" lần!";
                break;

            case LandState.Farmed:
                textPrompt.transform.parent.gameObject.SetActive(true);
                textPrompt.GetComponent<TextMesh>().text = "Chọn cây trồng!";
                break;

            case LandState.Planted:
                textPrompt.transform.parent.gameObject.SetActive(false);
                break;
        }
    }
    public void TextOn(string text)
    {
        textPrompt.transform.parent.gameObject.SetActive(true);
        textPrompt.GetComponent<TextMesh>().text = text;
    }
    public void TextOff()
    {
        textPrompt.transform.parent.gameObject.SetActive(false);
    }
    public void ResetLand()
    {
        currentState = LandState.Dry;
        timeShovel = 0;
        if (currentPlant != null)
        {
            currentPlant = null;
        }

        farmedLand.SetActive(false);

        plantPos.gameObject.SetActive(true);

    }
    public void SetPlantSelectionUI(GameObject ui)
    {
        plantSelectionUI = ui;
    }
}
