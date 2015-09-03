using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlobalEventText : MonoBehaviour
{
    public static GlobalEventText instance = null;
    public float FadingTime = 3.0f;
    private static Text _text;
    private static float _lastTimeMessage = -1;
    private static bool _onFading = false;


    void Update()
    {
        if(_lastTimeMessage != -1 && (_lastTimeMessage + FadingTime) < Time.time)
        {
            _text.text = "";
            _lastTimeMessage = -1;
        }
    }

    void Awake()
    {
        _text = GetComponent<Text>();
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public static void AddMessage(string message)
    {
        //_text.text = string.Format("{0}{1}\r\n", _text.text, message);
        instance.StartCoroutine(instance.UpdateText(message));
    }

    private IEnumerator UpdateText(string message)
    {
        //Check if there's another fading process running
        if(_onFading)
        {
            while (_onFading)
            {
                yield return null;
            }
        }
        _onFading = true;
        for(int i = 0; i < message.Length; i++)
        {
            _text.text = string.Format("{0}{1}", _text.text, message[i]);
            _lastTimeMessage = Time.time;
            yield return new WaitForSeconds(0.02f);
        }
        _text.text = string.Format("{0}\r\n", _text.text);
        _lastTimeMessage = Time.time;
        _onFading = false;
    }
}
