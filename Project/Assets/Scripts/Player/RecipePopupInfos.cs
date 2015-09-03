using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipePopupInfos : MonoBehaviour
{
    public Text text;
    private static Text _text;
    private static GameObject _gameObject;
    private static Transform _transform;
    public Canvas ParentCanevas;
    private static Canvas _parentCanevas;
    private static RectTransform _rectTransform;

    public GameObject[] Buttons;
    private static GameObject[] _buttons;
    private static Image[] _images;

    public GameObject[] ResultsButtons;
    private static GameObject[] _resultsButtons;
    private static Image[] _resultsImages;


    private static Vector2 _dimensions;

    void Awake()
    {
        _gameObject = gameObject;
        _parentCanevas = ParentCanevas;
        _transform = transform;
        _text = text;
        _rectTransform = GetComponent<RectTransform>();

        _buttons = new GameObject[9];
        _images = new Image[9];
        for (int i = 0; i < 9; i++)
        {
            _buttons[i] = Buttons[i];
            _images[i] = _buttons[i].transform.GetChild(0).GetComponent<Image>();
        }

        _resultsButtons = new GameObject[3];
        _resultsImages = new Image[3];
        for (int i = 0; i < 3; i++)
        {
            _resultsButtons[i] = ResultsButtons[i];
            _resultsImages[i] = _resultsButtons[i].transform.GetChild(0).GetComponent<Image>();
        }

        gameObject.SetActive(false);
    }

    public static Vector2 Show(int index)
    {
        if (Craft.Recipes[index] != null)
        {
            Recipe recipe = Craft.Recipes[index];
            _gameObject.SetActive(true);
            Vector2 pos;

            _dimensions = new Vector2(_rectTransform.GetWidth() / 1.9f, -_rectTransform.GetHeight() / 1.9f);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanevas.transform as RectTransform, Input.mousePosition, _parentCanevas.worldCamera, out pos);

            _transform.position = _parentCanevas.transform.TransformPoint(pos + GetOffset(pos));

            //Set the buttons images:
            for (int i = 0; i < 9; i++ )
            {
                if(recipe.Items[i] == null)
                {
                    _images[i].gameObject.SetActive(false);
                }
                else
                {
                    _images[i].gameObject.SetActive(true);
                    _images[i].sprite = recipe.Items[i].InventorySprite;
                    _images[i].transform.GetChild(0).GetComponent<Text>().text = recipe.Items[i].Count.ToString();
                }
            }

            //Results:
            for (int i = 0; i < 3; i++)
            {
                if (recipe.Results[i] == null)
                {
                    _resultsImages[i].gameObject.SetActive(false);
                }
                else
                {
                    _resultsImages[i].gameObject.SetActive(true);
                    _resultsImages[i].sprite = recipe.Results[i].InventorySprite;
                    _resultsImages[i].transform.GetChild(0).GetComponent<Text>().text = recipe.Results[i].Count.ToString();
                }
            }

            _text.text = string.Format("{0}", recipe.Name);

            return pos;
        }
        return Vector2.zero;
    }

    private static Vector2 GetOffset(Vector2 pos)
    {
        bool isX = false;
        bool isY = false;
        Vector2 offset = Vector2.zero;

        Vector2 canevaSize = _parentCanevas.GetComponent<RectTransform>().GetSize();
        Vector2 dimensions = _rectTransform.GetSize();
        //Debug.Log(canevaSize.x / 2 + " : " + (pos.x + dimensions.x));
        if (pos.x + dimensions.x < canevaSize.x / 2)
            offset.x =  - dimensions.x / 1.9f;
        else
        {
            isX = true;
            offset.x =  - dimensions.x / 1.9f;
        }
        if (pos.y - dimensions.y > -canevaSize.y / 2)
            offset.y = -dimensions.y / 1.9f;
        else
        {
            isY = true;
            //offset.y = -dimensions.y / 1.9f;
        }
        if (isY || isX)
        {
            if(isX && !isY)
            {
                offset.y = - dimensions.y;
                offset.x = (dimensions.x - (canevaSize.x - pos.x) / 2) / 2;
                return offset;
            }
            if(isY && !isX)
            {
                offset.y = dimensions.y - (canevaSize.y / 2f - Mathf.Abs(pos.y));
                return offset;
            }
            if(isY && isX)
            {
                offset.y = dimensions.y;
                offset.x = (dimensions.x - (canevaSize.x - pos.x) / 2) / 2;
                return offset;
            }
        }

        return offset;
    }

    public static void Hide()
    {
        _gameObject.SetActive(false);
    }

    public static Vector2 ChangePosition()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanevas.transform as RectTransform, Input.mousePosition, _parentCanevas.worldCamera, out pos);

        _transform.position = _parentCanevas.transform.TransformPoint(pos + GetOffset(pos));
        return pos;
    }
}
