using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    //Script for drag and drop functionality with support for single, double, right, and middle clicks
    //Usage: Attach this script to a UI element for drag and drop behavior

    [Header("References")]
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    public Transform dropBoxParent;

    [Header("Settings")]
    [SerializeField] private float doubleClickTime = 0.3f;
    private float lastLeftClickTime = -1f;
    private Vector2 originalAnchoredPosition;
    private bool isDragging = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
            
        isDragging = true;

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;
        }

        //Add logic for when dragging starts
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        //Add logic for while dragging
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        isDragging = false;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        if (transform.parent == canvas.transform)
        {
            //Add logic for when the object is dropped outside of any valid drop area
        }

        //Add logic for when dragging ends

        //If using a drop box
        if(eventData.pointerEnter != null)
        {
            var dropBox = eventData.pointerEnter.GetComponent<DragDropDropBox>();
            if (dropBox != null)
            {
                dropBox.OnDrop(gameObject);
                return;
            }
            var dropBoxParent = eventData.pointerEnter.GetComponentInParent<DragDropDropBox>();
            if (dropBoxParent != null)
            {
                dropBoxParent.OnDrop(gameObject);
                return;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                HandleLeftClick();
                break;

            case PointerEventData.InputButton.Right:
                HandleRightClick();
                break;

            case PointerEventData.InputButton.Middle:
                HandleMiddleClick();
                break;
        }
    }

    private void HandleLeftClick()
    {
        if (Time.time - lastLeftClickTime <= doubleClickTime)
        {
            OnDoubleLeftClick();
            lastLeftClickTime = -1f;
            return;
        }

        lastLeftClickTime = Time.time;

        //Add logic for single left click
    }

    private void HandleRightClick()
    {
        //Add logic for right click
    }

    private void HandleMiddleClick()
    {
        //Add logic for middle click
    }

    private void OnDoubleLeftClick()
    {
        //Add logic for double left click
    }

    public void ReturnToStart()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalAnchoredPosition;

        //Add logic for returning to original position
    }
}
