using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject panelPlayer;

    public GameObject[] Buttons;
    private Image[] _images;

    public Text NameText;
    public Text CountText;
    public Text MassText;
    public Text DescriptionText;

    public Text GlobalMassText;
    public Text GlobalMoneyText;
    public Text GlobalHealthText;
    public Text GlobalManaText;
    public Text GlobalStaminaText;
    public Text GlobalEatText;
    public Text GlobalDrinkText;
    public Text GlobalPlayerNameText;

    public Button UseButton;
    public Button ThrowButton;

    private float _mass = 0;
    private int _lastSelectedItem = -1;

    void Awake()
    {
        _images = new Image[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            _images[i] = Buttons[i].transform.GetChild(0).GetComponent<Image>();
        }
        _lastSelectedItem = -1;
    }

    public void OpenInventory(Item[] items)
    {
        panel.SetActive(true);
        panelPlayer.SetActive(true);
        ThrowButton.gameObject.SetActive(false);
        UseButton.gameObject.SetActive(false);
        _lastSelectedItem = -1;
        ClearText();
        GlobalMoneyText.text = string.Format("Money: {0}", Inventory.Money);
        UpdateStatistics();
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] != null)
            {
                Buttons[i].SetActive(true);
                _images[i].sprite = items[i].InventorySprite; //Set the sprite
            }
            else
            {
                _images[i].sprite = null;
                Buttons[i].SetActive(false);
            }
        }
        CalculateMass();
    }

    public void Exit()
    {
        panel.SetActive(false);
        panelPlayer.SetActive(false);
        _lastSelectedItem = -1;
    }

    public void InventoryItemClick(int index)
    {
        if (Inventory.Items[index] == null)
        {
            ClearText();
            _lastSelectedItem = -1;
            return;
        }
        _lastSelectedItem = index;
        NameText.text = string.Format("Name: {0}", Inventory.Items[index].Name);
        MassText.text = string.Format("Mass: {0}", Inventory.Items[index].Mass);
        CountText.text = string.Format("Quantity: {0}", Inventory.Items[index].Count);
        DescriptionText.text = string.Format("Description: {0}", Inventory.Items[index].Description);

        ThrowButton.gameObject.SetActive(true);
        if (Inventory.Items[index].isUsable)
        {
            switch(Inventory.Items[index].GType)
            { 
                case GlobalType.Fruits:
                case GlobalType.Vegetables:
                    UseButton.transform.GetChild(0).GetComponent<Text>().text = "Eat";
                    break;
                case GlobalType.Container:
                    UseButton.transform.GetChild(0).GetComponent<Text>().text = "Open";
                    break;
                default:
                    UseButton.transform.GetChild(0).GetComponent<Text>().text = "Use";
                break;
        }
            UseButton.gameObject.SetActive(true);
        }
        else
            UseButton.gameObject.SetActive(false);
    }
    private void ClearText()
    {
        NameText.text = "";
        MassText.text = "";
        CountText.text = "";
        DescriptionText.text = "";
    }

    public void RefreshUI(bool fromRemove = false)
    {
        Inventory.ReorderInventory();
        if (fromRemove)
        {
            _lastSelectedItem = -1;
            UseButton.gameObject.SetActive(false);
            ThrowButton.gameObject.SetActive(false);
        }
        if(_lastSelectedItem != -1)
            InventoryItemClick(_lastSelectedItem);
        for (int i = 0; i < Inventory.Items.Length; i++)
        {
            if (Inventory.Items[i] != null)
            {
                Buttons[i].SetActive(true);
                _images[i].sprite = Inventory.Items[i].InventorySprite; //Set the sprite
            }
            else
            {
                _images[i].sprite = null;
                Buttons[i].SetActive(false);
            }
        }
        GlobalMoneyText.text = string.Format("Money: {0}", Inventory.Money);
        UpdateStatistics();
        CalculateMass();
    }

    public void UpdateStatistics()
    {
        GlobalDrinkText.text = string.Format("Drink: {0}", Player.Stats.Drink);
        GlobalEatText.text = string.Format("Food: {0}", Player.Stats.Eat);
        GlobalHealthText.text = string.Format("Health: {0}", Player.Stats.Health);
        GlobalManaText.text = string.Format("Mana: {0}", Player.Stats.Mana);
        GlobalStaminaText.text = string.Format("Stamina: {0}", Player.Stats.Stamina);
        GlobalPlayerNameText.text = Player.Stats.Name;
    }

    public void UseSelectedItem()
    {
        if (_lastSelectedItem == -1)
            return;

        GlobalEventText.AddMessage(string.Format("You used \"{0}\"", Inventory.Items[_lastSelectedItem].Name));
        Inventory.Items[_lastSelectedItem].Use();
        RefreshUI();
    }

    public void ThrowSelectedItem()
    {
        if (_lastSelectedItem == -1)
            return;
        GameObject item = MonoItem.CreateGameObjectFromItem(Inventory.Items[_lastSelectedItem]);
        GlobalEventText.AddMessage(string.Format("You just throw away \"{0}\" (x{1})", Inventory.Items[_lastSelectedItem].Name, Inventory.Items[_lastSelectedItem].Count));
        Inventory.RemoveItemFromInventory(Inventory.Items[_lastSelectedItem]);
        Debug.Log(item);
        item.transform.position = Scene._grid[Player.CurrentIndexPosition].position;
        item.transform.SetParent(Scene._grid[Player.CurrentIndexPosition].TileObject.transform);

        ThrowButton.gameObject.SetActive(false);
        _lastSelectedItem = -1;
    }

    public void CalculateMass()
    {
        _mass = 0;
        foreach(Item item in Inventory.Items)
        {
            if(item != null)
            {
                _mass += item.Count * item.Mass;
            }
        }
        GlobalMassText.text = string.Format("Mass: {0}", _mass);
    }
}
