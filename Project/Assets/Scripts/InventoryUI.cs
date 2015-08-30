using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel;

    public GameObject[] Buttons;
    private Image[] _images;

    public Text NameText;
    public Text CountText;
    public Text MassText;
    public Text DescriptionText;


    void Awake()
    {
        _images = new Image[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            _images[i] = Buttons[i].transform.GetChild(0).GetComponent<Image>();
        }
    }

    public void OpenInventory(Item[] items)
    {
        panel.SetActive(true);
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] != null)
            {
                _images[i].sprite = items[i].InventorySprite; //Set the sprite
            }
        }
    }

    public void Exit()
    {
        panel.SetActive(false);
    }

    public void InventoryItemClick(int index)
    {
        if (Inventory.Items[index] == null)
            return;
        NameText.text = string.Format("Name: {0}", Inventory.Items[index].Name);
        MassText.text = string.Format("Mass: {0}", Inventory.Items[index].Mass);
        CountText.text = string.Format("Quantity: {0}", Inventory.Items[index].Count);
        DescriptionText.text = string.Format("Description: {0}", Inventory.Items[index].Description);
    }
}
