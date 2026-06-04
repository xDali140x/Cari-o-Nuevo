using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    [Header("Dialogue")]
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;

    private InputDevice leftHandDevice;
    private bool previousPrimaryButtonState = false;
    private bool isPlayerInRange = false;
    private bool isTyping = false;
    private bool dialogueStarted = false;
    private int currentLineIndex = 0;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = string.Empty;
        GetLeftHandDevice();
    }

    void Update()
    {
        if (isPlayerInRange && !dialogueStarted)
        {
            ShowDialogue();
        }

        GetLeftHandDevice();
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
                    dialogueText.text = dialogueLines[currentLineIndex];
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

    public void ShowDialogue()
    {
        dialogueStarted = true;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = "";
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void NextLine()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            dialogueStarted = false;
            currentLineIndex = 0;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered dialogue range.");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueStarted = false;
            dialoguePanel.SetActive(false);
            StopAllCoroutines();
            isTyping = false;
        }
    }
}