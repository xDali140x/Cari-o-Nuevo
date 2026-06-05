using UnityEngine;

public class Radio : MonoBehaviour
{
    [Header("Triggers")]
    public Collider sourceTrigger;      // antena

    public Collider targetTrigger;      // radio
    public GameObject objectToDisappear;
    public GameObject objectToAppear;


    private void Start()
    {

    }

    private void Update()
    {
      
        if (sourceTrigger == null || targetTrigger == null || objectToDisappear == null)
            return;

        bool triggersTouching = sourceTrigger.enabled &&
                                targetTrigger.enabled &&
                                sourceTrigger.bounds.Intersects(targetTrigger.bounds);

        if (triggersTouching)
        {
            objectToAppear.SetActive(true);
            objectToDisappear.SetActive(false);
            Debug.Log("SE ESTAN TOCANDO");
        }
       
    }


}