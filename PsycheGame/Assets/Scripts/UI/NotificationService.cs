using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationService
{
    private static GameObject _notificationPrefab = Resources.Load<GameObject>("UI/NotificationCanvas");
    private static GameObject _masterCanvas = GameObject.Find("/MasterCanvas");

    private static Notification _currentNotification = null;

    public static void Create(string message, Sprite image = null, Action acceptCallback = null)
    {
        if (_currentNotification != null)
        {
            _currentNotification.Accept();
        }

        _currentNotification = GameObject.Instantiate(_notificationPrefab, _masterCanvas.transform).GetComponent<Notification>();

        _currentNotification.SetAcceptCallback(() =>
        {
            _currentNotification = null;
            if (acceptCallback != null)
            {
                acceptCallback();
            }
        });

        _currentNotification.SetMessage(message);
        if (image != null)
        {
            _currentNotification.SetImage(image);
        }
    }
}
