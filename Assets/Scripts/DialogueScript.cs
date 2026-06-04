using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public TMP_Text dialogueText;
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;
    int currentLineIndex = 0;
    InputDevice rightHandDevice;
    bool previousPrimaryButtonState = false;
    
    void Start()
    {
        dialogueText.text = string.Empty;
        GetRightHandDevice();
        StartDialogue();
    }

    
    void Update()
    {
        GetRightHandDevice();

        bool primaryButtonPressed = false;
        if (rightHandDevice.isValid && rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed))
        {
            if (primaryButtonPressed && !previousPrimaryButtonState)
            {
                if (dialogueText.text == dialogueLines[currentLineIndex])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[currentLineIndex];
                }
            }

            previousPrimaryButtonState = primaryButtonPressed;
        }
        else
        {
            previousPrimaryButtonState = false;
        }
    }

    void GetRightHandDevice()
    {
        if (!rightHandDevice.isValid)
        {
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
    }

    public void StartDialogue()
    {
        currentLineIndex = 0;
        dialogueText.text = "";
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
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
            gameObject.SetActive(false);
        }
    }

    
}
