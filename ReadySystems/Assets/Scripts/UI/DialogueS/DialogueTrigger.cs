using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	private DialogueScript dialogueScript;
	private Dialogue dialogue;

    private void Start()
	{
		dialogueScript = GetComponent<DialogueScript>();
		dialogue = dialogueScript.dialogue;
    }
	public void TriggerDialogue()
	{
		DialogueManager.Instance.StartDialogue(dialogue);
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (!DialogueManager.Instance.isDialogueActive)
			{
				TriggerDialogue();
			}
			else
			if (DialogueManager.Instance.isTyping)
			{
				DialogueManager.Instance.SkipTyping();
			}
			else
			{
				DialogueManager.Instance.UpdateDialogue();
            }
        }
    }
}

