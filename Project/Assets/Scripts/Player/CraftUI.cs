using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftUI : MonoBehaviour
{
    public GameObject[] Buttons;
    private static Image[] _images;

    public static Item[] ItemsInCraftTable;
    public static Item[] ItemsInResult;

    public Sprite EmptySprite;
    private static Sprite _emptySprite;

    public GameObject panel;
    private static GameObject _panel;
	

    void Awake()
    {
        _panel = panel;
        _emptySprite = EmptySprite;
        _images = new Image[Buttons.Length];
        for(int i = 0; i < _images.Length; i++)
        {
            _images[i] = Buttons[i].transform.GetChild(0).GetComponent<Image>();
            _images[i].gameObject.SetActive(false);
        }
        ItemsInCraftTable = new Item[Buttons.Length];
        ItemsInResult = new Item[2];
    }

    public void Exit()
    {
        panel.SetActive(false);
    }

    public static void OpenCraftingUI()
    {
        _panel.SetActive(true);

        //Reset the sprites of the crafting table:
        for(int i = 0; i < _images.Length; i ++)
        {
            _images[i].gameObject.SetActive(false);
            _images[i].sprite = _emptySprite;
            ItemsInCraftTable[i] = null;
        }
    }

    public void CraftTableBTN()
    {
        for(int i = 0; i < _images.Length; i++)
        {
            if (ItemsInCraftTable[i] != null)
                Debug.Log(string.Format("Item {0} : {1} (x{2})", i, ItemsInCraftTable[i].Name, ItemsInCraftTable[i].Count));
            else
                Debug.Log(string.Format("Item {0} : {1}", i, ItemsInCraftTable[i]));
        }
    }

    public static bool CheckIfItemAlreadyPresent(Item item)
    {
        for(int i = 0; i < ItemsInCraftTable.Length; i++)
        {
            if (ItemsInCraftTable[i] == item)
                return true;
        }
        return false;
    }

    public void SplitSelectedItem()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Splitting");
        }
    }
}
