using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPrompt : MonoBehaviour
{
    [SerializeField] private GameObject _promptLabel;
    [SerializeField] private GameObject _inputField;

    private Action<string> _confirmCallback = null;

    private void Rebuild()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void SetPrompt(string prompt)
    {
        _promptLabel.GetComponent<TextMeshProUGUI>().text = prompt;
        Rebuild();
    }

    public void SetConfirmCallback(Action<string> confirmCallback)
    {
        _confirmCallback = confirmCallback;
    }

    public void Confirm()
    {
        string input = _inputField.GetComponent<TMP_InputField>().text;
        Destroy(gameObject);
        _confirmCallback(input);
    }
}
