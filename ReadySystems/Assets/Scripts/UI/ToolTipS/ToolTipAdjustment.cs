using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipAdjustment : MonoBehaviour
{
    //Script for quick tooltip content 
    //Usage: Attach to a GameObject with a ToolTip component
    // in contents write the lines you want to show in the tooltip

    [Header("Content")]
    public List<string> contents = new List<string>();

    private ToolTipScript toolTip;

    private void Start()
    {
        toolTip = GetComponent<ToolTipScript>();
        toolTip.toolTipContent = contents;
    }
}
