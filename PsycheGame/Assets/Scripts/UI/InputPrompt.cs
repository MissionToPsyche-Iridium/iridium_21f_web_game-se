using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPrompt : MonoBehaviour
{
    [SerializeField] private GameObject _inputField;

    private Action<string> _callback = null;

    private void Rebuild()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void SetPrompt(string prompt)
    {
        _inputField.GetComponent<TMP_InputField>().text = prompt;

        Rebuild();
    }

    public void SetCallback(Action<string> callback)
    {
        _callback = callback;
    }

    public void Confirm()
    {
        string input = _inputField.GetComponent<TMP_InputField>().text;

        Destroy(gameObject);

        _callback(input);
    }
}
