using UnityEngine;

public class ToolTipSettings : MonoBehaviour
{
    //Setting for tooltip position based on mouse position
    //Usage: Attach this script to the tooltip GameObject

    private RectTransform rectTransform;
    [SerializeField]
    private Vector2 offset = new Vector2(20f, -20f);

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

        float nx = rectTransform.rect.width / Screen.width;
        float ny = rectTransform.rect.height / Screen.height;

        if (pivotX > 0.5f)
        {
            pivotX = pivotX + nx;
        }
        else
        {
            pivotX = pivotX - nx;
        }

        if (pivotY > 0.5f)
        {
            pivotY += ny;
        }
        else
        {
            pivotY -= ny;
        }

        pivotX = Mathf.Clamp(pivotX, 0f, 1f);
        pivotY = Mathf.Clamp(pivotY, 0f, 1f);

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = adjustedPos;
    }

}
