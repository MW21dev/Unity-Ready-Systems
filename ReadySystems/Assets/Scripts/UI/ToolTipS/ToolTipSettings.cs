using UnityEngine;

public class ToolTipSettings : MonoBehaviour
{
    //Setting for tooltip position based on mouse position
    //Usage: Attach this script to the tooltip GameObject

    private RectTransform rectTransform;
    [SerializeField]
    private Vector2 offset = new Vector2(16f, -16f);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 adjustedPos = mousePos + offset;

        float pivotX = adjustedPos.x / Screen.width;
        float pivotY = adjustedPos.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = adjustedPos;
    }
}
