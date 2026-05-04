using UnityEngine;

	[System.Serializable]
	public class DialogueSpeaker
	{
		public string name;
		public Sprite icon;
	}

	[System.Serializable]
	public class DialogueLine
	{
		public DialogueSpeaker speaker;
		[TextArea(3, 10)]
		public string text;
	}

	[System.Serializable]
	public class Dialogue
	{ 
		public DialogueLine[] dialogueLines;
	}


    public class DialogueScript : MonoBehaviour
	{
        public Dialogue dialogue;
    }

    

