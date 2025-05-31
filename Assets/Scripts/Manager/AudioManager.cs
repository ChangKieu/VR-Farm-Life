using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Danh sach AudioSources")]
    [SerializeField] private AudioSource shovelSource;
    [SerializeField] private AudioSource waterCanSource;
    [SerializeField] private AudioSource harvestSource;
    [SerializeField] private AudioSource buttonCollect;
    [SerializeField] private AudioSource buttonBuy;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayShovel()
    {
        if (shovelSource != null && shovelSource.clip != null)
            shovelSource.Play();
    }

    public void PlayWaterCan()
    {
        if (waterCanSource != null && waterCanSource.clip != null)
            waterCanSource.Play();
    }
    public void PlayWaterCanEnd()
    {
        if (waterCanSource != null && waterCanSource.clip != null)
            waterCanSource.Stop();
    }

    public void PlayHarvest()
    {
        if (harvestSource != null && harvestSource.clip != null)
            harvestSource.Play();
    }

    public void PlayButton()
    {
        if (buttonCollect != null && buttonCollect.clip != null)
            buttonCollect.Play();
    }
    public void PlayButtonBuy()
    {
        if (buttonBuy != null && buttonBuy.clip != null)
            buttonBuy.Play();
    }
}
