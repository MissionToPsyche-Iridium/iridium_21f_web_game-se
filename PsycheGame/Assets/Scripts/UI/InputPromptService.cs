using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPromptService
{
    private static InputPrompt _currentInputPrompt = null;

    public static InputPrompt Create(string prompt, Action<string> confirmCallback)
    {
        if (_currentInputPrompt != null)
        {
            _currentInputPrompt.Confirm();
        }

        InputPrompt inputPrompt = GameObject.Instantiate(Resources.Load<GameObject>("UI/InputPromptCanvas"), GameObject.Find("/MasterCanvas").transform).GetComponent<InputPrompt>();
        inputPrompt.SetPrompt(prompt);

        inputPrompt.SetConfirmCallback((input) =>
        {
            _currentInputPrompt = null;
            confirmCallback(input);
        });

        _currentInputPrompt = inputPrompt;

        return inputPrompt;
    }
}
