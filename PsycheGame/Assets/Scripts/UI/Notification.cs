using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField]
    private GameObject _messageField;

    public void SetMessage(string message)
    {
        _messageField.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void Accept()
    {
        Destroy(gameObject);
    }
}
