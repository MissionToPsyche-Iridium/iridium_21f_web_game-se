using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField]
    private GameObject _messageField;

    public void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void SetMessage(string message)
    {
        _messageField.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void Accept()
    {
        Destroy(gameObject);
    }
}
