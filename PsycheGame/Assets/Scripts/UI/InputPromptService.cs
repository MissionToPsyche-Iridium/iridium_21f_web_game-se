using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPromptService
{
    private static GameObject _inputPromptPrefab = Resources.Load<GameObject>("UI/InputPromptCanvas");
    private static GameObject _masterCanvas = GameObject.Find("/MasterCanvas");

    private static Queue<InputPrompt> _queue = new Queue<InputPrompt>();
    private static InputPrompt _currentInputPrompt = null;

    public static void Create(string prompt, Action<string> confirmCallback)
    {
        InputPrompt inputPrompt = GameObject.Instantiate(_inputPromptPrefab, _masterCanvas.transform).GetComponent<InputPrompt>();
        inputPrompt.SetPrompt(prompt);
        inputPrompt.SetConfirmCallback((input) =>
        {
            if (_queue.Count > 0)
            {
                _currentInputPrompt = _queue.Dequeue();
                _currentInputPrompt.gameObject.SetActive(true);
            }
            else
            {
                _currentInputPrompt = null;
            }

            confirmCallback(input);
        });

        if (_currentInputPrompt == null)
        {
            _currentInputPrompt = inputPrompt;
            return;
        }

        inputPrompt.gameObject.SetActive(false);
        _queue.Enqueue(inputPrompt);
    }
}
