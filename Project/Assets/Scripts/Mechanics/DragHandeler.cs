using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item currentItem { get; set; }

    public SlotType Type;
    public Image image { get; set; }
    public int ID { get; set; }
    public bool isSplitting { get; set; }

    public static SlotType staticType;
    public static GameObject itemBeginDragged;
    private Vector3 _startPosition;
    private Transform _startParent;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    //Drag handeler in the slot

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl) && Type == SlotType.Craft)
        {
            isSplitting = true;
        }
        else
            isSplitting = false;
        ID = int.Parse(transform.parent.name); //Get the ID of the current dragged item
        //Get the current item dragged:
        switch(Type)
        {
            case SlotType.Inventory:
                currentItem = Inventory.Items[ID];
                break;
            case SlotType.Craft:
                currentItem = CraftUI.ItemsInCraftTable[ID];
                break;
        }
        itemBeginDragged = gameObject;
        staticType = Type;
        _startPosition = transform.position;
        _startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("endDrag " + transform.parent.name);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent != _startParent)
        {
            transform.position = _startPosition;
        }
        else
        {
            itemBeginDragged.transform.position = _startPosition;
        }
        itemBeginDragged = null;
    }
}
