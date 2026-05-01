using UnityEngine;
using UnityEngine.UI;

public class DragDropDropBox : MonoBehaviour
{
    //Script for drop area for drag and drop functionality
    //Usage: Attach this script to a UI element to make it a drop area for drag and drop behavior

    [Header("References")]
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    [Header("Settings")]
    public bool canAcceptItems = true;
    public bool snapToCenter = true;
    public bool isEmpty;

    [Header("Purpose")]
    public bool swapItems = false; // If true, dropping an item on an occupied drop box will swap the items
    public bool stackItems = false; // If true, dropping an item on an occupied drop box will stack the items if they are the same type
    public bool deleteOnDrop = false; // If true, dropping an item on an occupied drop box will delete the existing item

    [Header("Stored Item data")]
    public GameObject storedItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        isEmpty = IsEmpty();
    }

    public bool IsEmpty()
    {
        if(gameObject.transform.childCount == 0)
        {
            return true;
        }
        return false;
    }

    public void OnDrop(GameObject droppedItem)
    {
        if (!canAcceptItems)
        {
            return;
        }

        if (!isEmpty)
        {
            if(swapItems)
            {
                SwapItems(storedItem, droppedItem);
            }
            else if(stackItems)
            {
                StackItems(storedItem, droppedItem, 1); // Assuming a quantity of 1 for simplicity, you can modify this to get the actual quantity from the item
                
            }
            else if(deleteOnDrop)
            {
                DeleteItem(storedItem, droppedItem);
            }
            else
            {
                // If none of the above options are enabled, simply return without accepting the new item
                return;
            }
        }
        else
        {
            DropItem(droppedItem);
        }


        //Add logic for when an item is dropped into the drop box
    }

    public void DropItem(GameObject droppedItem)
    {
        storedItem = droppedItem;

        RectTransform itemRect = storedItem.GetComponent<RectTransform>();
        DragDropScript itemDragScript = storedItem.GetComponent<DragDropScript>();
        itemDragScript.dropBoxParent = this.transform;
        itemRect.SetParent(rectTransform);
        itemRect.SetAsLastSibling();

        if (snapToCenter)
        {
            itemRect.anchoredPosition = Vector2.zero;
        }
    }

    public void SwapItems(GameObject storedItem, GameObject newItem)
    {
        DragDropDropBox newItemDropBox = newItem.GetComponent<DragDropScript>().dropBoxParent.GetComponent<DragDropDropBox>();
        newItemDropBox.DropItem(storedItem);

        DropItem(newItem);
    }

    public void StackItems(GameObject storedItem, GameObject newItem, int quantity)
    {
        
    }

    public void DeleteItem(GameObject storedItem, GameObject newItem)
    {
        Destroy(storedItem);

        DropItem(newItem);
    }
}
