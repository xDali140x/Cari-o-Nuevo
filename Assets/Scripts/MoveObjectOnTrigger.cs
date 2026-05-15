using System.Collections;
using UnityEngine;

public class MoveObjectOnTrigger : MonoBehaviour
{
    [Header("Objeto que se va a mover")]
    public GameObject objectToMove;

    [Header("Posición destino")]
    public Transform targetPosition;

    [Header("Opciones")]
    public bool matchRotation = true;
    public bool onlyOnce = false;
    public float moveSpeed = 0f; 
    // 0 = se mueve instantáneo
    // mayor a 0 = se mueve suavemente

    private bool alreadyMoved;
    private Coroutine moveCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (onlyOnce && alreadyMoved) return;
        if (targetPosition == null) return;

        GameObject targetObject = objectToMove;

        // Si no asignas objectToMove, mueve el objeto que entró al trigger
        if (targetObject == null)
        {
            if (other.attachedRigidbody != null)
                targetObject = other.attachedRigidbody.gameObject;
            else
                targetObject = other.gameObject;
        }

        MoveObject(targetObject);
        alreadyMoved = true;
    }

    private void MoveObject(GameObject obj)
    {
        if (obj == null) return;

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (moveSpeed <= 0f)
        {
            obj.transform.position = targetPosition.position;

            if (matchRotation)
                obj.transform.rotation = targetPosition.rotation;
        }
        else
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(MoveSmooth(obj.transform));
        }
    }

    private IEnumerator MoveSmooth(Transform objTransform)
    {
        while (Vector3.Distance(objTransform.position, targetPosition.position) > 0.01f)
        {
            objTransform.position = Vector3.MoveTowards(
                objTransform.position,
                targetPosition.position,
                moveSpeed * Time.deltaTime
            );

            if (matchRotation)
            {
                objTransform.rotation = Quaternion.RotateTowards(
                    objTransform.rotation,
                    targetPosition.rotation,
                    moveSpeed * 100f * Time.deltaTime
                );
            }

            yield return null;
        }

        objTransform.position = targetPosition.position;

        if (matchRotation)
            objTransform.rotation = targetPosition.rotation;
    }
}
