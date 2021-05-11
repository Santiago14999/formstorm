using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public enum MessageType { Message, Error }

    [System.Serializable]
    public class Message
    {
        public MessageType messageType;
        public Color color;
    }

    [SerializeField] private TextMeshProUGUI _toastMessage;
    [SerializeField] private float _transitionTime = .5f;
    [SerializeField] private float _toastTime = 1f;
    [SerializeField] private float _showPosition = 50f;
    [SerializeField] private float _hidePosition = -50f;
    [SerializeField] private Message[] _messages;

    private Dictionary<MessageType, Color> _messageColors;
    private Coroutine _toastCoroutine;

    public static ToastManager Instance;
    private void Awake()
    {
        Instance = this;
        _messageColors = new Dictionary<MessageType, Color>();
        foreach(var message in _messages)
        {
            _messageColors.Add(message.messageType, message.color);
        }
    }

    public void Toast(string message, MessageType messageType)
    {
        if (_toastCoroutine != null)
        {
            StopCoroutine(_toastCoroutine);
        }

        _toastMessage.text = message;
        _toastMessage.color = _messageColors[messageType];
        _toastCoroutine = StartCoroutine(ToastCoroutine());
    }

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }

    IEnumerator ToastCoroutine()
    {
        var position = _toastMessage.rectTransform.anchoredPosition;
        position.y = _hidePosition;
        _toastMessage.rectTransform.anchoredPosition = position;

        float startTime = Time.time;
        while(Time.time < startTime + _transitionTime)
        {
            position.y = Mathf.Lerp(_hidePosition, _showPosition, (Time.time - startTime) / _transitionTime);
            _toastMessage.rectTransform.anchoredPosition = position;
            yield return null;
        }

        position.y = _showPosition;
        _toastMessage.rectTransform.anchoredPosition = position;

        yield return new WaitForSeconds(_toastTime);

        startTime = Time.time;
        while (Time.time < startTime + _transitionTime)
        {
            position.y = Mathf.Lerp(_showPosition, _hidePosition, (Time.time - startTime) / _transitionTime);
            _toastMessage.rectTransform.anchoredPosition = position;
            yield return null;
        }

        position.y = _hidePosition;
        _toastMessage.rectTransform.anchoredPosition = position;

        _toastCoroutine = null;
    }
}
