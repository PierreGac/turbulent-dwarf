using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject panelPlayer;

    public GameObject[] Buttons;
    private Image[] _images;
    public Sprite EmptySprite;

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

    private Canvas _parentCanevas;
    public static bool OnHoovering = false;
    public static bool OnHooveringRecipe = false;

    private float _mass = 0;
    private int _lastSelectedItem = -1;

    void Awake()
    {
        _parentCanevas = transform.parent.GetComponent<Canvas>();
        _images = new Image[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            _images[i] = Buttons[i].transform.GetChild(0).GetComponent<Image>();
            _images[i].gameObject.SetActive(false);
        }
        _lastSelectedItem = -1;
    }

    public void OpenInventory(Item[] items)
    {
        panel.SetActive(true);
        panelPlayer.SetActive(true);
        UseButton.gameObject.SetActive(false);
        _lastSelectedItem = -1;
        ClearText();
        GlobalMoneyText.text = string.Format("Money: {0}", Inventory.Money);
        UpdateStatistics();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                _images[i].gameObject.SetActive(true);
                _images[i].transform.GetChild(0).GetComponent<Text>().text = items[i].Count.ToString();
                _images[i].sprite = items[i].InventorySprite; //Set the sprite
            }
            else
                _images[i].gameObject.SetActive(false);
                //_images[i].sprite = EmptySprite;*/
        }
        CalculateMass();
    }

    public void OnPointerEnter(int index)
    {
        if (Inventory.Items[index] == null)
            return;
        InventoryPopupInfos.Show(Inventory.Items[index].Name, Inventory.Items[index].Count);
        OnHoovering = true;
    }
    public void OnPointerExit()
    {
        if (OnHoovering)
        {
            InventoryPopupInfos.Hide();
            OnHoovering = false;
        }
    }

    void Update()
    {
        if(OnHoovering)
        {
            InventoryPopupInfos.ChangePosition();
        }
        if(OnHooveringRecipe)
        {
            RecipePopupInfos.ChangePosition();
        }
    }

    public void Exit()
    {
        if (CraftUI.isActive)
            CraftUI.StaticExit();
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
            UseButton.gameObject.SetActive(false);
            return;
        }

        //Removing item:
        if (Input.GetMouseButton(1))
        {
            GameObject item = null;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GlobalEventText.AddMessage(string.Format("You just throw away \"{0}\" (x1)", Inventory.Items[index].Name));
                item = MonoItem.CreateGameObjectFromItem(Inventory.Items[index]);
                item.GetComponent<MonoItem>().thisItem.Count = 1;
                Inventory.DecreaseCount(Inventory.Items[index], 1);
            }
            else
            {
                GlobalEventText.AddMessage(string.Format("You just throw away \"{0}\" (x{1})", Inventory.Items[index].Name, Inventory.Items[index].Count));
                item = MonoItem.CreateGameObjectFromItem(Inventory.Items[index]);
                Inventory.RemoveItemFromInventory(Inventory.Items[index]);
                //Hide the infos popup
                OnHoovering = false;
                InventoryPopupInfos.Hide();
            }


            //Check if the playe tile contains items
            if (Scene._grid[Player.CurrentIndexPosition].TileItem != null)
            {
                //Check if the existing item is not a container
                Item[] items;
                Item existing = Scene._grid[Player.CurrentIndexPosition].TileItem.GetComponent<MonoItem>().thisItem;
                if (existing.GType != GlobalType.Container)
                {
                    Debug.Log("Wrap items");
                    //Wrap all items in a bag:
                    Bag01 bag = new Bag01(Scene._grid[Player.CurrentIndexPosition].TileItem);
                    items = new Item[2];
                    items[0] = existing;
                    items[1] = item.GetComponent<MonoItem>().thisItem;
                    bag.SetContent(items);
                    Scene._grid[Player.CurrentIndexPosition].TileItem.GetComponent<MonoItem>().thisItem = bag;

                    Scene._grid[Player.CurrentIndexPosition].ItemValue = bag.ItemValue;

                    Scene._grid[Player.CurrentIndexPosition].TileItem.GetComponent<SpriteRenderer>().sprite = bag.InGameSprite;

                    Destroy(item);
                }
                else
                {
                    Debug.Log("Add content");
                    ItemContainer container = (ItemContainer)existing;
                    items = new Item[container.Content.Length + 1];
                    //Get the existing items
                    for(int i = 0; i < container.Content.Length; i++)
                    {
                        items[i] = container.Content[i];
                    }
                    items[container.Content.Length] = (Item)item.GetComponent<MonoItem>().thisItem.Clone(); //Add the new item
                    container.SetContent(items); //Set the new content

                    Destroy(item);
                }
                
            }
            else
            {
                //Create the object in the player tile
                item.transform.position = Scene._grid[Player.CurrentIndexPosition].position;
                item.transform.SetParent(Scene._grid[Player.CurrentIndexPosition].TileObject.transform);
                Scene._grid[Player.CurrentIndexPosition].TileItem = item;
                Scene._grid[Player.CurrentIndexPosition].ItemValue = item.GetComponent<MonoItem>().thisItem.ItemValue;
            }
            return;
        }
        _lastSelectedItem = index;
        NameText.text = string.Format("Name: {0}", Inventory.Items[index].Name);
        MassText.text = string.Format("Mass: {0}", Inventory.Items[index].Mass);
        CountText.text = string.Format("Quantity: {0}", Inventory.Items[index].Count);
        DescriptionText.text = string.Format("Description: {0}", Inventory.Items[index].Description);

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
        //Inventory.ReorderInventory();
        if (fromRemove)
        {
            _lastSelectedItem = -1;
            UseButton.gameObject.SetActive(false);
        }
        if(_lastSelectedItem != -1)
            InventoryItemClick(_lastSelectedItem);
        for (int i = 0; i < Inventory.Items.Length; i++)
        {
            if (Inventory.Items[i] != null)
            {
                _images[i].gameObject.SetActive(true);
                _images[i].transform.GetChild(0).GetComponent<Text>().text = Inventory.Items[i].Count.ToString();
                _images[i].sprite = Inventory.Items[i].InventorySprite; //Set the sprite
            }
            else
            {
                _images[i].gameObject.SetActive(false);
                _images[i].sprite = ResourcesManager.instance.EmptySprite;
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
