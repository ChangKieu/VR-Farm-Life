using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public ParticleSystem waterEffect;
    public Collider waterCollider;
    private bool isPouring = false;

    private void Update()
    {
        float zRotation = GetZRotation(); 

        if (zRotation < -30 && !isPouring)
        {
            StartPouring();
        }
        else if (zRotation >= -30 && isPouring)
        {
            StopPouring();
        }
    }

    void StartPouring()
    {
        AudioManager.Instance.PlayWaterCan();
        isPouring = true;
        waterEffect.Play();
        waterCollider.enabled = true;
    }

    void StopPouring()
    {
        AudioManager.Instance.PlayWaterCanEnd();
        isPouring = false;
        waterEffect.Stop();
        waterCollider.enabled = false;
    }

    float GetZRotation()
    {
        float zRot = transform.eulerAngles.z;
        if (zRot > 180) zRot -= 360; 
        return zRot;
    }
}
