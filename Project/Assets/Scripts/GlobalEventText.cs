using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlobalEventText : MonoBehaviour
{
    public float FadingTime = 5.0f;
    public static int MaxMessages = 5;
    private static Text _text;
    private static int _nbMessages = 0;
    private static string[] _messages;
    private static float _lastTimeMessage = -1;

    void Update()
    {
        if(_lastTimeMessage != -1 && (_lastTimeMessage + FadingTime) < Time.time)
        {
            _text.text = "";
            _nbMessages = 0;
            _lastTimeMessage = -1;
        }
    }

    void Awake()
    {
        _text = GetComponent<Text>();
        _messages = new string[MaxMessages];
    }
    public static void AddMessage(string message)
    {
        _nbMessages++;
        if (_nbMessages >= MaxMessages)
        {
            _nbMessages--;
            _text.text.Replace(_messages[0], "");
            _text.text = string.Format("{0}{1}\r\n", _text.text, message);
            _messages[0] = message;
            Debug.Log("Global event text overflow !");
        }
        else
        {
            _text.text = string.Format("{0}{1}\r\n", _text.text, message);
            _messages[_nbMessages] = message;
        }
        _lastTimeMessage = Time.time;
    }
}
