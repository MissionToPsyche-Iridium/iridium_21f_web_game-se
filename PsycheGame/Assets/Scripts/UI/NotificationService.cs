using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationService
{
    private static Notification _currentNotification = null;

    public static Notification Create(string message, Sprite image = null, Action acceptCallback = null)
    {
        if (_currentNotification != null)
        {
            _currentNotification.Accept();
        }

        Notification notification = GameObject.Instantiate(Resources.Load<GameObject>("UI/NotificationCanvas"), GameObject.Find("/MasterCanvas").transform).GetComponent<Notification>();
        notification.SetMessage(message);
        if (image != null)
        {
            notification.SetImage(image);
        }

        notification.SetAcceptCallback(() =>
        {
            _currentNotification = null;
            if (acceptCallback != null)
            {
                acceptCallback();
            }
        });

        _currentNotification = notification;

        return notification;
    }
}
