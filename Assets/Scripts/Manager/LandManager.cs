using UnityEngine;
using UnityEngine.UI;

public class LandManager : MonoBehaviour
{
    [SerializeField] GameObject landPrefab;
    [SerializeField] Transform landParent, uiParent;
    [SerializeField] int columns = 3;
    [SerializeField] float spacing = 0.2f;
    [SerializeField] private GameObject plantSelectionUIPrefab;


    void Start()
    {
        int landCount = PlayerPrefs.GetInt("LandCount", 1);
        InstantiateLands(landCount);
    }

    void InstantiateLands(int count)
    {
        Vector3 landSize = GetLandSize();

        for (int i = 0; i < count; i++)
        {
            Vector3 position = GetLandPosition(i, landSize);
            GameObject land = Instantiate(landPrefab, position, Quaternion.identity, landParent);
            land.name = "Land_" + i;
            Vector3 uiPosition = new Vector3(land.transform.position.x - 0.8f, land.transform.position.y + 1f, land.transform.position.z);
            GameObject ui = Instantiate(plantSelectionUIPrefab, uiPosition, Quaternion.Euler(0, 90, 0), uiParent);
            ui.transform.localScale = new Vector3(0.04f, 0.04f, 0.05f);
            land.GetComponentInChildren<Land>().SetPlantSelectionUI(ui);
            Transform listPlant = ui.transform.Find("ListPlant");
            if (listPlant != null)
            {
                for (int j = 0; j < listPlant.childCount; j++)
                {
                    Button button = listPlant.GetChild(j).GetComponent<Button>();
                    if (button != null)
                    {
                        int index = j; 
                        button.onClick.AddListener(() => land.GetComponentInChildren<Land>().PlantSeed(index));
                    }
                }
            }
            ui.SetActive(false);
        }
    }
    public void InstantiateSingleLand(int index)
    {
        Vector3 landSize = GetLandSize();
        Vector3 position = GetLandPosition(index, landSize);
        GameObject land = Instantiate(landPrefab, position, Quaternion.identity, landParent);
        land.name = "Land_" + index;

        Vector3 uiPosition = new Vector3(land.transform.position.x - 0.8f, land.transform.position.y + 1f, land.transform.position.z);
        GameObject ui = Instantiate(plantSelectionUIPrefab, uiPosition, Quaternion.Euler(0, 90, 0), uiParent);
        ui.transform.localScale = new Vector3(0.04f, 0.04f, 0.05f);
        land.GetComponentInChildren<Land>().SetPlantSelectionUI(ui);

        Transform listPlant = ui.transform.Find("ListPlant");
        if (listPlant != null)
        {
            for (int j = 0; j < listPlant.childCount; j++)
            {
                Button button = listPlant.GetChild(j).GetComponent<Button>();
                if (button != null)
                {
                    int plantIndex = j;
                    button.onClick.AddListener(() => land.GetComponentInChildren<Land>().PlantSeed(plantIndex));
                }
            }
        }

        ui.SetActive(false);
    }

    Vector3 GetLandPosition(int index, Vector3 landSize)
    {
        int row = index / columns;
        int col = index % columns;

        float x = col * (landSize.x + spacing);
        float z = -row * (landSize.z + spacing);

        return new Vector3(x, 0, z);
    }

    Vector3 GetLandSize()
    {
        BoxCollider collider = landPrefab.GetComponent<BoxCollider>();
        if (collider != null)
        {
            Vector3 scaledSize = Vector3.Scale(collider.size, collider.transform.lossyScale);
            return scaledSize;
        }
        else
        {
            return new Vector3(1, 0, 1);
        }
    }

}
