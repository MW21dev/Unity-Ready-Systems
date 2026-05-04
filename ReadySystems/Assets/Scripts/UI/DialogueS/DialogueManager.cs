using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("References")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialogueBox;
    public Image speakerIcon1;
    private CanvasGroup canvasGroup;

    [Header("Settings")]
    public float typingSpeed = 0.2f;
    public bool isDialogueActive = false;
    public bool isTyping = false;

    [Header("Runtime")]
    public List<DialogueLine> lines;
    public int currentLineIndex = -1;


    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        lines = new List<DialogueLine>();
        canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        lines.Clear();

        lines = new List<DialogueLine>(dialogue.dialogueLines);

        UpdateDialogue();
    }

    public void UpdateDialogue()
    {
        if(currentLineIndex == lines.Count -1)
        {
            EndDialogue();
            return;
        }

        currentLineIndex++;

        DialogueLine currentLine = lines[currentLineIndex];
        nameText.text = currentLine.speaker.name;
        speakerIcon1.sprite = currentLine.speaker.icon;
    
        StopAllCoroutines();
        StartCoroutine(TypeLine(currentLine.text));
        isTyping = true;
    }

    public void SkipTyping()
    {
        StopAllCoroutines();
        if(lines.Count > 0)
        {
            DialogueLine currentLine = lines[currentLineIndex];
            dialogueText.text = currentLine.text;
            isTyping = false;
        }
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        isTyping = false;

        lines.Clear();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        currentLineIndex = -1;
    }
}

