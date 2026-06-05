// NPCDialogue.cs — este va en cada NPC (reemplaza tu DialogueScript)
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    public string[] dialogueLines;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Solo inicia si el manager está libre
            if (DialogueManager.Instance.IsAvailable())
            {
                DialogueManager.Instance.StartDialogue(dialogueLines);
                Debug.Log($"{gameObject.name} inició diálogo.");
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.ForceClose();
        }
    }
}