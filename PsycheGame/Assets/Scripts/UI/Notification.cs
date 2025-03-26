using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private GameObject _image;
    [SerializeField] private GameObject _messageField;
   
    private void Rebuild()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void SetImage(Sprite sprite)
    {
        Image image = _image.GetComponent<Image>();
        image.sprite = sprite;
        image.type = Image.Type.Simple;
        image.preserveAspect = true;

        _image.SetActive(true);

        Rebuild();
    }

    public void SetMessage(string message)
    {
        _messageField.GetComponent<TextMeshProUGUI>().text = message;

        _messageField.SetActive(true);

        Rebuild();
    }

    public void Accept()
    {        
        Destroy(gameObject);
    }
}
