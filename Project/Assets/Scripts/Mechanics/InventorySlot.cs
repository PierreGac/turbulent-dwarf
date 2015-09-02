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
        if (DragHandeler.itemBeginDragged == null)
            return;
        if (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>() == null)
            return;
        int ID = 0;
        if (item == null)
            DragHandeler.itemBeginDragged.transform.SetParent(transform);
        else
        {
            //Switching the objects
            #region Same type
            if (Type == DragHandeler.staticType)
            {
                #region Crafting table
                //Swithing items in the grid:
                if (Type == SlotType.Craft)
                {
                    #region Splitting
                    if (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().isSplitting)
                    {
                        if (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Count > 1)
                        {
                            item.SetActive(true);
                            //Check if the target tile is empty or the same:
                            if (CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)] != null)
                            {
                                if (CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].Name == DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Name)
                                {
                                    CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].Count += (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Count / 2);

                                    //Old one
                                    if (DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Count % 2 == 0)
                                        CraftUI.ItemsInCraftTable[DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().ID].Count /= 2;
                                    else
                                    {
                                        CraftUI.ItemsInCraftTable[DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().ID].Count /= 2;
                                        CraftUI.ItemsInCraftTable[DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().ID].Count++;
                                    }
                                    //Update the count texts:
                                    CraftUI.RefreshUI();
                                }
                                else
                                    return;
                            }
                            else //The target item is null
                            {
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

                                //item.GetComponent<DragHandeler>().image.sprite = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].InventorySprite;

                                CraftUI.RefreshUI();
                            }
                        }
                        return;
                    }
                    #endregion
                    #region Normal
                    else
                    {
                        Item tmp = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)];
                        Debug.Log(tmp + "  " + DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem);
                        if (tmp != null && tmp.Name == DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Name) //Different items
                        {
                            if (item.transform.parent == DragHandeler.itemBeginDragged.transform.parent)
                                return;
                            Debug.Log("Merge");
                            //Same item => merging counts
                            CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].Count += DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem.Count;
                            CraftUI.ItemsInCraftTable[int.Parse(DragHandeler.itemBeginDragged.transform.parent.name)] = null;
                            DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().image.sprite = ResourcesManager.instance.EmptySprite;
                            DragHandeler.itemBeginDragged.SetActive(false);
                            CraftUI.RefreshUI();
                            return;
                        }
                        else
                        {
                            Debug.Log("Normal");
                            CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)] = DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem;
                            CraftUI.ItemsInCraftTable[int.Parse(DragHandeler.itemBeginDragged.transform.parent.name)] = tmp;

                            //Switching parents:
                            item.transform.SetParent(DragHandeler.itemBeginDragged.transform.parent);
                            DragHandeler.itemBeginDragged.transform.SetParent(transform);
                            DragHandeler.itemBeginDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            return;
                        }
                    }
                    #endregion
                }
                #endregion

                Inventory.SwitchItems(DragHandeler.itemBeginDragged, item);
                DragHandeler.itemBeginDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            #endregion

            #region Inventory to Crafting table
            //If inventory to craft table :
            if (Type == SlotType.Craft && DragHandeler.staticType == SlotType.Inventory)
            {
                Item draggedItem = DragHandeler.itemBeginDragged.GetComponent<DragHandeler>().currentItem;
                //Check if the item is not already in the list
                if (!CraftUI.CheckIfItemAlreadyPresent(draggedItem) && !Input.GetKey(KeyCode.LeftShift))
                {
                    item.SetActive(true);
                    Debug.Log("Inventory to CraftUI => ID: " + int.Parse(item.transform.parent.name));
                    CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)] = (Item)draggedItem.Clone();
                    item.GetComponent<DragHandeler>().UpdateCountText(CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].Count);
                    item.GetComponent<DragHandeler>().currentItem = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)];
                    item.GetComponent<DragHandeler>().image.sprite = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].InventorySprite;
                    return;
                }
                #region Single drop
                //If there's a drag'n drop with left shift => drop + 1 item
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    item.SetActive(true);
                    ID = int.Parse(item.transform.parent.name);
                    Debug.Log("SINGLE Inventory to CraftUI => ID: " + ID);
                    //1- Check if the target is an empty item or not
                    if (CraftUI.ItemsInCraftTable[ID] == null)
                    {
                        //2- Check of we can add one more item
                        if (draggedItem.Count > CraftUI.GetCountOfItem(draggedItem.Name))
                        {
                            CraftUI.ItemsInCraftTable[ID] = (Item)draggedItem.Clone();
                            CraftUI.ItemsInCraftTable[ID].Count = 1;
                            //item.GetComponent<DragHandeler>().UpdateCountText(CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].Count);
                            //item.GetComponent<DragHandeler>().currentItem = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)];
                            //item.GetComponent<DragHandeler>().image.sprite = CraftUI.ItemsInCraftTable[int.Parse(item.transform.parent.name)].InventorySprite;
                            CraftUI.RefreshUI();
                        }
                        else
                            item.SetActive(false);
                    }
                    else
                    {
                        //2- Check if the target item is the same item:
                        if(CraftUI.ItemsInCraftTable[ID].Name == draggedItem.Name)
                        {
                            if (draggedItem.Count > CraftUI.GetCountOfItem(draggedItem.Name))
                            {
                                CraftUI.ItemsInCraftTable[ID].Count++;
                                CraftUI.RefreshUI();
                            }
                        }
                    }
                    return;
                }
                #endregion
            }
            #endregion
        }
    }
}
