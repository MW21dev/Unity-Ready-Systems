using System.Collections.Generic;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    //Manager script for tooltips
    //Singleton pattern for easy access
    //Usage: It keeps track of all tooltip GameObjects in the scene
    //Put all tooltip GameObjects in the toolTips list in the inspector

    public static ToolTipManager Instance { get; private set; }

    [Header("ToolTips")]
    public List<GameObject> toolTips; //List of tooltip GameObjects

    private void Awake()
    {
        //Ensure only one instance exists
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
        for (int i = 0; i < toolTips.Count; i++)
        {
            if (toolTips[i] != null)
            {
                //Ensure all tooltips are inactive at start
                toolTips[i].GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
    }

}
