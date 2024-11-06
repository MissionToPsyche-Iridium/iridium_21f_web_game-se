using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static T FindComponentInParents<T>(GameObject gameObject) where T : Component
    {
        if (gameObject == null)
        {
            return null;
        }

        T component = gameObject.GetComponent<T>();
        Transform parent = gameObject.transform.parent;

        while (parent != null && component == null)
        {
            component = parent.gameObject.GetComponent<T>();
            parent = parent.parent;
        }

        return component;
    }
}
