using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryPopupInfos : MonoBehaviour
{
    public Text text;
    private static Text _text;
    private static GameObject _gameObject;
    private static Transform _transform;
    public Canvas ParentCanevas;
    private static Canvas _parentCanevas;
    private static RectTransform _rectTransform;

    private static Vector2 _dimensions;

    void Awake()
    {
        _gameObject = gameObject;
        _parentCanevas = ParentCanevas;
        _transform = transform;
        _text = text;
        _rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public static Vector2 Show(string name, int count)
    {
        _gameObject.SetActive(true);
        Vector2 pos;

        _dimensions = new Vector2(_rectTransform.GetWidth() / 1.9f,  - _rectTransform.GetHeight() / 1.9f);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanevas.transform as RectTransform, Input.mousePosition, _parentCanevas.worldCamera, out pos);
        
        _transform.position = _parentCanevas.transform.TransformPoint(pos + _dimensions);
        _text.text = string.Format("{0} (x{1})", name, count);
        _rectTransform.SetHeight(_text.preferredHeight + 4);

        return pos;
    }

    private Vector2 GetOffset(Vector2 pos)
    {
        Vector2 offset = Vector2.zero;

        Vector2 canevaSize = _parentCanevas.GetComponent<RectTransform>().GetSize();
        Vector2 dimensions = _rectTransform.GetSize();
        if (pos.x + (dimensions.x / 2) < canevaSize.x)
            offset.x = dimensions.x / 1.9f;

        return offset;
    }

    public static void Hide()
    {
        _gameObject.SetActive(false);
    }

    //TODO : Adjusting position if a part of the panel is out of bounds
    public static Vector2 ChangePosition()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanevas.transform as RectTransform, Input.mousePosition, _parentCanevas.worldCamera, out pos);

        _transform.position = _parentCanevas.transform.TransformPoint(pos + _dimensions);
        return pos;
    }
}
