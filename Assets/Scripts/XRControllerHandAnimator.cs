using UnityEngine;
using UnityEngine.InputSystem;

public class XRControllerHandAnimator : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator handAnimator;
    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference triggerAction;

    [Header("Suavizado")]
    [SerializeField, Min(1f)] private float animationSpeed = 12f;

    private static readonly int GripParameter = Animator.StringToHash("Grip");
    private static readonly int TriggerParameter = Animator.StringToHash("Trigger");

    private float currentGrip;
    private float currentTrigger;

    private void Awake()
    {
        if (handAnimator == null)
        {
            handAnimator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (handAnimator == null)
        {
            return;
        }

        float targetGrip = ReadActionValue(gripAction);
        float targetTrigger = ReadActionValue(triggerAction);

        currentGrip = Mathf.MoveTowards(
            currentGrip,
            targetGrip,
            animationSpeed * Time.deltaTime
        );

        currentTrigger = Mathf.MoveTowards(
            currentTrigger,
            targetTrigger,
            animationSpeed * Time.deltaTime
        );

        handAnimator.SetFloat(GripParameter, currentGrip);
        handAnimator.SetFloat(TriggerParameter, currentTrigger);
    }

    private static float ReadActionValue(InputActionReference actionReference)
    {
        if (actionReference == null || actionReference.action == null)
        {
            return 0f;
        }

        return Mathf.Clamp01(actionReference.action.ReadValue<float>());
    }
}