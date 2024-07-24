using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaintainRotationOnGrab : MonoBehaviour
{
    private Quaternion initialRotation;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(StoreRotation);
        grabInteractable.selectExited.AddListener(ResetRotation);
    }

    private void StoreRotation(SelectEnterEventArgs args)
    {
        initialRotation = transform.rotation;
    }

    private void ResetRotation(SelectExitEventArgs args)
    {
        if (!args.isCanceled) // V�rifie si la s�lection n'est pas annul�e
        {
            transform.rotation = initialRotation;
        }
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            transform.rotation = initialRotation;
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(StoreRotation);
            grabInteractable.selectExited.RemoveListener(ResetRotation);
        }
    }
}
