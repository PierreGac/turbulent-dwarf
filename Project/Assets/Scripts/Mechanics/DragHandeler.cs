using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item currentItem { get; set; }

    public SlotType Type;
    private Text _countText;
    public Image image { get; set; }
    public int ID { get; set; }
    public bool isSplitting { get; set; }

    public static SlotType staticType;
    public static GameObject itemBeginDragged;
    private Vector3 _startPosition;
    private Transform _startParent;

    void Awake()
    {
        //out of bounds???
        _countText = transform.GetChild(0).GetComponent<Text>();
        image = GetComponent<Image>();
    }

    public void UpdateCountText(int count)
    {
        _countText.text = count.ToString();
    }
    public Text GetCountText()
    {
        return _countText;
    }
    //Drag handeler in the slot

    public void OnBeginDrag(PointerEventData eventData)
    {
        ID = int.Parse(transform.parent.name); //Get the ID of the current dragged item
        //Check if delete key:
        if (Input.GetKey(KeyCode.LeftControl) && Type == SlotType.Craft)
            isSplitting = true;
        else
            isSplitting = false;
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
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent != _startParent)
            transform.position = _startPosition;
        else
            itemBeginDragged.transform.position = _startPosition;
        itemBeginDragged = null;
    }
}
