using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DisableGrabOnContact : MonoBehaviour
{
    [Header("Objeto que debe tocar este trigger")]
    public GameObject objectToDetect;

    [Header("XR Grab Interactable que se va a desactivar")]
    public XRGrabInteractable grabToDisable;

    [Header("Opciones")]
    public bool makeRigidbodyKinematic = true;
    public bool stopPhysicsMovement = true;
    public bool onlyOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (onlyOnce && hasTriggered)
            return;

        if (objectToDetect == null)
        {
            Debug.LogWarning("No asignaste Object To Detect.");
            return;
        }

        bool isCorrectObject =
            other.gameObject == objectToDetect ||
            other.transform.root.gameObject == objectToDetect ||
            other.transform.IsChildOf(objectToDetect.transform);

        if (!isCorrectObject)
            return;

        DisableGrab();

        hasTriggered = true;
    }

    private void DisableGrab()
    {
        if (grabToDisable == null)
        {
            Debug.LogWarning("No asignaste Grab To Disable.");
            return;
        }

        GameObject grabbedObject = grabToDisable.gameObject;

        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();

        if (rb != null && stopPhysicsMovement)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (rb != null && makeRigidbodyKinematic)
        {
            rb.isKinematic = true;
        }

        grabToDisable.enabled = false;

        Debug.Log("XR Grab Interactable desactivado.");
    }
}