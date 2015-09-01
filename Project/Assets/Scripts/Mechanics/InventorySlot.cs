using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Inventory, Craft, Container}

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public SlotType Type;

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(item == null)
        {
            DragHandeler.itemBeginDragged.transform.SetParent(transform);
        }
        else
        {
            if (DragHandeler.itemBeginDragged == null)
                return;
            //Switching the objects
            if(Type == DragHandeler.staticType)
            {
                //Swithing items in the grid:
                if(Type == SlotType.Craft)
                {
                    if (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().isSplitting)
                    {
                        if (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Count > 1)
                        {
                            Debug.Log("isSplitting");
                            item.SetActive(true);
                            CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)] = (Item)DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Clone();
                            //Splitting the stack in 2:
                            //New one
                            CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].Count /= 2;

                            //Old one
                            if (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Count % 2 == 0)
                                CraftUI.ItemsInCraftTable[DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().ID].Count /= 2;
                            else
                            {
                                CraftUI.ItemsInCraftTable[DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().ID].Count /= 2;
                                CraftUI.ItemsInCraftTable[DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().ID].Count++;
                            }

                            item.GetComponent<DragHandeler>().image.sprite = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].InventorySprite;
                        }
                        return;
                    }
                    else
                    {
                        Item tmp = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)];
                        CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)] = DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem;
                        CraftUI.ItemsInCraftTable[int.Parse(DragHandeler.itemBeginDragged.transform.parent.name)] = tmp;
                    }
                }
                item.transform.SetParent(DragHandeler.itemBeginDragged.transform.parent);
                DragHandeler.itemBeginDragged.transform.SetParent(transform);
            }

            //If inventory to craft table :
            if(Type == SlotType.Craft && DragHandeler.staticType == SlotType.Inventory)
            {
                Item draggedItem = DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem;
                //Check if the item is not already in the list
                if (!CraftUI.CheckIfItemAlreadyPresent(draggedItem))
                {
                    item.SetActive(true);
                    Debug.Log("Inventory to CraftUI => ID: " + int.Parse(item.transform.parent.name));
                    CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)] = draggedItem;
                    item.GetComponent<DragHandeler>().currentItem = draggedItem;
                    item.GetComponent<DragHandeler>().image.sprite = draggedItem.InventorySprite;
                }
            }
        }
    }
}
