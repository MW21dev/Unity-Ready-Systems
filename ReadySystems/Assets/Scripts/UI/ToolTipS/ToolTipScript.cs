using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ToolTipScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Script for tooltips
    //Usage: Attach this script to a UI element or game object
    //Call ToolTipScript.ShowToolTip(toolTipContent = your list of strings) to display the tooltip

    //Editor structure for tooltip UI:
    //UI Canvas (Set sort order to maximum value to always be on top)
    //ToolTip (Set inactive by default) (Vertical Layout Group component for automatic sizing(Control Child size Width and Height)/(Content Size Fitter component set to Preferred Size)/(Layout element to set prefered size)/(CanvasGroup))
    //Background (Image component with desired background sprite)
    //Text (Text component for displaying tooltip text)

    //ToolTip Manager storing references to tooltip components

    [Header("UI References")]
    private List<TextMeshProUGUI> textBoxes; //Reference to the Text components for displaying tooltip text
    private LayoutElement layoutElement; //Reference to the Layout Element component for controlling size
    private CanvasGroup canvasGroup; //Reference to the Canvas Group component for controlling visibility
    public GameObject toolTipObject; //Reference to the tooltip GameObject 

    [Header("Settings")]
    public int characterWrapLimit = 50; //Character limit before enabling layout element
    public int toolTipID = 0; //ID to identify which tooltip to use from the ToolTipManager
    public float showDelay = 0.5f; //Delay before showing the tooltip

    [Header("Content")]
    public List<string> toolTipContent; //Text to display in the tooltip

    [Header("Hover Settings")]
    private bool isHovering = false;


    void Start()
    {
        toolTipObject = ToolTipManager.Instance.toolTips[toolTipID];
        textBoxes = new List<TextMeshProUGUI>(toolTipObject.GetComponentsInChildren<TextMeshProUGUI>());
        layoutElement = toolTipObject.GetComponent<LayoutElement>();
        canvasGroup = toolTipObject.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (isHovering)
        {
            showDelay -= Time.unscaledDeltaTime;
            if (showDelay <= 0f)
            {
                ShowToolTip(toolTipContent);
                isHovering = false; 
            }
        }
    }


    public void ShowToolTip(List<string> content)
    {
        if (textBoxes.Count == 0)
        {
            return;
        }

        //Make tooltip visible
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;

        //Set tooltip text
        for (int i = 0; i < textBoxes.Count; i++)
        {
            if (i < content.Count)
            {
                textBoxes[i].text = content[i];
            }
            else
            {
                textBoxes[i].text = "";
            }

            if (content[i].Length > characterWrapLimit)
            {
                layoutElement.enabled = true;
            }
            else
            {
                layoutElement.enabled = false;
            }
        }
    }

    public void HideToolTip()
    {
        canvasGroup.alpha = 0f;
        showDelay = 0.5f;
        isHovering = false;
    }

    //For UI elements
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }

    //For game objects
    public void OnMouseEnter()
    {
        isHovering = true;
    }
    public void OnMouseExit()
    {
        HideToolTip();
    }
}

