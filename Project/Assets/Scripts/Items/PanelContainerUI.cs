using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelContainerUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject[] Buttons;
    private static GameObject[] _buttons;
    private static Image[] _images;
    public GameObject NextButton;
    private static GameObject _nextButton;
    public GameObject PreviousButton;
    private static GameObject _previousButton;
    private static GameObject _panel;
    public Text ContainerName;
    private static Text _containerName;
    private static int _actualPage = 0;

    private static ItemContainer _itemContainer = null;
    private bool _onHoovering = false;

    void Awake()
    {
        _panel = panel;
        _buttons = new GameObject[Buttons.Length];
        _images = new Image[Buttons.Length];
        for(int i = 0; i < Buttons.Length; i++)
        {
            _buttons[i] = Buttons[i];
            _images[i] = Buttons[i].transform.GetChild(0).GetComponent<Image>();
        }
        _nextButton = NextButton;
        _previousButton = PreviousButton;
        _containerName = ContainerName;
    }

    public void Exit()
    {
        _actualPage = 0;
        panel.SetActive(false);
    }

    public void NextPage()
    {
        _actualPage++;
        _previousButton.SetActive(true);
        UpdateButtonAtPage(_actualPage);
        //Check if it's the last page
        if (_actualPage * _buttons.Length + _buttons.Length >= _itemContainer.Content.Length)
            _nextButton.SetActive(false);
    }

    public void PreviousPage()
    {
        _actualPage--;
        if (_actualPage == 0)
            _previousButton.SetActive(false);
        _nextButton.SetActive(true);
        UpdateButtonAtPage(_actualPage);
    }


    public void SelectedItem(int index)
    {
        int contentIndex = _buttons.Length * _actualPage + index;
        Debug.Log(contentIndex);
        if (_itemContainer.Content[contentIndex].Name != "Money")
            Inventory.AddItemToInventory(_itemContainer.Content[contentIndex]);
        else
        {
            Inventory.Money += _itemContainer.Content[contentIndex].Value;
            GlobalEventText.AddMessage(string.Format("You get {0} gold !", _itemContainer.Content[contentIndex].Value));
            Inventory.RefreshUI();
        }
        _itemContainer.Content[contentIndex] = null;
        
        Debug.Log("Removed item n°" + contentIndex);
        _images[index].sprite = null;
        _buttons[index].SetActive(false);

        //Check if all the content of this page is gone
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].activeInHierarchy)
            {
                Debug.Log("This page still containing items");
                return;
            }
        }
        //Actual page empty !
        //If we aren't at the page 0
        if(_actualPage > 0)
        {
            do
            {
                _actualPage--;
                int start = _actualPage * _buttons.Length;
                int end = start + _buttons.Length;
                //Check if the page -1 contains items:
                for (int i = start; i < end; i++)
                {
                    if (_itemContainer.Content[i] != null)
                    {
                        UpdateButtonAtPage(_actualPage);
                        if (_actualPage == 0)
                            _previousButton.SetActive(false);
                        Debug.Log("Switch to page " + _actualPage);
                        return;
                    }
                }
            } while (_actualPage > 0);
        }
        //If we are at the page 0 => check if the content list is empty:
        //Looking for a page with some content
        for(int i = 0; i < _itemContainer.Content.Length; i++)
        {
            if(_itemContainer.Content[i] != null)
            {
                _actualPage = (int)(i / Buttons.Length);
                UpdateButtonAtPage(_actualPage);
                _previousButton.SetActive(false);
                //Checking if we can keep the next button on:
                if ((_actualPage + 1) * _buttons.Length >= _itemContainer.Content.Length)
                    _nextButton.SetActive(false);
                Debug.Log("Moving to the page " + _actualPage);
                return;
            }
        }

        //Nothing left, we can destroy the object:
        Inventory.RemoveItemFromInventory(_itemContainer);
        InventoryUI.OnHoovering = false;
        InventoryPopupInfos.Hide();
        Debug.Log("Container is empty");
        Exit();
    }


    public void OnPointerEnter(int index)
    {
        int contentIndex = _buttons.Length * _actualPage + index;
        if (contentIndex >= _itemContainer.Content.Length)
            return;
        if (_itemContainer.Content[contentIndex] == null)
            return;
        InventoryPopupInfos.Show(_itemContainer.Content[contentIndex].Name, _itemContainer.Content[contentIndex].Count);
        _onHoovering = true;
    }
    public void OnPointerExit()
    {
        if (_onHoovering)
        {
            InventoryPopupInfos.Hide();
            _onHoovering = false;
        }
    }

    void Update()
    {
        if (_onHoovering)
        {
            InventoryPopupInfos.ChangePosition();
        }
    }

    public static void StaticExit()
    {
        _panel.SetActive(false);
    }

    public static void OpenPanelContainerUI(ItemContainer container)
    {
        _panel.SetActive(true);
        _itemContainer = container;
        _containerName.text = _itemContainer.Name;
        //Enable or disable previous/next buttons:
        if (_itemContainer.Content.Length >= _buttons.Length)
            _nextButton.SetActive(true);
        else
            _nextButton.SetActive(false);
        _previousButton.SetActive(false);
        UpdateButtonAtPage(0);
    }

    private static void UpdateButtonAtPage(int pageID)
    {
        int start = pageID * _buttons.Length;
        int end = start + _buttons.Length;
        int cnt = 0;
        for (int i = start; i < end; i++)
        {
            if (i < _itemContainer.Content.Length && _itemContainer.Content[i] != null)
            {
                _buttons[cnt].SetActive(true);
                _images[cnt].sprite = _itemContainer.Content[i].InventorySprite;
                _images[cnt].transform.GetChild(0).GetComponent<Text>().text = _itemContainer.Content[i].Count.ToString();
            }
            else
                _buttons[cnt].SetActive(false);
            cnt++;
        }
    }
}
