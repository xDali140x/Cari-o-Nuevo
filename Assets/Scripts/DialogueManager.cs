// DialogueManager.cs — ponlo en un GameObject vacío en la escena
using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;  // Singleton

    [Header("UI")]
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float cooldownTime = 3f;

    private InputDevice leftHandDevice;
    private bool previousPrimaryButtonState = false;
    private bool isTyping = false;
    private bool isOnCooldown = false;
    private bool dialogueActive = false;

    private string[] currentLines;
    private int currentLineIndex = 0;

    void Awake()
    {
        // Solo existe una instancia del Manager
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        GetLeftHandDevice();
    }

    void Update()
    {
        GetLeftHandDevice();

        if (dialogueActive)
            HandleButtonInput();
    }

    void GetLeftHandDevice()
    {
        if (!leftHandDevice.isValid)
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    void HandleButtonInput()
    {
        bool primaryButtonPressed = false;

        if (leftHandDevice.isValid &&
            leftHandDevice.TryGetFeatureValue(
                CommonUsages.primaryButton, out primaryButtonPressed))
        {
            if (primaryButtonPressed && !previousPrimaryButtonState)
            {
                if (!isTyping)
                    NextLine();
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = currentLines[currentLineIndex];
                    isTyping = false;
                }
            }
            previousPrimaryButtonState = primaryButtonPressed;
        }
        else
        {
            previousPrimaryButtonState = false;
        }
    }

    // Los NPCs llaman este método para iniciar su diálogo
    public void StartDialogue(string[] lines)
    {
        // Si hay un diálogo activo lo interrumpe y empieza el nuevo
        if (isOnCooldown) return;

        StopAllCoroutines();

        currentLines = lines;
        currentLineIndex = 0;
        isTyping = false;
        dialogueActive = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = "";

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in currentLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        if (currentLineIndex < currentLines.Length - 1)
        {
            currentLineIndex++;
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(DialogueCooldown());
        }
    }

    IEnumerator DialogueCooldown()
    {
        dialoguePanel.SetActive(false);
        dialogueActive = false;
        isOnCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        currentLineIndex = 0;
        dialogueText.text = "";
        isOnCooldown = false;

        Debug.Log("DialogueManager reseteado.");
    }

    public void ForceClose()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        dialogueActive = false;
        isTyping = false;
        dialogueText.text = "";
    }

    public bool IsAvailable() => !dialogueActive && !isOnCooldown;
}