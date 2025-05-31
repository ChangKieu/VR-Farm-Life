using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InstantGrab : MonoBehaviour
{
    public Transform attachPoint; // Vị trí đích khi grab
    public float grabSpeed = 10f; // Tốc độ di chuyển

    private XRGrabInteractable grabInteractable;
    private bool isBeingGrabbed = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void Update()
    {
        if (isBeingGrabbed && attachPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, attachPoint.position, Time.deltaTime * grabSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, attachPoint.rotation, Time.deltaTime * grabSpeed);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        attachPoint = args.interactorObject.transform;
        isBeingGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isBeingGrabbed = false;
    }
}
